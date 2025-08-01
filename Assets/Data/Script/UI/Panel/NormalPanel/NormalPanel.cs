using UnityEngine;
using UnityEngine.UI;

public class NormalPanel : MonoSingleton<NormalPanel>
{
    [Header("Top")]
    public Transform QuestFinishNoticePoint;

    [Header("Right")]
    public Transform RandomEventNoticePoint;
    [Tooltip("随机补给面板提升")]
    public GameObject RandomEventNoticeItemPre;

    [Header("RightUp")]
    [Tooltip("玩家信息")]
    public PlayerInfo PlayerInfo;

    [Header("Left")]
    public Button Button_Quest;
    public Transform StartQuestShowItemPoint;

    [Header("LeftUp")]
    public Transform Minmap;

    [Header("Center")]
    public Image AimImage;

    [Header("小地图")]
    public RectTransform MinmapRawImage;

    [Header("伤害数字生成点")]
    public Transform DamageTextPoint;

    [Header("受伤屏幕闪烁")]
    public PlayerDamageScreenEffect playerDamageScreenEffect;

    [Header("动态UICanvas")]
    public Transform DynamicUI_Canvas;
}
