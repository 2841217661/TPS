using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public static class BuffPool
{
    private static readonly Dictionary<string, ObjectPool<object>> buffPoolDir;

    //从指定的池中取出Object
    public static Buff Get<Buff>(string poolName) where Buff : BuffBase, new()
    {
        //如果字典中没有这个key，说明目前为止没有添加过这个buff，添加进字典
        if (!buffPoolDir.ContainsKey(poolName))
        {
            // 创建一个对象池，使用 new T() 来生成新对象
            ObjectPool<object> newPool = new ObjectPool<object>(() =>
            {
                // 用无参构造函数创建一个新对象;
                // 以后如果从这个池中取出对象时，如果这个池空了，那么就会自动调用无参构造函数自动从new一个新对象并返回
                return new Buff();
            });

            // 把这个池子放入字典中，对应池名
            buffPoolDir.Add(poolName, newPool);
        }

        // 从池子中取出一个对象，并强制转换为 T 类型后返回
        object pooledObject = buffPoolDir[poolName].Get();
        return (Buff)pooledObject;
    }

    //将Object放入指定的池中
    public static void Release(object item, string poolName)
    {
        //不出意外的话不可能在回收池对象时出现池字典没有对应的池
        if (!buffPoolDir.ContainsKey(poolName))
        {
            Debug.LogError("这里好像出现了问题？？？请检查BuffData是否与脚本类名相同!!!");
            return;
        }
        buffPoolDir[poolName].Release(item);
    }

    //静态构造函数，只执行一次且只在第一次访问类的成员之前自动调用
    //等价于：
    //private static readonly Dictionary<string, ObjectPool<object>> pool = new Dictionary<string, ObjectPool<object>>(10);
    static BuffPool()
    {
        buffPoolDir = new Dictionary<string, ObjectPool<object>>(10);
    }
}
