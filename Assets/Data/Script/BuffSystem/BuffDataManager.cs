using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public enum ConflictResolution
{
    [UnityEngine.InspectorName("合并")]
    /// <summary>
    /// 合并为一个buff，叠层。
    /// </summary>
    combine,
    [UnityEngine.InspectorName("独立")]
    /// <summary>
    /// 独立存在
    /// </summary>
    separate,
    [UnityEngine.InspectorName("覆盖")]
    /// <summary>
    /// 覆盖，后者覆盖前者
    /// </summary>
    cover,
}
public enum BuffType
{

    /// <summary>
    /// 有害的
    /// </summary>
    [UnityEngine.InspectorName("有害的")]
    Negative,

    /// <summary>
    /// 有益的
    /// </summary>
    [UnityEngine.InspectorName("有益的")]
    Positive,

    /// <summary>
    /// 中性的
    /// </summary>
    [UnityEngine.InspectorName("中性的")]
    Neutral,
}

public static class BuffDataManager
{
    private static Dictionary<string, BuffData> buffDataDictionary;

    // 获得BuffData:T为buff的名字，需要与buff类名相同
    public static BuffData GetBuffData<T>() where T : class
    {
        if (buffDataDictionary == null)
        {
            buffDataDictionary = new Dictionary<string, BuffData>(5);
        }

        string name = typeof(T).Name;
        BuffData result;

        buffDataDictionary.TryGetValue(name, out result);

        //添加人字典缓存
        if (result == null)
        {
            result = LoadBuffData(name);
            buffDataDictionary.Add(name, result);
        }

        return result;
    }

    // 释放掉缓存的所有BuffData,并释放字典内存
    public static void Release()
    {
        buffDataDictionary = null;

    }

    // 释放掉缓存的所有BuffData,不释放字典内存
    public static void Clear()
    {
        buffDataDictionary.Clear();
    }


    private const string folder = "BuffData";
    //加载Resources/BuffData目录下的BuffData
    private static BuffData LoadBuffData(string name)
    {
        BuffData result;
        string path = Path.Combine(folder, name);
        result = Resources.Load<BuffData>(path);
        if (result == null)
        {
            throw new Exception($"加载BuffData失败，加载路径：{path}");
        }
        return result;
    }
}
