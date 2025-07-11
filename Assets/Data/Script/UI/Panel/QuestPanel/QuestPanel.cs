using UnityEngine;

public class QuestPanel : BasePanel
{
    public override void OpenPanel(string name)
    {
        base.OpenPanel(name);

        GameManager.Instance.playerManager.inputManager.ApplyActionMap(false, true);

        //该面板被打开时，会同步打开MainTaskPanel;
        UIManager.Instance.OpenPanel("MainTaskPanel",this.transform);
    }
}
