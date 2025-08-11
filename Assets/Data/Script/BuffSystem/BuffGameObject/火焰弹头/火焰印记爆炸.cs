using Cinemachine;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class 火焰印记爆炸 : MonoBehaviour,IPoolable
{
    private CinemachineImpulseSource impulseSource;
    [SerializeField] private float explosionForce = 0.5f;
    [SerializeField] private float explodeRadius = 3;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float explodeForce = 300f;

    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void OnRecycle()
    {
        
    }

    public void OnSpawn()
    {
        //对周围敌人进行检测，调用检测到的敌人的击飞接口方法
        Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRadius, targetLayer);
        foreach (var collider in colliders)
        {
            collider.GetComponent<IKnockUpable>()?.ApplyExplosionForce(transform.position, explodeForce, explodeRadius);

            collider.GetComponent<IDamageable>()?.TakeDamage(100f, GameManager.Instance.playerManager,collider.transform.position + Vector3.up, DamageIntensity.Heavy,DamageElement.Fire);
        }

        GameManager.Instance.playerManager.cameraManager.ApplyImpluseCameraShark(impulseSource, explosionForce);
    }
}
