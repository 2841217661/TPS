using System.Collections;
using UnityEngine;

public class 火焰印记爆炸 : MonoBehaviour
{

    [SerializeField] private float explodeRadius;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float explodeForce = 300f;

    [Header("相机抖动设置")]
    [SerializeField] private float sharkTime = 0.3f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float checkRadius = 10f; 
    private void Awake()
    {
        Destroy(gameObject, 1);

        //对周围敌人进行检测，调用检测到的敌人的击飞接口方法
        Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRadius, targetLayer);
        foreach(var collider in colliders)
        {
            collider.GetComponent<IKnockUpable>()?.ApplyExplosionForce(transform.position, explodeForce, 10f);

            collider.GetComponent<IDamageable>()?.TakeDamage(100f, GameManager.Instance.playerManager, TakeDamageType.Heavy);
        }

        Vector3 toPlayer = GameManager.Instance.playerManager.transform.position - transform.position;
        if (toPlayer.sqrMagnitude < checkRadius * checkRadius)
        {
            StartCoroutine(SharkCamera(sharkTime));
        }

    }

    private IEnumerator SharkCamera(float duraction)
    {
        GameManager.Instance.playerManager.cameraManager.AddCurrentCameraShark(0.5f, 15);
        yield return new WaitForSeconds(duraction);
        GameManager.Instance.playerManager.cameraManager.RemoveCurrentCameraShark();
    }
}
