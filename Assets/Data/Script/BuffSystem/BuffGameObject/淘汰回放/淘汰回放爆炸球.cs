using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class 淘汰回放爆炸球 : MonoBehaviour
{
    private CinemachineImpulseSource impulseSource;
    public float impulseForce = 0.5f;

    [SerializeField] private float moveSpeed = 0.5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float explodeRadius = 4;
    [SerializeField] private float explodeForce = 500;
    private Vector3 offset = Vector3.up;

    private Transform target;

    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Start()
    {
        target = GameManager.Instance.playerManager.transform;
    }

    private void Update()
    {
        Vector3 targetPoint = target.position + offset;
        Vector3 moveDir = targetPoint - transform.position;

        // 移动
        transform.position += moveDir.normalized * moveSpeed * Time.deltaTime;

        // 旋转
        if (moveDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        //距离玩家很近时自动回收(不进行爆炸)
        if (moveDir.sqrMagnitude < 0.5)
        {
            PoolManager.Instance.Recycle(PoolManager.Instance.淘汰回放爆炸.name, this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRadius);

        PoolManager.Instance.Spawn(PoolManager.Instance.淘汰回放爆炸.name, transform.position, transform.rotation);

        if (colliders.Length > 0)
        {
            foreach (var collider in colliders)
            {
                if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    collider.transform.GetComponent<IKnockUpable>()?.ApplyExplosionForce(transform.position, explodeForce, explodeRadius);
                    collider.transform.GetComponent<IDamageable>()?.TakeDamage(500f, GameManager.Instance.playerManager,collider.transform.position + Vector3.up, DamageIntensity.Heavy,DamageElement.Fire);
                }
            }
        }

        GameManager.Instance.playerManager.cameraManager.ApplyImpluseCameraShark(impulseSource, impulseForce);
        PoolManager.Instance.Recycle(PoolManager.Instance.淘汰回放爆炸.name, this.gameObject);
    }
}
