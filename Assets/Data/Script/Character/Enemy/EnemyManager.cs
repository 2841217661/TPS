using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : CharacterManager, IDamageable, IKnockUpable
{
    public EnemyState state;

    [HideInInspector] public NavMeshAgent agent; //寻路代理
    [HideInInspector] public Animator animator;
    [HideInInspector] public Transform currentPatrolPoint; //当前巡逻目标点

    [HideInInspector] public Rigidbody rb;
    [SerializeField] private Transform m_target;//需要攻击的对象
    public Transform target{
        get { return m_target; }
        set
        {
            if(value != null)
            {
                SuccessCheckPlayerIsAround();
            }
            else
            {
                FailureCheckPlayerIsAround();
            }
            m_target = value;
        }
    }
    public LayerMask targetLayer; //可攻击的对象层级
    public float reachDistance; //到达可攻击玩家的距离
    public float moveSpeed;
    public float rotateSpeed;

    private GameObject healthBar;


    protected override void OnEnable()
    {
        base.OnEnable();

        //注册受伤伤害数字飘动事件
        onDamageEvent += DamageEvent_DisplayDamageTextEffect;

        //注册玩家死亡事件：玩家死亡时，进入胜利姿态
        EventManager.Instance.playerEvent.onPlayerDeath += PlayerDeath;

    }

    protected override void OnDisable()
    {
        base.OnDisable();

        onDamageEvent -= DamageEvent_DisplayDamageTextEffect;

        EventManager.Instance.playerEvent.onPlayerDeath -= PlayerDeath;
    }

    private void PlayerDeath()
    {
        //进入胜利姿态
        state = EnemyState.Vectory;
    }

    protected Action onSuccessCheckPlayerIsAround; //成功检测到玩家时的事件
    protected void SuccessCheckPlayerIsAround()
    {
        onSuccessCheckPlayerIsAround?.Invoke();
    }

    protected Action onFailureCheckPlayerIsAround; //失败检测到玩家时的事件
    protected void FailureCheckPlayerIsAround()
    {
        onFailureCheckPlayerIsAround?.Invoke();
    }

    private void DamageEvent_DisplayDamageTextEffect(float _damageValue, Vector3 _damagePosition, DamageElement _element, bool _isCritical)
    {
        Debug.Log($"受到{_damageValue}伤害，类型为{_element},发生暴击？{_isCritical}");
        GameManager.Instance.GenerateDamageTextEffect(_damageValue, _damagePosition, _element, _isCritical);
    }

    protected override void Awake()
    {
        base.Awake();

        healthBar = transform.Find("HealthBar").gameObject;

        rb = GetComponent<Rigidbody>();

        buffSystem = new BuffSystem(this);
    }

    protected override void Start()
    {
        base.Start();

        //buffSystem.AddBuff<B_火焰弹头>();
    }

    protected override void Update()
    {
        base.Update();

        if (state == EnemyState.Death) return;

        buffSystem.Update();

        GroundCheck();

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        buffSystem.FixedUpdate();
    }

    public override void OnDeath()
    {
        base.OnDeath();

        gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
        healthBar.SetActive(false);

        EventManager.Instance.enemyEvent.Death_Enemy(this);

    }

    [Header("地面检测")]
    public bool isGrounded;
    public LayerMask groundLayer; //检测层级
    public float groundCheckSphereRadius; //使用球型检测，检测的半径,与capsuleCollision半径一致最为合适
    private float airTime = 0.1f;
    private float airTimer;
    protected virtual void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckSphereRadius, groundLayer);
        if (!isGrounded)
        {
            //state = EnemyState.KnockUp;
            airTimer += Time.deltaTime;
            if (airTimer > airTime)
            {
                state = EnemyState.KnockUp;
                airTimer = 0f;
            }
        }
        else
        {
            airTimer = 0f;
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        #region 绘制地面检测
        // 绘制地面检测范围
        Gizmos.color = Color.gray;
        Gizmos.DrawSphere(transform.position, groundCheckSphereRadius); // 实心球
        #endregion
    }

    //受击方法接口
    public virtual void TakeDamage(float _value, CharacterManager _source, Vector3 _damagePositin, DamageIntensity _intensityType, DamageElement _elementType)
    {
        if (state == EnemyState.Death) return;
    }

    //受到爆炸冲击方法接口
    public virtual void ApplyExplosionForce(Vector3 explosionPosition, float force, float radius, float upwardModifier = 0.5F)
    {
        if (state == EnemyState.Death) return;
    }
}
