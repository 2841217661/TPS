using UnityEngine;
using UnityEngine.AI;

public class Enemy_Ordinary : EnemyManager,IKnockUpable
{
    public float backResistance; //击退抗性
    private float currentBackResistance;

    //检测范围分为两个检测
    //1：前方一定视角
    //2：周围近距离
    [Header("玩家检测范围")]
    public float forwardCheckDistance;
    public float forwardCheckAngle; //前方检测可见角度(左右都有，例如30°表示前方左侧30° - 右侧30°)
    public float roundCheckRaious; //周围检测半径
    public Vector3 checkOffset; //只能改变y的值
    private float checkInterval = 1f;  // 每隔1秒检测一次
    private float checkTimer = 0f;



    public void TakeDamage(float _value, CharacterManager _source, Vector3 _damagePosition, DamageIntensity _type, DamageElement _element)
    {
        if (state == EnemyState.Death) return;

        if (state == EnemyState.Patrol)
        {
            //此时应该发现玩家
            target = GameManager.Instance.playerManager.transform;
            //播放警戒语音
            PoolManager.Instance.Spawn(PoolManager.Instance.sx_僵尸_警戒.name, transform.position, Quaternion.identity, true);
        }

        //判断是否发生暴击
        if (Random.Range(0, 1f) < GameManager.Instance.playerManager.currentCrticalMul)
        {
            DamageEvent(_value * 2f, _damagePosition, _element, true);
        }
        else
        {
            DamageEvent(_value, _damagePosition, _element, false);
        }

        switch (_type)
        {
            case DamageIntensity.Light: //持续受到轻击会被击退
                currentBackResistance -= _value;
                if (currentBackResistance <= 0f)
                {
                    state = EnemyState.KnockBack;
                    currentBackResistance = backResistance;
                }
                break;
            case DamageIntensity.Middle: //受到较重的攻击会被击退
                state = EnemyState.KnockBack;
                currentBackResistance = backResistance;
                break;
            case DamageIntensity.Heavy: //受到重击会被击飞
                state = EnemyState.KnockUp;
                currentBackResistance = backResistance;
                break;
        }

        currentHealthValue -= _value;
        if (currentHealthValue <= 0f)
        {
            state = EnemyState.Death;
        }
    }



    protected override void OnEnable()
    {
        base.OnEnable();

        //注册受伤伤害数字飘动事件
        onDamageEvent += DamageEvent_DisplayDamageTextEffect;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        onDamageEvent -= DamageEvent_DisplayDamageTextEffect;
    }

    private void DamageEvent_DisplayDamageTextEffect(float _damageValue, Vector3 _damagePosition, DamageElement _element, bool _isCritical)
    {
        Debug.Log($"受到{_damageValue}伤害，类型为{_element},发生暴击？{_isCritical}");
        GameManager.Instance.GenerateDamageTextEffect(_damageValue, _damagePosition, _element, _isCritical);
    }

    protected override void Awake()
    {
        base.Awake();

        //target = GameManager.Instance.playerManager.transform;

        currentBackResistance = backResistance;

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        agent.updatePosition = false;   // 不自动更新 Transform 位置
        agent.updateRotation = false;   // 不自动更新 Transform 旋转

    }

    public override void OnDeath()
    {
        base.OnDeath();

        PoolManager.Instance.Spawn(PoolManager.Instance.zombie_death_explode.name, transform.position + Vector3.up, transform.rotation);
        PoolManager.Instance.Spawn(PoolManager.Instance.sx_僵尸_死亡爆炸.name, transform.position, transform.rotation);
    }



    protected override void Update()
    {
        base.Update();

        CheckPlayerIsAround(target);
    }



    /// <summary>
    /// 尝试发现玩家
    /// </summary>
    protected virtual void CheckPlayerIsAround(Transform _currentTarget)
    {
        if (_currentTarget != null)
        {
            checkTimer = 0f;
            return;
        }

        checkTimer += Time.deltaTime;
        if (checkTimer < checkInterval)
        {
            return;
        }
        checkTimer = 0f;

        Transform player = GameManager.Instance.playerManager.transform;
        Vector3 origin = transform.position + checkOffset;
        Vector3 playerPos = player.position + checkOffset;
        Vector3 dirToPlayer = playerPos - origin;
        float distanceToPlayer = dirToPlayer.magnitude;

        // 距离超过最大范围，直接返回
        if (distanceToPlayer > forwardCheckDistance)
        {
            return;
        }

        // 检测玩家在近距离
        Collider[] targets = Physics.OverlapSphere(origin, roundCheckRaious, targetLayer);
        if (targets.Length > 0)
        {
            target = targets[0].transform;
        }

        // 前方扇形视野检测:直接由检测点向玩家发射射线，如果第一个命中了玩家，说明看见了玩家
        if (Physics.Raycast(origin, GameManager.Instance.playerManager.transform.position - transform.position, out RaycastHit hit, forwardCheckDistance))
        {
            if (hit.transform == player && Vector3.Angle(transform.forward, hit.transform.position - transform.position) <= forwardCheckAngle)
            {
                target = player;
                return;
            }
        }
    }

    /// <summary>
    /// 绘制检测范围
    /// </summary>
    protected virtual void OnDrawGizmosSelected()
    {
        #region 绘制警戒范围
        // 绘制颜色
        Color forwardColor = new Color(0, 1, 0, 0.3f); //绿色
        Color aroundColor = new Color(1, 0, 0, 0.2f);  //红色

        Vector3 origin = transform.position + checkOffset;

        // 1. 绘制前方检测扇形
        Vector3 forward = transform.forward;
        float halfAngle = forwardCheckAngle;

        int segments = 10; //扇形分段数
        float angleStep = (halfAngle * 2) / segments;

        Vector3 prevPoint = origin + Quaternion.Euler(0, -halfAngle, 0) * forward * forwardCheckDistance;

        for (int i = 1; i <= segments; i++)
        {
            float angle = -halfAngle + angleStep * i;
            Vector3 nextPoint = origin + Quaternion.Euler(0, angle, 0) * forward * forwardCheckDistance;

            //画线
            Gizmos.color = forwardColor;
            Gizmos.DrawLine(origin, nextPoint);
            Gizmos.DrawLine(prevPoint, nextPoint);

            prevPoint = nextPoint;
        }

        // 2. 绘制周围近距离检测范围
        Gizmos.color = aroundColor;
        Gizmos.DrawWireSphere(origin, roundCheckRaious);
        #endregion

        #region 绘制普通攻击范围
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + transform.rotation * normalAttackoffset, normalAttackRadius);
        #endregion
    }


    #region 动画事件
    public void Play_SX_攻击()
    {
        PoolManager.Instance.Spawn(PoolManager.Instance.sx_僵尸_攻击.name, transform.position, Quaternion.identity, true);
    }

    [Header("普通攻击设置")]
    public float normalAttackRadius; //普通攻击范围半径
    public Vector3 normalAttackoffset; //以原点 + offset 为中心
    public void TryNormalAttack()
    {
        Collider[] collisions = Physics.OverlapSphere(transform.position + transform.rotation * normalAttackoffset, normalAttackRadius, targetLayer);
        if (collisions.Length > 0) //敌人在范围内
        {
            collisions[0].GetComponent<IDamageable>()?.TakeDamage(attackPower, this, Vector3.zero, DamageIntensity.Light, DamageElement.Physical);
        }
    }

    public void ApplyExplosionForce(Vector3 explosionPosition, float force, float radius, float upwardModifier = 0.5F)
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.AddExplosionForce(force, explosionPosition, radius, upwardModifier, ForceMode.Impulse);
        }
    }

    #endregion
}
