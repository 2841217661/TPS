using UnityEngine;


[CreateAssetMenu(fileName = "BuffData", menuName = "Scriptable/BuffData", order = 0)]
public class BuffData:ScriptableObject
{
    [Header("用于实现功能")]

    [Tooltip("buff的名字。")]
    public string buffName;

    [Tooltip("buff的最大等级（堆叠层数）。")]
    public int maxLevel = 1;

    [Tooltip("buff是不是永久性的，如果是则忽略持续时间。")]
    public bool isPermanent = false;

    [Tooltip("Buff的最大持续时间,同时也是初始持续时间。")]
    public float maxDuration = 0.02f;

    [Tooltip("当buff持续时间到0时要降低的等级，如果设置为0则代表降至0级。")]
    public int demotion = 1;

    [Tooltip("当buff的等级上升时或达到最大等级后又提升等级时，是否自动刷新持续时间。")]
    public bool autoRefresh = true;

    [Tooltip("当两个不同来源者向同一个单位施加同一个buff时的冲突处理\r\n分为三种：\r\ncombine：合并为一个buff，叠层。\r\nseparate：独立存在。\r\ncover：覆盖，后者覆盖前者。")]
    public ConflictResolution conflictResolution = ConflictResolution.combine;

    [Header("用于分类处理")]
    [Tooltip("此属性用于对buff进行分类处理")]
    public BuffType buffType = BuffType.Neutral;

    [Tooltip("此属性用于对buff进行分类处理")]
    public int intensity = 0;

    [Tooltip("此属性用于对buff进行分类处理")]
    public string[] tags = null;

    [Header("用于UI显示")]
    public string buffDescript1 = "描述1";
    public string buffDescript2 = "描述2"; 

    public Sprite icon;

    private void OnValidate()
    {
        if (maxLevel <= 0)
        {
            Debug.LogError("buff的最大等级不能小于或等于0");
            maxLevel = 1;
        }
        if (maxDuration <= 0)
        {
            Debug.LogError("buff的最大持续时间不能小于或等于0");
            maxDuration = 0.02f;
        }
        if (demotion < 0)
        {
            Debug.LogError("buff的到时降级量不能小于0");
            demotion = 0;
        }
    }
}
