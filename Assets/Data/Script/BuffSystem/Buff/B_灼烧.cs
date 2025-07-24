using UnityEngine;

public class B_灼烧 : BuffBase
{
    private float time = 0.2f; //每0.2s造成一次伤害
    private float damageValue = 1f; //每次受到1点伤害
    private float timer;
    public float onceDamageValue = 20f; //触碰一次造成的伤害
    private GameObject vfx_fire; //灼烧特效
    public override void Update()
    {
        base.Update();


        timer += Time.deltaTime;
        if(timer > time)
        {
            timer = 0f;

            DamageCharacter(damageValue);
        }
    }

    public override void AfterBeAdded()
    {
        base.AfterBeAdded();

        //每次触碰会刷新持续时间，并且立即造成一次伤害
        DamageCharacter(onceDamageValue);

        vfx_fire = PoolManager.Instance.Spawn(PoolManager.Instance.灼烧.name, characterManager.transform.position + Vector3.up, Quaternion.identity);
        vfx_fire.transform.SetParent(characterManager.transform);
    }

    public override void AfterBeRemoved()
    {
        base.AfterBeRemoved();

        PoolManager.Instance.Recycle(vfx_fire.name, vfx_fire,PoolManager.Instance.transform.Find("VFXPool/Pool_VX_Buff_灼烧"));
    }

    private void DamageCharacter(float _value)
    {
        //判断characterManager是否实现了受击接口
        if(characterManager is IDamageable damageable)
        {
            damageable.TakeDamage(_value, characterManager, TakeDamageType.Fire);
        }
    }
}
