using UnityEngine;

public class Enemy_StoneMan : Enemy_Ordinary
{
    public float baseMoveSpeed;
    public float runSpeed;
    protected override void Awake()
    {
        base.Awake();

        baseMoveSpeed = moveSpeed;
        runSpeed = moveSpeed * 2f;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        //注册发现player事件回调
        onSuccessCheckPlayerIsAround += OnFindPlayer;

        //注册失败发现player事件回调
        onFailureCheckPlayerIsAround += OnFailurePlayer;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        onSuccessCheckPlayerIsAround -= OnFindPlayer;

        onFailureCheckPlayerIsAround -= OnFailurePlayer;
    }

    private void OnFindPlayer()
    {
        //发现玩家后：
        Debug.Log("石头人发现玩家");
        animator.SetFloat("IsAttack", 1f); //进入战斗状态的idle改变
        animator.SetFloat("Walk", 1f);

        //增加移动速度
        moveSpeed = runSpeed;
    }

    private void OnFailurePlayer()
    {
        //未发现玩家后：
        animator.SetFloat("IsAttack", 0f); //进入normal状态的idle改变
        animator.SetFloat("Walk", 0f);

        //降低移动速度
        moveSpeed = baseMoveSpeed;
    }

    public override void TakeDamage(float _value, CharacterManager _source, Vector3 _damagePosition, DamageIntensity _type, DamageElement _element)
    {
        if (state == EnemyState.Death) return;

        if (state == EnemyState.Patrol)
        {
            //此时应该发现玩家
            target = GameManager.Instance.playerManager.transform;
            //播放警戒语音
            PoolManager.Instance.Spawn(PoolManager.Instance.sx_石头人_警戒.name, transform.position, Quaternion.identity, true);
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

    public override void ApplyExplosionForce(Vector3 explosionPosition, float force, float radius, float upwardModifier = 0.5F)
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.AddExplosionForce(force, explosionPosition, radius, upwardModifier, ForceMode.Impulse);
        }
    }

    public override void OnDeath()
    {
        base.OnDeath();

        PoolManager.Instance.Spawn(PoolManager.Instance.sx_石头人_死亡.name, transform.position, transform.rotation);
        Destroy(gameObject,2f);
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        #region 绘制普通攻击范围
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + transform.rotation * normalAttackoffset, normalAttackRadius);
        #endregion
    }

    #region 自己的动画事件
    public void Play_SX_挥拳()
    {
        PoolManager.Instance.Spawn(PoolManager.Instance.sx_石头人_挥拳.name, transform.position, Quaternion.identity, true);
    }

    public void Play_SX_锤击()
    {
        PoolManager.Instance.Spawn(PoolManager.Instance.sx_石头人_锤击.name, transform.position, Quaternion.identity, true);
    }

    public void Play_SX_KnockBack()
    {
        PoolManager.Instance.Spawn(PoolManager.Instance.sx_石头人_被击退.name, transform.position, Quaternion.identity, true);
    }

    [Header("普通攻击设置")]
    public float normalAttackRadius; //普通攻击范围半径
    public Vector3 normalAttackoffset; //以原点 + offset 为中心
    public void TryNormalAttack()
    {
        Collider[] collisions = Physics.OverlapSphere(transform.position + transform.rotation * normalAttackoffset, normalAttackRadius, targetLayer);
        if (collisions.Length > 0) //敌人在范围内
        {
            collisions[0].GetComponent<IDamageable>()?.TakeDamage(currentAttackValue, this, Vector3.zero, DamageIntensity.Middle, DamageElement.Physical);
        }
    }
    #endregion
}