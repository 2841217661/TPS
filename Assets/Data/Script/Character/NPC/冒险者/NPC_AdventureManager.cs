using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NPC_AdventureManager : NPCManager
{
    [Header("对话图")]
    public DialogueGraph _02Graph;//第二次对话结束：及玩家选择了buff后再次对话
    public void Change_02Graph()
    {
        currentGraph = _02Graph;
    }

    protected override void Start()
    {
        base.Start();

        //3s后开启第一个任务
        StartCoroutine(StartFirestQuest());
    }

    public override void InteractableFinish()
    {
        base.InteractableFinish();

        if(currentGraph != _02Graph)
            Change_02Graph();
    }

    public override void InteractableStart()
    {
        base.InteractableStart();

        
        if(QuestManager.Instance.GetQuestById(QuestName.Quest_村庄许可证).state == QuestState.IN_PROGRESS)
        {
            questTip.SetActive(false);
        }
    }

    private IEnumerator StartFirestQuest()
    {
        yield return new WaitForSeconds(3f);
        QuestManager.Instance.TryStartQuest(QuestName.Quest_村庄许可证);
        questTip.SetActive(true);

        Minmap.Instance.AddMinmapIcon(this.transform, MinmapIconType.quest);
    }

    #region 对话方法
    public void AddBuff_狂暴火力()
    {
        GameManager.Instance.playerManager.buffSystem.AddBuff<B_狂暴火力>();
    }

    public void AddBuff_危机合约()
    {
        GameManager.Instance.playerManager.buffSystem.AddBuff<B_危机合约>();
    }

    public void AddBuff_淘汰回放()
    {
        GameManager.Instance.playerManager.buffSystem.AddBuff<B_淘汰回放>();
    }

    public void AddBuff_All()
    {
        GameManager.Instance.playerManager.buffSystem.AddBuff<B_狂暴火力>();
        GameManager.Instance.playerManager.buffSystem.AddBuff<B_危机合约>();
        GameManager.Instance.playerManager.buffSystem.AddBuff<B_淘汰回放>();
    }
    #endregion
}
