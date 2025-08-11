using System;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("角色基本信息")]
    public string characterName;
    public string characterTitle;

    [Header("生命值相关")]
    public float baseHealthValue;
    [SerializeField] private float m_maxHealthValue;
    public float maxHealthValue
    {
        get { return m_maxHealthValue; }
        set
        {
            //注意：目前没有考虑到生命上限降低的情况
            float changeValue = value - maxHealthValue; //增加的生命上限值
            //最大生命值增加时，例如增加10，那么当前生命值也会增加10
            m_maxHealthValue = value;
            currentHealthValue += changeValue;
        }
    }
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
            else if(value <= 0)
            {
                value = 0f;
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
