using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject exploreEffectPre;

    [SerializeField] private float explodeTime = 3f;
    private float explodeTimer = 0f;
    private float explodeRadius = 8f;
    private float explodeForce = 800f;


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
            Destroy(gameObject);
        }
    }
}
