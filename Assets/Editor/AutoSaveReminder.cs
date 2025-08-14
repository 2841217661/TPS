using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class AutoSaveReminder
{
    private static double lastSaveTime;
    private static double remindInterval = 600;

    static AutoSaveReminder()
    {
        lastSaveTime = EditorApplication.timeSinceStartup;
        EditorApplication.update += Update;
    }

    static void Update()
    {
        if (EditorApplication.isPlaying) return; // 运行时不提示

        // 检测时间
        if (EditorApplication.timeSinceStartup - lastSaveTime > remindInterval)
        {
            if (EditorUtility.DisplayDialog("保存提醒", "你已经很久没保存了，是否现在保存？", "保存", "稍后"))
            {
                SaveAll();
            }
            lastSaveTime = EditorApplication.timeSinceStartup;
        }
    }

    static void SaveAll()
    {
        AssetDatabase.SaveAssets();
        Debug.Log("✅ 项目已保存");
    }
}
