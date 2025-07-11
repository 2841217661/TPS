using UnityEngine;


[CreateAssetMenu(fileName = "BuffData", menuName = "Scriptable/BuffData", order = 0)]
public class BuffData:ScriptableObject
{
    [Header("����ʵ�ֹ���")]

    [Tooltip("buff�����֡�")]
    public string buffName;

    [Tooltip("buff�����ȼ����ѵ���������")]
    public int maxLevel = 1;

    [Tooltip("buff�ǲ��������Եģ����������Գ���ʱ�䡣")]
    public bool isPermanent = false;

    [Tooltip("Buff��������ʱ��,ͬʱҲ�ǳ�ʼ����ʱ�䡣")]
    public float maxDuration = 0.02f;

    [Tooltip("��buff����ʱ�䵽0ʱҪ���͵ĵȼ����������Ϊ0�������0����")]
    public int demotion = 1;

    [Tooltip("��buff�ĵȼ�����ʱ��ﵽ���ȼ����������ȼ�ʱ���Ƿ��Զ�ˢ�³���ʱ�䡣")]
    public bool autoRefresh = true;

    [Tooltip("��������ͬ��Դ����ͬһ����λʩ��ͬһ��buffʱ�ĳ�ͻ����\r\n��Ϊ���֣�\r\ncombine���ϲ�Ϊһ��buff�����㡣\r\nseparate���������ڡ�\r\ncover�����ǣ����߸���ǰ�ߡ�")]
    public ConflictResolution conflictResolution = ConflictResolution.combine;

    [Header("���ڷ��ദ��")]
    [Tooltip("���������ڶ�buff���з��ദ��")]
    public BuffType buffType = BuffType.Neutral;

    [Tooltip("���������ڶ�buff���з��ദ��")]
    public int intensity = 0;

    [Tooltip("���������ڶ�buff���з��ദ��")]
    public string[] tags = null;

    [Header("����UI��ʾ")]
    public string buffDescript1 = "����1";
    public string buffDescript2 = "����2"; 

    public Sprite icon;

    private void OnValidate()
    {
        if (maxLevel <= 0)
        {
            Debug.LogError("buff�����ȼ�����С�ڻ����0");
            maxLevel = 1;
        }
        if (maxDuration <= 0)
        {
            Debug.LogError("buff��������ʱ�䲻��С�ڻ����0");
            maxDuration = 0.02f;
        }
        if (demotion < 0)
        {
            Debug.LogError("buff�ĵ�ʱ����������С��0");
            demotion = 0;
        }
    }
}
