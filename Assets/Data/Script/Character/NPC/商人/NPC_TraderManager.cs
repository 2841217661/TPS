using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NPC_TraderManager : NPCManager
{
    public DialogueGraph finishGraph;

    protected override void Awake()
    {
        base.Awake();

        Destroy(gameObject,120f); //在场景中不能待太久

        Minmap.Instance.AddMinmapIcon(this.transform,MinmapIconType.buy);
        questTip.SetActive(true);
    }
    //挑选商品：
    public void SelectProducts()
    {
        Debug.Log("准备挑选商品");

        //测试
        StorePanel storePanel = UIManager.Instance.OpenPanel("StorePanel") as StorePanel;
        storePanel.traderManager = this;
    }


    //没钱：
    public void NoMoney()
    {
        Debug.Log("没有钱");

        Destroy(gameObject);
    }

    //结束对话
    public void Finish()
    {
        Debug.Log("结束了..........");
        Destroy(gameObject);
    }

    public override void InteractableFinish()
    {
        //state = NPCState.Idle; 不能设置为idle，不然在进入商店面板时又会出现"F进行交谈"
        EventManager.Instance.npcEvent.FinishConversation_Anyone(this);
    }

    private void OnDestroy()
    {
       Minmap.Instance.RemoveMinmapIcon(this.transform, MinmapIconType.buy);
    }
}
