using UnityEngine;

public class EnemyManager : CharacterManager,IDamageable
{
    public void TakeDamage(float _value)
    {
        Debug.Log($"{gameObject.name}�ܵ�{_value}���˺�");
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
