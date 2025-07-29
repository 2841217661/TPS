using UnityEngine;

//受击程度
public enum DamageIntensity
{
    Light, //轻击
    Middle, //中击
    Heavy, //重击
}
//受到伤害的具体类型
public enum DamageElement
{
    Physical, //物理伤害
    Fire, //火焰伤害
}
public interface IDamageable
{
    void TakeDamage(float _value, CharacterManager _source, Vector3 _damagePositin, DamageIntensity _intensityType, DamageElement _elementType);
}
