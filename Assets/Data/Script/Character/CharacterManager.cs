using System;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("角色基本信息")]
    public string characterName;
    public string characterTitle;

    [Header("生命值相关")]
    public float baseHealthValue;
    public float maxHealthValue;
    [SerializeField] private float m_currentHealthValue;
    public float currentHealthValue
    {
        get { return m_currentHealthValue; }
        set
        {
            if(value > maxHealthValue)
            {
                value = maxHealthValue;
            }

            m_currentHealthValue = value;
            OnHealthValueChanged();
        }
    }

    protected virtual void OnHealthValueChanged()
    {

    }

    [Header("攻击相关")]
    public float baseAttackValue;
    public float maxAttackValue;
    public float currentAttackValue;

    public BuffSystem buffSystem;

    /// <summary>
    /// 受伤事件：伤害值、伤害具体类型、是否是暴击伤害
    /// </summary>
    public Action<float,Vector3, DamageElement, bool> onDamageEvent;
    public void DamageEvent(float _damageValue,Vector3 _damagePosition, DamageElement _element, bool _isCritical)
    {
        onDamageEvent?.Invoke(_damageValue, _damagePosition, _element,_isCritical);
    }

    protected virtual void OnEnable()
    {
        
    }

    protected virtual void OnDisable()
    {

    }

    protected virtual void Awake()
    {
        maxHealthValue = baseHealthValue;
        currentHealthValue = maxHealthValue;

        maxAttackValue = baseAttackValue;
        currentAttackValue = baseAttackValue;
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {

    }

    public virtual void OnDeath()
    {

    }
}
