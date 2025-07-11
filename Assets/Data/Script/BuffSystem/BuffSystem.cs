using System;
using System.Collections.Generic;
using UnityEngine;

public class BuffSystem
{
    public CharacterManager characterManager;

    public BuffSystem(CharacterManager _characterManager)
    {
        characterManager = _characterManager;
    }

    #region 事件
    public event Action<BuffBase> E_OnAddBuff; //buff添加时的事件
    public event Action<BuffBase> E_OnRemoveBuff; //buff移除时的事件
    #endregion

    private readonly List<BuffBase> m_Buffs = new List<BuffBase>(10);
    public List<BuffBase> buffs
    {
        get { return m_Buffs; }
    }

    #region public方法
    /// <summary>
    /// 添加一个buff
    /// </summary>
    /// <typeparam name="BuffType">buff类型</typeparam>
    /// <param name="heap">添加多少层?</param>
    public void AddBuff<BuffType>(int heap = 1) where BuffType : BuffBase,new()
    {
        //如果身上没有任何buff则直接挂一个新buff即可
        if (m_Buffs.Count == 0)
        {
            AddNewBuff<BuffType>(heap);
            return;
        }
        //遍历看看身上有没有已存在的同类buff，然后做处理
        List<BuffType> buffs = new List<BuffType>(5);
        foreach (BuffBase buff in m_Buffs)
        {
            if (buff is BuffType)
            {
                buffs.Add(buff as BuffType);
            }
        }
        //如果不存在同类buff，直接挂一个新buff即可
        if (buffs.Count == 0)
        {
            AddNewBuff<BuffType>(heap);
        }
        else//否则进行冲突处理
        {
            BuffType sameProviderBuff = null;
            switch (buffs[0].BuffData.conflictResolution)
            {
                /*叠层(合并)：
                 * 规则：
                 * 如果buff为永久buff：持续添加buff会提升等级，但是持续时间始终是最大持续时间，不会衰减
                 * 如果buff自动刷新：在得到新的同类buff时会刷新前一次buff的持续时间
                 * 如果A提供了buff1给玩家，此时B也提供了buff1给玩家，那么B提供的buff1会使A提供的buff1等级提升
                 * 并且buff1的提供者始终为A
                 */
                case ConflictResolution.combine:
                    buffs[0].CurrentLevel += heap;
                    break;
                /*独立存在：
                 * 规则：如果A提供了buff1给玩家，
                 * 接下来无论谁再次提供buff1给玩家，都会添加新的buff1
                 */
                case ConflictResolution.separate:
                    AddNewBuff<BuffType>(heap);
                    break;
                /*
                 * 覆盖：
                 * 规则：如果A提供了buff1给玩家，接下来
                 * 无论谁再次提供buff1给玩家，都会删除之前的buff并重新添加新的buff1
                 */
                case ConflictResolution.cover://覆盖
                    RemoveBuff(sameProviderBuff);
                    AddNewBuff<BuffType>(heap);
                    break;
            }
        }
    }
    /// <summary>
    /// 移指定类型的Buff，如果该Buff拥有多个时，只移除第一个Buff
    /// 如果此buff不存在则返回false
    /// </summary>
    /// <param name="buff">要移除的buff</param>
    /// <returns></returns>
    public bool RemoveBuff(BuffBase buff)
    {
        //先找到索引
        int index = IndexOf(buff);
        if (index < 0)
        {
            Debug.LogError("索引错误: " + index);
            return false;
        }
        //然后移除他！
        RemoveAt(index);
        return true;
    }
    /// <summary>
    /// 移除该类型的所有buff
    /// 如果没有此buff则返回false
    /// </summary>
    /// <typeparam name="BuffType">要移除的buff的类型</typeparam>
    /// <returns></returns>
    public bool RemoveBuff<BuffType>() where BuffType : BuffBase
    {
        //找到符号条件的buff
        List<BuffType> buffs = FindBuff<BuffType>();
        if (buffs.Count == 0)
        {
            return false;
        }

        foreach(BuffType buff in buffs)
        {
            //获取它的索引
            int index = IndexOf(buff);
            //移除他
            RemoveAt(index);
        }

        return true;
    }
    /// <summary>
    /// 查找指定类型的所有Buff
    /// </summary>
    /// <typeparam name="BuffType"></typeparam>
    /// <returns></returns>
    public List<BuffType> FindBuff<BuffType>() where BuffType : BuffBase
    {
        List<BuffType> buffs = new List<BuffType>(5);
        foreach (BuffBase buff in m_Buffs)
        {
            if (buff is BuffType)
            {
                buffs.Add(buff as BuffType);
            }
        }

        return buffs;
    }
    /// <summary>
    /// 返回当前拥有的所有buff。
    /// </summary>
    /// <returns></returns>
    public List<BuffBase> FindAllBuff()
    {
        return m_Buffs;
    }
    #endregion

