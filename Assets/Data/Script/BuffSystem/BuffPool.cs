using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public static class BuffPool
{
    private static readonly Dictionary<string, ObjectPool<object>> buffPoolDir;

    //��ָ���ĳ���ȡ��Object
    public static Buff Get<Buff>(string poolName) where Buff : BuffBase, new()
    {
        //����ֵ���û�����key��˵��ĿǰΪֹû����ӹ����buff����ӽ��ֵ�
        if (!buffPoolDir.ContainsKey(poolName))
        {
            // ����һ������أ�ʹ�� new T() �������¶���
            ObjectPool<object> newPool = new ObjectPool<object>(() =>
            {
                // ���޲ι��캯������һ���¶���;
                // �Ժ�������������ȡ������ʱ���������ؿ��ˣ���ô�ͻ��Զ������޲ι��캯���Զ���newһ���¶��󲢷���
                return new Buff();
            });

            // ��������ӷ����ֵ��У���Ӧ����
            buffPoolDir.Add(poolName, newPool);
        }

        // �ӳ�����ȡ��һ�����󣬲�ǿ��ת��Ϊ T ���ͺ󷵻�
        object pooledObject = buffPoolDir[poolName].Get();
        return (Buff)pooledObject;
    }

    //��Object����ָ���ĳ���
    public static void Release(object item, string poolName)
    {
        //��������Ļ��������ڻ��ճض���ʱ���ֳ��ֵ�û�ж�Ӧ�ĳ�
        if (!buffPoolDir.ContainsKey(poolName))
        {
            Debug.LogError("���������������⣿��������BuffData�Ƿ���ű�������ͬ!!!");
            return;
        }
        buffPoolDir[poolName].Release(item);
    }

    //��̬���캯����ִֻ��һ����ֻ�ڵ�һ�η�����ĳ�Ա֮ǰ�Զ�����
    //�ȼ��ڣ�
    //private static readonly Dictionary<string, ObjectPool<object>> pool = new Dictionary<string, ObjectPool<object>>(10);
    static BuffPool()
    {
        buffPoolDir = new Dictionary<string, ObjectPool<object>>(10);
    }
}
