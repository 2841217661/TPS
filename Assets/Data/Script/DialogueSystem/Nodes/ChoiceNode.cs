using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Dialogue/Choice Node")]
public class ChoiceNode : Node
{
    [Input] public Empty input;

    // 动态端口列表
    [Output(dynamicPortList = true)]
    public List<string> outputs = new List<string>();

    // 与outputs一一对应的方法名
    public List<string> methodName = new List<string>();

    // 在属性变化时自动同步
    private void OnValidate()
    {
        // 保证methodName数量与outputs一致
        if (outputs == null)
            outputs = new List<string>();
        if (methodName == null)
            methodName = new List<string>();

        while (methodName.Count < outputs.Count)
        {
            methodName.Add(string.Empty);
        }

        while (methodName.Count > outputs.Count)
        {
            methodName.RemoveAt(methodName.Count - 1);
        }
    }


}
