using UnityEngine;

public class NPC_AdventureManager : NPCManager
{
    [Header("¶Ô»°Í¼")]
    public DialogueGraph _02Graph;
    public void Change_02Graph()
    {
        currentGraph = _02Graph;
    }
}
