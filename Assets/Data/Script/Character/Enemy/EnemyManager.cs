using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : CharacterManager,IDamageable
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

    public void TakeDamage(float _value, CharacterManager _source, TakeDamageType _type)
    {
        Debug.Log($"{gameObject.name}受到来自{_source}的{_value}点伤害，伤害类型:{_type}");
        if(_type == TakeDamageType.Heavy)
        {
            state = EnemyState.Damage;
        }
        currentHealthValue -= _value;
    }

    protected override void Awake()
    {
        base.Awake();

        currentHealthValue = maxHealthValue;

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

    }


    public override void OnDeath()
    {
        base.OnDeath();
        state = EnemyState.Chase;
        EventManager.Instance.enemyEvent.Death_Enemy(this);
        Destroy(gameObject);
    }
}
