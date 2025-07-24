using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : CharacterManager,IKnockUpable
{
    public EnemyState state;

    [HideInInspector] public NavMeshAgent agent; //寻路代理
    [HideInInspector] public Animator animator;
    [HideInInspector] public Transform currentPatrolPoint; //当前巡逻目标点

    public Transform target; //需要攻击的对象
    public LayerMask targetLayer; //可攻击的对象层级
    public float reachDistance; //到达可攻击玩家的距离
    public float moveSpeed;
    public float rotateSpeed;
    

    protected override void Awake()
    {
        base.Awake();

        buffSystem = new BuffSystem(this);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        GroundCheck();

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
    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckSphereRadius, groundLayer);
    }

    public void ApplyExplosionForce(Vector3 explosionPosition, float force, float radius, float upwardModifier = 0.5F)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; 
            rb.AddExplosionForce(force, explosionPosition, radius, upwardModifier, ForceMode.Impulse);
        }
    }
}
