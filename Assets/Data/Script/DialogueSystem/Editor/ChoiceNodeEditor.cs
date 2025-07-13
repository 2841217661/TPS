using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

[CustomNodeEditor(typeof(ChoiceNode))]
public class ChoiceNodeEditor : NodeEditor
{
    public override void OnHeaderGUI()
    {
        GUILayout.Label("⭐选项节点", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
    }

    public override void OnBodyGUI()
    {
        serializedObject.Update();

        // 先画输入端口（保持默认背景）
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("input"));

        // 改变背景色为绿色
        Color oldColor = GUI.backgroundColor;
        GUI.backgroundColor = new Color(0.8f, 1f, 0.8f);

        // 动态端口列表
        NodeEditorGUILayout.DynamicPortList(
            "outputs",
            typeof(string),
            serializedObject,
            NodePort.IO.Output,
            Node.ConnectionType.Override
        );

        // methodName 列表
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("methodName"));

        // 恢复背景色
        GUI.backgroundColor = oldColor;

        serializedObject.ApplyModifiedProperties();
    }
}
