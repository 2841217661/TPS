using TMPro;
using UnityEngine;

public class QuestItem : MonoBehaviour
{
    [HideInInspector] public string title;
    [HideInInspector] public string description;
    [HideInInspector] public string reward;

    public TextMeshProUGUI UI_title;
    public TextMeshProUGUI UI_chapter;
}
