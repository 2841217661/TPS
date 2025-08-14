using UnityEngine;

public class 普通魔法球_飞行 : MonoBehaviour
{
    public float moveSpeed = 15f;
    public float impulseForce = 50f; //推力

    private void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {


        PoolManager.Instance.Spawn(PoolManager.Instance.普通魔法球_命中.name, transform.position, transform.rotation);
        PoolManager.Instance.Recycle(gameObject.name, gameObject);

        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * impulseForce, ForceMode.Impulse);
        }
    }
}
