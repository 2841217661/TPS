using UnityEngine;

public class NormalBulletManager : BulletManager
{
    private Rigidbody rb;

    protected override void Awake()
    {
        base.Awake();
        //rb = GetComponent<Rigidbody>();
    }


    private void OnEnable()
    {
        //if (rb != null)
        //{
        //    rb.linearVelocity = transform.forward * speed;
        //}
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
