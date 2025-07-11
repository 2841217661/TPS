using UnityEngine;

public class 进入村庄 : QuestStep
{
    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            FinishQuestStep();
        }
    }
}
