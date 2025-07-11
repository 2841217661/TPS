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
    // ·�������ֵ�
    private Dictionary<string, string> pathDict;
    // Ԥ�Ƽ������ֵ�
    private Dictionary<string, GameObject> prefabDict;
    // �Ѵ򿪽���Ļ����ֵ�
    public Dictionary<string, BasePanel> panelDict;


    /// <summary>
    /// UI���ڵ㣬�����ʸýڵ�ʱ���û�иýڵ㣬����һ������Canvas
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
                    // �����µ� Canvas
                    GameObject newCanvas = new GameObject("Canvas");
                    Canvas canvasComponent = newCanvas.AddComponent<Canvas>();
                    canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay; // ������ȾģʽΪ��Ļ�ռ�

                    // ��ӱ�Ҫ�����
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
    /// ��ʼ�������ֵ䣬���������·���ֵ䣬�Ѿ������˵����Ԥ�����ֵ䣬��ǰ�Ѿ��򿪵�����ֵ�
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
    /// ��ȡ���
    /// </summary>
    /// <param name="name">��������</param>
    /// <returns></returns>
    public BasePanel GetPanel(string name)
    {
        BasePanel panel = null;
        // ����Ƿ��Ѵ�
        if (panelDict.TryGetValue(name, out panel))
        {
            return panel;
        }
        return null;
    }

    //�����,���δ���ݸ��ڵ㣬�򸸽ڵ�ΪCanvas,����ͨ��esc�ر����
    //�����Ҫʹ��Canvas��Ϊ���ڵ㣬�����ֲ���Ҫ��ݹرգ�����Դ���UIRoot
    public BasePanel OpenPanel(string name, Transform parent = null)
    {
        BasePanel panel = null;
        // ����Ƿ��Ѵ�
        if (panelDict.TryGetValue(name, out panel))
        {
            Debug.Log("�����Ѵ�: " + name);
            return null;
        }

        // ���·���Ƿ�����
        string path = "";
        if (!pathDict.TryGetValue(name, out path))
        {
            Debug.Log("�������ƴ��󣬻�δ����·��: " + name);
            return null;
        }

        // ʹ�û���Ԥ�Ƽ�
        GameObject panelPrefab = null;
        if (!prefabDict.TryGetValue(name, out panelPrefab))
        {
            string realPath = path;

            panelPrefab = Resources.Load<GameObject>(realPath) as GameObject;
            prefabDict.Add(name, panelPrefab);
        }
        //Debug.Log(path);
        // �򿪽���
        if (parent == null)
        {
            GameObject panelObject = GameObject.Instantiate(panelPrefab, UIRoot, false);
            panel = panelObject.GetComponent<BasePanel>();
            panelDict.Add(name, panel); //������Ѵ�����ֵ仺��
            panel.OpenPanel(name); //�����
            return panel;
        }
        else
        {
            GameObject panelObject = GameObject.Instantiate(panelPrefab, parent, false);
            panel = panelObject.GetComponent<BasePanel>();
            panel.OpenPanel(name); //�����
            return panel;
        }

    }


    //�ر����
    public bool ClosePanel(string name)
    {
        BasePanel panel = null;
        if (!panelDict.TryGetValue(name, out panel))
        {
            Debug.Log("����δ��: " + name);
            return false;
        }

        panel.ClosePanel();
        return true;
    }

    //�رյ�ǰ����򿪵����(esc����)
    public void CloseCurrentPanel()
    {
        if (panelDict.Count != 0)
        {
            var lastItem = panelDict.Last(); // ʹ�� LINQ ��ȡ���һ��Ԫ��
            ClosePanel(lastItem.Key);
        }
        else
        {
            Debug.Log("û����屻��");
        }
    }
}
