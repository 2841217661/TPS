using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public enum ConflictResolution
{
    [UnityEngine.InspectorName("�ϲ�")]
    /// <summary>
    /// �ϲ�Ϊһ��buff�����㡣
    /// </summary>
    combine,
    [UnityEngine.InspectorName("����")]
    /// <summary>
    /// ��������
    /// </summary>
    separate,
    [UnityEngine.InspectorName("����")]
    /// <summary>
    /// ���ǣ����߸���ǰ��
    /// </summary>
    cover,
}
public enum BuffType
{

    /// <summary>
    /// �к���
    /// </summary>
    [UnityEngine.InspectorName("�к���")]
    Negative,

    /// <summary>
    /// �����
    /// </summary>
    [UnityEngine.InspectorName("�����")]
    Positive,

    /// <summary>
    /// ���Ե�
    /// </summary>
    [UnityEngine.InspectorName("���Ե�")]
    Neutral,
}

public static class BuffDataManager
{
    private static Dictionary<string, BuffData> buffDataDictionary;

    // ���BuffData:TΪbuff�����֣���Ҫ��buff������ͬ
    public static BuffData GetBuffData<T>() where T : class
    {
        if (buffDataDictionary == null)
        {
            buffDataDictionary = new Dictionary<string, BuffData>(5);
        }

        string name = typeof(T).Name;
        BuffData result;

        buffDataDictionary.TryGetValue(name, out result);

        //������ֵ仺��
        if (result == null)
        {
            result = LoadBuffData(name);
            buffDataDictionary.Add(name, result);
        }

        return result;
    }

    // �ͷŵ����������BuffData,���ͷ��ֵ��ڴ�
    public static void Release()
    {
        buffDataDictionary = null;

    }

    // �ͷŵ����������BuffData,���ͷ��ֵ��ڴ�
    public static void Clear()
    {
        buffDataDictionary.Clear();
    }


    private const string folder = "BuffData";
    //����Resources/BuffDataĿ¼�µ�BuffData
    private static BuffData LoadBuffData(string name)
    {
        BuffData result;
        string path = Path.Combine(folder, name);
        result = Resources.Load<BuffData>(path);
        if (result == null)
        {
            throw new Exception($"����BuffDataʧ�ܣ�����·����{path}");
        }
        return result;
    }
}
