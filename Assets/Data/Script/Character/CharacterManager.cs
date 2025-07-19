using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("角色基本信息")]
    public string characterName;
    public string characterTitle;

    [Header("生命值相关")]
    public float baseHealthValue;
    public float maxHealthValue;
    public float currentHealthValue;

    [Header("攻击相关")]
    public float baseAttackValue;
    public float maxAttackValue;
    public float currentAttackValue;

    public BuffSystem buffSystem;

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
