using UnityEngine;
using TMPro;
using DG.Tweening;

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
        //���ԣ����������
        //UIManager.Instance.OpenPanel("QuestPanel");
    }

    private void Update()
    {
        //���ԣ������������еĶԻ�����
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }
    }
}
