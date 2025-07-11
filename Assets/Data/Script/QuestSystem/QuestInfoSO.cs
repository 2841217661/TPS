using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "Scriptable/QuestInfoSO", order = 1)]
public class QuestInfoSO : ScriptableObject
{
    [field: SerializeField] public string id { get; private set; }

    [Header("��������")]
    public QuestType type;

    [Header("�������ڵ��½�")]
    public string chapter;

    [Header("��������")]
    public string questDescription;

    /*һ������ɷ�Ϊ���С����������ܵ�npc�Ļ�ɱ���������1.��ɱ���� 2.��ί����npc���н�̸���������*/
    [Header("������")]
    public GameObject[] questStepPrefabs;

    [Header("����������")]
    public QuestInfoSO[] startRequestPrefabs; //��������Ҫ��ɵ�ǰ������

    [Header("������")]
    public string rewardDescription;


    private void OnValidate()
    {
#if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
