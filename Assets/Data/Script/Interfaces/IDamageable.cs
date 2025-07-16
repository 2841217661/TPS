using UnityEngine;

//ÊÜ»÷ÀàĞÍ
public enum TakeDamageType
{
    Light,
    Middle,
    Heavy,
}
public interface IDamageable
{
    void TakeDamage(float _value, CharacterManager _source, TakeDamageType _type);
}
