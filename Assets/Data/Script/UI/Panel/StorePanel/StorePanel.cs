using UnityEngine;

public class StorePanel : BasePanel
{
    public NPC_TraderManager traderManager;
    public override void OpenPanel(string name)
    {
        base.OpenPanel(name);

        GameManager.Instance.Pause();
    }

    public override void ClosePanel()
    {
        base.ClosePanel();

        GameManager.Instance.Continue();

        traderManager.currentGraph = traderManager.finishGraph;

        DialoguePanel dialoguePanel = UIManager.Instance.OpenPanel("DialoguePanel") as DialoguePanel;
        dialoguePanel.npcManager = traderManager;
    }
}
