using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "Scriptable/QuestInfoSO", order = 1)]
public class QuestInfoSO : ScriptableObject
{
    [field: SerializeField] public string id { get; private set; }

    [Header("任务名")]
    public string questName;

    /*一个任务可分为多个小任务，例如接受到npc的击杀怪物的任务：1.击杀怪物 2.与委托人npc进行交谈，完成任务*/
    [Header("任务步骤")]
    public GameObject[] questStepPrefabs;

    [Header("任务开启条件")]
    public QuestInfoSO[] startRequestPrefabs; //任务开启需要完成的前置任务

    [Header("任务奖励")]
    public string rewardDescription;


    private void OnValidate()
    {
#if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
