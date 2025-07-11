using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("角色基本信息")]
    public string characterName;
    public string characterTitle;

    #region 属性值
    [Header("移动速度倍率")]
    public float defaultMoveSpeedMul; //初始移动倍率
    public float currentMoveSpeedMul; //当前移动倍率

    [Header("攻击力倍率")]
    public float defaultAttackPowerMul;
    public float currentAttackPowerMul;

    [Header("生命值上限倍率")]
    public float defaultHealthMul;
    public float currentHealthMul;
    public float maxHealthValue;
    public float currentHealthValue;
    #endregion

    public BuffSystem buffSystem;

    protected virtual void Awake()
    {
        buffSystem = new BuffSystem(this);
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {

    }
}
