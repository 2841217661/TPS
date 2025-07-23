using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public enum MinmapIconType
{
    quest,
    buff,
    buy,
}

public class MinmapIcon : MonoBehaviour
{
    public MinmapIconType minmapIconType;
    private Camera minimapCamera;
    private Image currentIcon;
    public Transform target;
    private RectTransform iconRect;
    private RectTransform minimapRect;
    private float maskRadius;
    private bool isInit;

    [Header("小地图图标类型")]
    [SerializeField] private Image icon_quest;
    [SerializeField] private Image icon_buff;
    [SerializeField] private Image icon_buy;


    private void Start()
    {
        if (!isInit)
        {
            Debug.LogWarning("未进行初始化！");
        }

        minimapCamera = Minmap.Instance.minimapCamera;

        // 设置父级为图标容器
        transform.SetParent(UIManager.Instance.UIRoot.Find("Left/MinmapParent/Mask/MinmapRawImage"));

        RectTransform owner = GetComponent<RectTransform>();
        // 设置锚点为全拉伸（横向/纵向都从0到1）
        owner.anchorMin = Vector2.zero;      // (0, 0)
        owner.anchorMax = Vector2.one;       // (1, 1)

        // 设置偏移量为0（左、右、上、下）
        owner.offsetMin = Vector2.zero;      // 左下
        owner.offsetMax = Vector2.zero;      // 右上

        iconRect = currentIcon.GetComponent<RectTransform>();
        iconRect.anchorMin = iconRect.anchorMax = new Vector2(0.5f, 0.5f); // 以中心锚点定位

        // 获取小地图 RawImage 的 RectTransform
        minimapRect = transform.parent.GetComponent<RectTransform>();

        // 半径 = 内切圆半径（你可以根据你的 mask 圆形大小微调）
        maskRadius = minimapRect.rect.width * 0.5f * 0.9f; // 0.9f 是安全比例，避免贴边

        // 呼吸缩放动画：0.8x ~ 1.2x 循环
        iconRect.localScale = Vector3.one;
        iconRect.DOScale(1.2f, 0.5f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    private void Update()
    {
        Vector3 viewportPos = minimapCamera.WorldToViewportPoint(target.position);

        // 计算相对于 RawImage 中心的偏移
        Vector2 offset = new Vector2(viewportPos.x - 0.5f, viewportPos.y - 0.5f);
        Vector2 iconPos = offset * minimapRect.rect.size;

        if (offset.sqrMagnitude <= 0.25f) // 半径为0.5的单位圆（在圆形内）
        {
            iconRect.localPosition = iconPos;
            iconRect.rotation = Quaternion.identity; // 无旋转
        }
        else
        {
            // 超出小地图边界，限制在圆形边缘并旋转朝向目标
            Vector2 dir = offset.normalized;
            iconRect.localPosition = dir * maskRadius;
        }
    }

    public void Init(Transform _target,MinmapIconType _iconType)
    {
        target = _target;
        switch (_iconType)
        {
            case MinmapIconType.quest:
                currentIcon = icon_quest;
                break;
            case MinmapIconType.buff:
                currentIcon = icon_buff;
                break;
            case MinmapIconType.buy:
                currentIcon = icon_buy;
                break;
        }
        minmapIconType = _iconType;
        currentIcon.gameObject.SetActive(true);
        isInit = true;
    }
}
