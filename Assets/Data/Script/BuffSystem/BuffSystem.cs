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

    #region �¼�
    public event Action<BuffBase> E_OnAddBuff; //buff���ʱ���¼�
    public event Action<BuffBase> E_OnRemoveBuff; //buff�Ƴ�ʱ���¼�
    #endregion

    private readonly List<BuffBase> m_Buffs = new List<BuffBase>(10);
    public List<BuffBase> buffs
    {
        get { return m_Buffs; }
    }

    #region public����
    /// <summary>
    /// ���һ��buff
    /// </summary>
    /// <typeparam name="BuffType">buff����</typeparam>
    /// <param name="heap">��Ӷ��ٲ�?</param>
    public void AddBuff<BuffType>(int heap = 1) where BuffType : BuffBase,new()
    {
        //�������û���κ�buff��ֱ�ӹ�һ����buff����
        if (m_Buffs.Count == 0)
        {
            AddNewBuff<BuffType>(heap);
            return;
        }
        //��������������û���Ѵ��ڵ�ͬ��buff��Ȼ��������
        List<BuffType> buffs = new List<BuffType>(5);
        foreach (BuffBase buff in m_Buffs)
        {
            if (buff is BuffType)
            {
                buffs.Add(buff as BuffType);
            }
        }
        //���������ͬ��buff��ֱ�ӹ�һ����buff����
        if (buffs.Count == 0)
        {
            AddNewBuff<BuffType>(heap);
        }
        else//������г�ͻ����
        {
            BuffType sameProviderBuff = null;
            switch (buffs[0].BuffData.conflictResolution)
            {
                /*����(�ϲ�)��
                 * ����
                 * ���buffΪ����buff���������buff�������ȼ������ǳ���ʱ��ʼ����������ʱ�䣬����˥��
                 * ���buff�Զ�ˢ�£��ڵõ��µ�ͬ��buffʱ��ˢ��ǰһ��buff�ĳ���ʱ��
                 * ���A�ṩ��buff1����ң���ʱBҲ�ṩ��buff1����ң���ôB�ṩ��buff1��ʹA�ṩ��buff1�ȼ�����
                 * ����buff1���ṩ��ʼ��ΪA
                 */
                case ConflictResolution.combine:
                    buffs[0].CurrentLevel += heap;
                    break;
                /*�������ڣ�
                 * �������A�ṩ��buff1����ң�
                 * ����������˭�ٴ��ṩbuff1����ң���������µ�buff1
                 */
                case ConflictResolution.separate:
                    AddNewBuff<BuffType>(heap);
                    break;
                /*
                 * ���ǣ�
                 * �������A�ṩ��buff1����ң�������
                 * ����˭�ٴ��ṩbuff1����ң�����ɾ��֮ǰ��buff����������µ�buff1
                 */
                case ConflictResolution.cover://����
                    RemoveBuff(sameProviderBuff);
                    AddNewBuff<BuffType>(heap);
                    break;
            }
        }
    }
    /// <summary>
    /// ��ָ�����͵�Buff�������Buffӵ�ж��ʱ��ֻ�Ƴ���һ��Buff
    /// �����buff�������򷵻�false
    /// </summary>
    /// <param name="buff">Ҫ�Ƴ���buff</param>
    /// <returns></returns>
    public bool RemoveBuff(BuffBase buff)
    {
        //���ҵ�����
        int index = IndexOf(buff);
        if (index < 0)
        {
            Debug.LogError("��������: " + index);
            return false;
        }
        //Ȼ���Ƴ�����
        RemoveAt(index);
        return true;
    }
    /// <summary>
    /// �Ƴ������͵�����buff
    /// ���û�д�buff�򷵻�false
    /// </summary>
    /// <typeparam name="BuffType">Ҫ�Ƴ���buff������</typeparam>
    /// <returns></returns>
    public bool RemoveBuff<BuffType>() where BuffType : BuffBase
    {
        //�ҵ�����������buff
        List<BuffType> buffs = FindBuff<BuffType>();
        if (buffs.Count == 0)
        {
            return false;
        }

        foreach(BuffType buff in buffs)
        {
            //��ȡ��������
            int index = IndexOf(buff);
            //�Ƴ���
            RemoveAt(index);
        }

        return true;
    }
    /// <summary>
    /// ����ָ�����͵�����Buff
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
    /// ���ص�ǰӵ�е�����buff��
    /// </summary>
    /// <returns></returns>
    public List<BuffBase> FindAllBuff()
    {
        return m_Buffs;
    }
    #endregion

    #region private����

    //��ȡָ��buff��buffs�е�����
    private int IndexOf(BuffBase _buff)
    {
        return m_Buffs.IndexOf(_buff);
    }

    //�Ƴ�buffsָ������λ�õ�buff
    private BuffBase RemoveAt(int _index)
    {
        BuffBase buff = m_Buffs[_index];

        m_Buffs.RemoveAt(_index);

        BuffPool.Release(buff, buff.GetType().Name);

        buff.CurrentLevel = 0;
        buff.AfterBeRemoved(); //��buff���Ƴ��ķ����ص�
        E_OnRemoveBuff?.Invoke(buff); //��buffϵͳ��buff���Ƴ����¼��ص�

        return buff;
    }
    private void AddNewBuff<BuffType>(int heap = 1) where BuffType : BuffBase,new()
    {
        //����buff
        string poolName = typeof(BuffType).FullName;
        BuffType buff = BuffPool.Get<BuffType>(poolName);
        buff.Init(BuffDataManager.GetBuffData<BuffType>(),characterManager);


        //���buff
        m_Buffs.Add(buff);
        buff.AfterBeAdded(); //��buff����ӳɹ������ص�

        E_OnAddBuff?.Invoke(buff); //��buffϵͳ��buff��ӳɹ��¼��ص�

        //��ʼ������
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
            //ִ��fixed update
            buff.FixedUpdate();
            // ������������� Buff ��ʱ��û�б�����
            if (!buff.BuffData.isPermanent && !buff.TimeFreeze)
            {
                // �������ʱ��˥��
                if (buff.DurationScale > 0f)
                {
                    buff.ResidualDuration -= Time.fixedDeltaTime * buff.DurationScale;
                }
                else
                {
                    buff.ResidualDuration = 0f;
                }

                // �������ʱ��ľ�
                if (buff.ResidualDuration <= 0f)
                {
                    if (buff.BuffData.demotion > 0)
                    {
                        buff.CurrentLevel -= buff.BuffData.demotion;
                        // ȷ������为
                        if (buff.CurrentLevel < 0)
                            buff.CurrentLevel = 0;
                    }
                    else
                    {
                        buff.CurrentLevel = 0;
                    }

                    // ���ó���ʱ�䣨������в�����
                    if (buff.CurrentLevel > 0)
                    {
                        buff.ResidualDuration = buff.BuffData.maxDuration;
                    }
                }
            }

            //����ѵ�����Ϊ0�����Ƴ���buff��
            if (buff.CurrentLevel == 0)
            {
                RemoveAt(i);
            }
        }
    }
    #endregion
}