    #region private方法

    //获取指定buff在buffs中的索引
    private int IndexOf(BuffBase _buff)
    {
        return m_Buffs.IndexOf(_buff);
    }

    //移除buffs指定索引位置的buff
    private BuffBase RemoveAt(int _index)
    {
        BuffBase buff = m_Buffs[_index];

        m_Buffs.RemoveAt(_index);

        BuffPool.Release(buff, buff.GetType().Name);

        buff.CurrentLevel = 0;
        buff.AfterBeRemoved(); //该buff被移除的方法回调
        E_OnRemoveBuff?.Invoke(buff); //该buff系统的buff被移除的事件回调

        return buff;
    }
    private void AddNewBuff<BuffType>(int heap = 1) where BuffType : BuffBase,new()
    {
        //创建buff
        string poolName = typeof(BuffType).FullName;
        BuffType buff = BuffPool.Get<BuffType>(poolName);
        buff.Init(BuffDataManager.GetBuffData<BuffType>(),characterManager);


        //添加buff
        m_Buffs.Add(buff);
        buff.AfterBeAdded(); //该buff的添加成功方法回调

        E_OnAddBuff?.Invoke(buff); //该buff系统的buff添加成功事件回调

        //初始化设置
        buff.DurationScale = 1;
        buff.CurrentLevel = heap;
        buff.ResidualDuration = buff.BuffData.maxDuration;
    }
    #endregion

    #region Update
    public void Update()
    {
        foreach (BuffBase buff in m_Buffs)
        {
            buff.Update();
        }
    }
    public void FixedUpdate()
    {
        for (int i = m_Buffs.Count - 1; i >= 0; i--)
        {
            BuffBase buff = m_Buffs[i];
            //执行fixed update
            buff.FixedUpdate();
            // 如果不是永久性 Buff 且时间没有被冻结
            if (!buff.BuffData.isPermanent && !buff.TimeFreeze)
            {
                // 处理持续时间衰减
                if (buff.DurationScale > 0f)
                {
                    buff.ResidualDuration -= Time.fixedDeltaTime * buff.DurationScale;
                }
                else
                {
                    buff.ResidualDuration = 0f;
                }

                // 如果持续时间耗尽
                if (buff.ResidualDuration <= 0f)
                {
                    if (buff.BuffData.demotion > 0)
                    {
                        buff.CurrentLevel -= buff.BuffData.demotion;
                        // 确保不会变负
                        if (buff.CurrentLevel < 0)
                            buff.CurrentLevel = 0;
                    }
                    else
                    {
                        buff.CurrentLevel = 0;
                    }

                    // 重置持续时间（如果还有层数）
                    if (buff.CurrentLevel > 0)
                    {
                        buff.ResidualDuration = buff.BuffData.maxDuration;
                    }
                }
            }

            //如果堆叠层数为0，则移除此buff。
            if (buff.CurrentLevel == 0)
            {
                RemoveAt(i);
            }
        }
    }
    #endregion
}
