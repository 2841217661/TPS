using UnityEngine;
public class GameManager : MonoSingleton<GameManager>
{
    public PlayerManager playerManager;

    protected override void Init()
    {
        base.Init();

        DontDestroyOnLoad(gameObject);
    }




    private void Start()
    {

    }

    private void Update()
    {
        // 测试：打开任务面板
        if (Input.GetKeyDown(KeyCode.O))
        {
            UIManager.Instance.OpenPanel("QuestPanel");
        }

        // 测试：关闭最近打开的面板
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.CloseCurrentPanel();
        }

        // 切换鼠标显示/隐藏
        if (Input.GetMouseButtonDown(2)) // 鼠标中键
        {
            // 切换状态
            if (Cursor.visible)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
