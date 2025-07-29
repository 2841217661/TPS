using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : CharacterManager,IKnockUpable
{
    public EnemyState state;

    [HideInInspector] public NavMeshAgent agent; //寻路代理
    [HideInInspector] public Animator animator;
    [HideInInspector] public Transform currentPatrolPoint; //当前巡逻目标点

    [HideInInspector] public Rigidbody rb;
    public Transform target; //需要攻击的对象
    public LayerMask targetLayer; //可攻击的对象层级
    public float reachDistance; //到达可攻击玩家的距离
    public float moveSpeed;
    public float rotateSpeed;

    [Header("攻击设置")]
    public float attackPower;

    

    protected override void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody>();

        buffSystem = new BuffSystem(this);

        //Minmap.Instance.AddMinmapIcon(this.transform, MinmapIconType.enemy);
    }

    protected override void Start()
    {
        base.Start();

    }

    protected override void Update()
    {
        GroundCheck();

        base.Update();


        buffSystem.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        buffSystem.FixedUpdate();
    }

    public override void OnDeath()
    {
        base.OnDeath();

        EventManager.Instance.enemyEvent.Death_Enemy(this);
        Destroy(gameObject);
    }

    [Header("地面检测")]
    public bool isGrounded;
    public LayerMask groundLayer; //检测层级
    public float groundCheckSphereRadius; //使用球型检测，检测的半径,与capsuleCollision半径一致最为合适
    private float airTime = 0.1f;
    private float airTimer;
    private void GroundCheck()
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

    public void ApplyExplosionForce(Vector3 explosionPosition, float force, float radius, float upwardModifier = 0.5F)
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; 
            rb.AddExplosionForce(force, explosionPosition, radius, upwardModifier, ForceMode.Impulse);
        }
    }


}
