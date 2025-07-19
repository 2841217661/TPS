using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : CharacterManager
{
    public EnemyState state;

    [HideInInspector] public NavMeshAgent agent; //寻路代理
    [HideInInspector] public Animator animator;

    public Transform target; //需要攻击的对象
    public LayerMask targetLayer; //可攻击的对象层级
    public float reachDistance; //到达可攻击玩家的距离
    public float moveSpeed;
    public float rotateSpeed;
    public Transform patrolPointParent;
    [HideInInspector] public Transform[] patrolPoint; //巡逻数组点

    protected override void Awake()
    {
        base.Awake();

        buffSystem = new BuffSystem(this);

        patrolPoint = new Transform[patrolPointParent.childCount];
        for(int i = 0; i < patrolPointParent.childCount; i++)
        {
            patrolPoint[i] = patrolPointParent.GetChild(i);
        }
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
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
        state = EnemyState.Chase;
        EventManager.Instance.enemyEvent.Death_Enemy(this);
        Destroy(gameObject);
    }
}
