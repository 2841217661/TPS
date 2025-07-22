using UnityEngine;

public class NPC_AdministratorManager : NPCManager
{
    public DialogueGraph graph_1; //没有与解说员了解规则
    public DialogueGraph graph_2; //已经了解规则

    [HideInInspector] public bool isReady;
    public override void InteractableFinish()
    {
        base.InteractableFinish();


    }

    public override void InteractableStart()
    {
        base.InteractableStart();

        //判断是否已经和解说员了解了规则（与解说员对话完成后开启挑战）
        if(QuestManager.Instance.GetQuestById(QuestName.Quest_开启挑战).state == QuestState.IN_PROGRESS)
        {
            questTip.SetActive(false);
            currentGraph = graph_2;
        }
        else
        {
            currentGraph = graph_1;
        }
    }

    public void StartChallenge()
    {
        Debug.Log("开启挑战!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        isReady = true;
    }

    public void CancleChallenge()
    {
        Debug.Log("没有准备好。。。。。。。。。。。。。。。。。。。");
        isReady = false;
    }
}
