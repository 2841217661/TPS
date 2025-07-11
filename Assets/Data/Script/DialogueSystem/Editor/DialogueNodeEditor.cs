using UnityEngine;
using UnityEditor;
using XNodeEditor;

[CustomNodeEditor(typeof(DialogueNode))]
public class DialogueNodeEditor : NodeEditor
{
    public override void OnHeaderGUI()
    {
        // 显示标题
        DialogueNode node = target as DialogueNode;
        string speakerText = node.speaker == Speaker.Player ? "👤 玩家" : "🧙 NPC";
        GUILayout.Label($"[{speakerText}]", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
    }

    public override void OnBodyGUI()
    {
        DialogueNode node = target as DialogueNode;

        // 根据 speaker 决定背景颜色
        Color bgColor;
        switch (node.speaker)
        {
            case Speaker.Player:
                bgColor = new Color(0.5f, 0.8f, 1f); // 蓝色：玩家
                break;
            case Speaker.NPC:
                bgColor = new Color(1f, 0.9f, 0.5f); // 黄色：NPC
                break;
            default:
                bgColor = GUI.backgroundColor;
                break;
        }

        // 设置 GUI 背景色
        Color originalColor = GUI.backgroundColor;
        GUI.backgroundColor = bgColor;

        // 绘制默认的节点内容
        base.OnBodyGUI();

        // 恢复原始颜色
        GUI.backgroundColor = originalColor;
    }
}
