using UnityEditor;
using UnityEngine;
using UnityEngine.AI;


public class PetManager : CharacterManager
{
    public PetState state;
    [HideInInspector] public Animator animator;
    [HideInInspector] public NavMeshAgent agent;

    [Header("战斗设置")]
    public Transform attackPoint; //攻击点，当进行攻击检测时，以该点为中心
    public LayerMask attackLayer; //攻击对象的层级
    [HideInInspector] public Transform followTarget; //跟随目标
    public Transform attackTarget; //攻击目标
    public float nearDistance;
    public float midDistance;
    public float farDistance;


    protected override void Awake()
    {
        base.Awake();

        followTarget = GameManager.Instance.playerManager.transform;

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }


    protected virtual void OnDrawGizmos()
    {
        #region 绘制范围
        //绘制近距离
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, nearDistance);
        //绘制中距离
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, midDistance);
        //绘制远距离
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, farDistance);
        #endregion
    }


}
