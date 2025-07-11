using UnityEngine;

public class QuestPanel : BasePanel
{
    public override void OpenPanel(string name)
    {
        base.OpenPanel(name);

        GameManager.Instance.playerManager.inputManager.ApplyActionMap(false, true);

        //����屻��ʱ����ͬ����MainTaskPanel;
        UIManager.Instance.OpenPanel("MainTaskPanel",this.transform);
    }
}
