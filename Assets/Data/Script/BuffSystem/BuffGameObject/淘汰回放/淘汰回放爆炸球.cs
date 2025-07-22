using UnityEngine;

public class 淘汰回放爆炸球 : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float explodeRadius = 10;
    [SerializeField] private float explodeForce = 500f;
    public GameObject exploedEffect;
    private Vector3 offset = Vector3.up;

    private Transform target;

    private void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.playerManager != null)
        {
            target = GameManager.Instance.playerManager.transform;
        }
        else
        {
            Debug.LogWarning("PlayerManager 未找到，爆炸球目标为空！");
            enabled = false;
        }
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
    }

    private void OnTriggerEnter(Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRadius);

        Instantiate(exploedEffect, transform.position, transform.rotation);

        if (colliders.Length > 0)
        {
            foreach (var collider in colliders)
            {
                if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    collider.transform.GetComponent<IKnockUpable>()?.ApplyExplosionForce(transform.position, explodeForce, explodeRadius);
                    collider.transform.GetComponent<IDamageable>()?.TakeDamage(500f, GameManager.Instance.playerManager, TakeDamageType.Heavy);
                }
            }
        }

        Destroy(gameObject);

    }
}
