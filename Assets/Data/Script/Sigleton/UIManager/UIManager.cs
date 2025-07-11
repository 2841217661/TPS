using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{

    protected override void Init()
    {
        base.Init();

        DontDestroyOnLoad(gameObject);

        InitDicts();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseCurrentPanel();
        }
    }
    private Transform _uiRoot;
    // 路径配置字典
    private Dictionary<string, string> pathDict;
    // 预制件缓存字典
    private Dictionary<string, GameObject> prefabDict;
    // 已打开界面的缓存字典
    public Dictionary<string, BasePanel> panelDict;


    /// <summary>
    /// UI根节点，当访问该节点时如果没有该节点，创建一个画布Canvas
    /// </summary>
    public Transform UIRoot
    {
        get
        {
            if (_uiRoot == null)
            {
                if (GameObject.Find("Canvas"))
                {
                    _uiRoot = GameObject.Find("Canvas").transform;
                }
                else
                {
                    // 创建新的 Canvas
                    GameObject newCanvas = new GameObject("Canvas");
                    Canvas canvasComponent = newCanvas.AddComponent<Canvas>();
                    canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay; // 设置渲染模式为屏幕空间

                    // 添加必要的组件
                    newCanvas.AddComponent<CanvasScaler>();
                    newCanvas.AddComponent<GraphicRaycaster>();

                    _uiRoot = newCanvas.transform;

                }
            }
            ;
            return _uiRoot;
        }
    }

    /// <summary>
    /// 初始化各个字典，包括：面板路径字典，已经缓存了的面板预制体字典，当前已经打开的面板字典
    /// </summary>
    private void InitDicts()
    {
        prefabDict = new Dictionary<string, GameObject>();
        panelDict = new Dictionary<string, BasePanel>();

        pathDict = new Dictionary<string, string>()
        {
            {"DialoguePanel", UIPanelPath.DialoguePanel },
            {"QuestPanel",UIPanelPath.QuestPanel},
            {"MainQuestPanel",UIPanelPath.MainQuestPanel },
            {"BranchQuestPanel",UIPanelPath.BranchQuestPanel },
            //{PanelPathConfi.WeaponPanel, "Panel/Package/Weapon/WeaponPanel" },
            //{PanelPathConfi.WeaponDetailPanel, "Panel/Package/Weapon/WeaponDetailPanel" },
            //{PanelPathConfi.SelectSlotItemPanel, "Panel/Package/Weapon/SelectSlotItemPanel"  },
            //{PanelPathConfi.RelicPanel, "Panel/Package/Relic/RelicPanel"  },
            //{PanelPathConfi.PlotPanel, "Panel/Plot/PlotPanel"  },
            //{PanelPathConfi.StorePanel, "Panel/Store/StorePanel"  },
        };
    }

    /// <summary>
    /// 获取面板
    /// </summary>
    /// <param name="name">面板的名字</param>
    /// <returns></returns>
    public BasePanel GetPanel(string name)
    {
        BasePanel panel = null;
        // 检查是否已打开
        if (panelDict.TryGetValue(name, out panel))
        {
            return panel;
        }
        return null;
    }

    //打开面板,如果未传递父节点，则父节点为Canvas,可以通过esc关闭面板
    //如果需要使用Canvas作为父节点，但是又不需要快捷关闭，则可以传递UIRoot
    public BasePanel OpenPanel(string name, Transform parent = null)
    {
        BasePanel panel = null;
        // 检查是否已打开
        if (panelDict.TryGetValue(name, out panel))
        {
            Debug.Log("界面已打开: " + name);
            return null;
        }

        // 检查路径是否配置
        string path = "";
        if (!pathDict.TryGetValue(name, out path))
        {
            Debug.Log("界面名称错误，或未配置路径: " + name);
            return null;
        }

        // 使用缓存预制件
        GameObject panelPrefab = null;
        if (!prefabDict.TryGetValue(name, out panelPrefab))
        {
            string realPath = path;

            panelPrefab = Resources.Load<GameObject>(realPath) as GameObject;
            prefabDict.Add(name, panelPrefab);
        }
        //Debug.Log(path);
        // 打开界面
        if (parent == null)
        {
            GameObject panelObject = GameObject.Instantiate(panelPrefab, UIRoot, false);
            panel = panelObject.GetComponent<BasePanel>();
            panelDict.Add(name, panel); //添加入已打开面板字典缓存
            panel.OpenPanel(name); //打开面板
            return panel;
        }
        else
        {
            GameObject panelObject = GameObject.Instantiate(panelPrefab, parent, false);
            panel = panelObject.GetComponent<BasePanel>();
            panel.OpenPanel(name); //打开面板
            return panel;
        }

    }


    //关闭面板
    public bool ClosePanel(string name)
    {
        BasePanel panel = null;
        if (!panelDict.TryGetValue(name, out panel))
        {
            Debug.Log("界面未打开: " + name);
            return false;
        }

        panel.ClosePanel();
        return true;
    }

    //关闭当前最近打开的面板(esc操作)
    public void CloseCurrentPanel()
    {
        if (panelDict.Count != 0)
        {
            var lastItem = panelDict.Last(); // 使用 LINQ 获取最后一个元素
            ClosePanel(lastItem.Key);
        }
        else
        {
            Debug.Log("没有面板被打开");
        }
    }
}
