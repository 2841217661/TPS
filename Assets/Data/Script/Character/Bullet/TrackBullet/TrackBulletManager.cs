using UnityEngine;

public class TrackBulletManager : BulletManager
{
    //�������ӵ�������������׶Σ���һ�׶�����ֱ�߷���trackDefaultTime,Ȼ���ٽ��и���
    [Header("׷���ӵ�")]
    public float trackDefaultTime = 0.1f;
    public LayerMask trackTargetLayer; //��׷�ٵ�����㼶
    public float rotateSpeed;
    private float trackTimer;
    private Transform tractTargetTransform; //��׷�ٵ�Ŀ��

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
                    // 1. ����Ŀ�귽��
                    Vector3 dirToTarget = (tractTargetTransform.position - transform.position).normalized;

                    // 2. ����Ŀ����ת
                    Quaternion targetRotation = Quaternion.LookRotation(dirToTarget);

                    // 3. ƽ����ת
                    transform.rotation = Quaternion.RotateTowards(
                        transform.rotation,
                        targetRotation,
                        rotateSpeed * Time.fixedDeltaTime
                    );

                    // 4. ǰ��
                    transform.position = transform.position + transform.forward * speed * Time.fixedDeltaTime;
                }
                break;
            default:
                Debug.LogWarning("���Ͳ�����");
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

            // �ж��Ƿ���ǰ��180��
            if (Vector3.Dot(transform.forward, dirToTarget.normalized) < 0)
            {
                // �ں󷽣�����
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
