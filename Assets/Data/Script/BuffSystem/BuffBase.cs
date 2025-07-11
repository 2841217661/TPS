using System;
using UnityEngine;

public class BuffBase
{
    //�޲ι���
    public BuffBase() { }

    public BuffData BuffData { get; private set; } = null;

    // ����ʱ������
    public float DurationScale { get; set; } = 1;

    private int m_CurrentLevel = 0;
    private float m_ResidualDuration = 0;
    private int m_TimeFreeze = 0;

    private bool m_Initialized = false;

    public bool TimeFreeze
    {
        get { return m_TimeFreeze == 0; }
        set
        {
            if (value)
            {
                m_TimeFreeze += 1;
            }
            else
            {
                m_TimeFreeze -= 1;
            }
        }
    }


    /// <summary>
    /// buff�ĵ�ǰ�ȼ�����������
    /// </summary>
    public int CurrentLevel
    {
        get
        {
            return m_CurrentLevel;
        }
        set
        {
            value = Math.Clamp(value, 0, BuffData.maxLevel);
            int change = value - m_CurrentLevel;
            OnCurrentLevelChange(change);
            m_CurrentLevel = value;
            //ˢ�³���ʱ��
            if (BuffData.autoRefresh && change >= 0)
            {
                ResidualDuration = BuffData.maxDuration;
            }
        }
    }

    /// <summary>
    /// Buff��ʣ��ʱ�䡣
    /// ���ܳ���������ʱ�䣬����С��0��
    /// </summary>
    public float ResidualDuration
    {
        get { return m_ResidualDuration; }
        set { m_ResidualDuration = Math.Clamp(value, 0, BuffData.maxDuration); }
    }

    public CharacterManager characterManager;
    public void Init(BuffData buffData, CharacterManager owner)
    {
        if (m_Initialized)
        {
            return;
        }
        BuffData = buffData;
        characterManager = owner;
        m_Initialized = true;
    }

    #region �鷽��
    /// <summary>
    /// ���buff�Ľ���
    /// </summary>
    /// <returns></returns>
    public virtual string GetDescription1()
    {
        return string.Empty;
    }

    public virtual string GetDescription2()
    {
        return string.Empty;
    }

    /// <summary>
    /// ����buff����Ӻ󴥷�
    /// </summary>
    public virtual void AfterBeAdded() { }
    /// <summary>
    /// ����buff���Ƴ�ʱ����
    /// </summary>
    public virtual void AfterBeRemoved() { }
    /// <summary>
    /// Update
    /// </summary>
    public virtual void Update() { }
    /// <summary>
    /// FixedUpdate
    /// </summary>
    public virtual void FixedUpdate() { }
    /// <summary>
    /// �������ı�ʱ����
    /// </summary>
    protected virtual void OnCurrentLevelChange(int change) { }
    #endregion
}
