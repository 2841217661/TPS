using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("��ɫ������Ϣ")]
    public string characterName;
    public string characterTitle;

    #region ����ֵ
    [Header("�ƶ��ٶȱ���")]
    public float defaultMoveSpeedMul; //��ʼ�ƶ�����
    public float currentMoveSpeedMul; //��ǰ�ƶ�����

    [Header("����������")]
    public float defaultAttackPowerMul;
    public float currentAttackPowerMul;

    [Header("����ֵ���ޱ���")]
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
