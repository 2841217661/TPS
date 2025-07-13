using UnityEngine;

public class TrackBulletManager : BulletManager
{
    //当跟踪子弹发射后，有两个阶段，第一阶段沿着直线飞行trackDefaultTime,然后再进行跟踪
    [Header("追踪子弹")]
    public float trackDefaultTime = 0.1f;
    public LayerMask trackTargetLayer; //可追踪的物体层级
    public float rotateSpeed;
    private float trackTimer;
    private Transform tractTargetTransform; //被追踪的目标

    private BulletState bulletState = BulletState.normal;


    protected override void Awake()
    {
        base.Awake();

        trackTimer = trackDefaultTime;
    }


    protected override void Update()
    {
        base.Update();

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();



        trackTimer -= Time.fixedDeltaTime;
        if(trackTimer <= 0f)
        {
            bulletState = BulletState.track;
        }

        switch (bulletState)
        {
            case BulletState.normal:
                transform.position = transform.position + transform.forward * speed * Time.fixedDeltaTime;
                break;
            case BulletState.track:
                if(tractTargetTransform == null)
                {
                    transform.position = transform.position + transform.forward * speed * Time.fixedDeltaTime;
                }
                else
                {
                    // 1. 计算目标方向
                    Vector3 dirToTarget = (tractTargetTransform.position - transform.position).normalized;

                    // 2. 计算目标旋转
                    Quaternion targetRotation = Quaternion.LookRotation(dirToTarget);

                    // 3. 平滑旋转
                    transform.rotation = Quaternion.RotateTowards(
                        transform.rotation,
                        targetRotation,
                        rotateSpeed * Time.fixedDeltaTime
                    );

                    // 4. 前进
                    transform.position = transform.position + transform.forward * speed * Time.fixedDeltaTime;
                }
                break;
            default:
                Debug.LogWarning("类型不存在");
                break;
        }
    }

    private Transform CheckCloseEnemy(float _radius)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius, trackTargetLayer);

        if (colliders.Length == 0)
            return null;

        CharacterManager closestCharacterManager = null;
        float minDistance = float.MaxValue;

        for (int i = 0; i < colliders.Length; i++)
        {
            Vector3 dirToTarget = colliders[i].transform.position - transform.position;

            // 判断是否在前方180°
            if (Vector3.Dot(transform.forward, dirToTarget.normalized) < 0)
            {
                // 在后方，跳过
                continue;
            }

            float dis = dirToTarget.magnitude;

            if (dis < minDistance)
            {
                minDistance = dis;
                closestCharacterManager = colliders[i].GetComponent<CharacterManager>();
            }
        }

        if (closestCharacterManager != null)
            return closestCharacterManager.transform;

        return null;
    }

    public override void OnRecycle()
    {
        base.OnRecycle();

        tractTargetTransform = null;
    }

    public override void OnSpawn()
    {
        base.OnSpawn();

        tractTargetTransform = CheckCloseEnemy(100f);
    }
}
