using System;
using UnityEngine;

public class BuffBase
{
    //无参构造
    public BuffBase() { }

    public BuffData BuffData { get; private set; } = null;

    // 持续时间缩放
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
    /// buff的当前等级（层数）。
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
            //刷新持续时间
            if (BuffData.autoRefresh && change >= 0)
            {
                ResidualDuration = BuffData.maxDuration;
            }
        }
    }

    /// <summary>
    /// Buff的剩余时间。
    /// 不能超过最大持续时间，不能小于0。
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

    #region 虚方法
    /// <summary>
    /// 获得buff的介绍
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
    /// 当此buff被添加后触发
    /// </summary>
    public virtual void AfterBeAdded() { }
    /// <summary>
    /// 当此buff被移除时触发
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
    /// 当层数改变时触发
    /// </summary>
    protected virtual void OnCurrentLevelChange(int change) { }
    #endregion
}
