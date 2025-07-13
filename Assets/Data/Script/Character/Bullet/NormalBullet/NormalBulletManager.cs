using UnityEngine;

public class NormalBulletManager : BulletManager
{
    private Rigidbody rb;

    protected override void Awake()
    {
        base.Awake();
        //rb = GetComponent<Rigidbody>();
    }




    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
