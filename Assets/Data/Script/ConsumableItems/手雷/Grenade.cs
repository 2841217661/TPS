using Cinemachine;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class Grenade : MonoBehaviour
{
    public GameObject exploreEffectPre;
    public float impluseForce = 0.5f;

    private CinemachineImpulseSource impulseSource;
    

    [SerializeField] private float explodeTime = 3f;
    private float explodeTimer = 0f;
    private float explodeRadius = 8f;
    [SerializeField] private float explodeForce = 800f;

    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();

        //实例后1s不会与player发生碰撞
        Collider genade = GetComponent<Collider>();
        Collider player1 = GameManager.Instance.playerManager.GetComponent<CapsuleCollider>();
        Collider player2 = GameManager.Instance.playerManager.GetComponent<CharacterController>();
        Physics.IgnoreCollision(genade, player1, true);
        Physics.IgnoreCollision(genade, player2, true);
        StartCoroutine(EnableCollisionLater(genade, player1, 1f));
        StartCoroutine(EnableCollisionLater(genade, player2, 1f));
    }

    private IEnumerator EnableCollisionLater(Collider a, Collider b, float delay)
    {
        yield return new WaitForSeconds(delay);
        Physics.IgnoreCollision(a, b, false);
    }


    private void Update()
    {
        explodeTimer += Time.deltaTime;
        if (explodeTimer > explodeTime)
        {
            //对周围敌人造成一次爆炸伤害，并摧毁自己
            Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRadius);
            foreach (Collider collider in colliders)
            {
                collider.GetComponent<IKnockUpable>()?.ApplyExplosionForce(transform.position, explodeForce, explodeRadius, 1f);
            }

            var obj = Instantiate(exploreEffectPre, transform.position, Quaternion.identity);
            obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y + 6f, obj.transform.position.z);
            GameManager.Instance.playerManager.cameraManager.ApplyImpluseCameraShark(impulseSource, impluseForce);
            Destroy(gameObject);
        }
    }
}
