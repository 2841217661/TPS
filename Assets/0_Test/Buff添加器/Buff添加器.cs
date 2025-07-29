using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Buff添加器 : MonoSingleton<Buff添加器>
{
    public Button Button_OC;
    public GameObject 修改器面板;
    public TextMeshProUGUI 描述1;
    public BuffSystem actorBuffSystem;

    private void Start()
    {
        Button_OC.onClick.AddListener(() => OpenOrClose修改器面板());
        actorBuffSystem = GameManager.Instance.playerManager.buffSystem; //目前以玩家buff系统为例
    }

    private void OpenOrClose修改器面板()
    {
        if (修改器面板.activeSelf)
        {
            修改器面板.SetActive(false);
        }
        else
        {
            修改器面板.SetActive(true);
        }
    }

    public Type FindTypeByName(string className)
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            Type type = assembly.GetType(className);
            if (type != null)
                return type;
        }
        return null;
    }
}
