using UnityEngine;

//受击类型
public enum TakeDamageType
{
    Light, //轻击
    Middle, //中击
    Heavy, //重击
    Fire, //灼烧

}
public interface IDamageable
{
    void TakeDamage(float _value, CharacterManager _source, TakeDamageType _type);
}
