using UnityEngine;
using XNode;

[CreateNodeMenu("Dialogue/Dialogue Node")]
public class DialogueNode : Node
{
    [Input] public Empty input;
    [Output] public Empty output;

    public Speaker speaker;

    public string methodName;

    [TextArea(1,10)]
    public string content;
}

// 一个空的可序列化占位类
[System.Serializable]
public class Empty { }

public enum Speaker
{
    Player,
    NPC,
}
