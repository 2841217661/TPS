using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class DamageText : MonoBehaviour, IPoolable
{
    [SerializeField] private float lifeTime = 1f;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image criticalIcon;
    private float lifeTimer;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Sequence seq;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnSpawn()
    {
        lifeTimer = 0f;

        // 初始化状态
        rectTransform.localScale = Vector3.zero;
        canvasGroup.alpha = 1f;
        rectTransform.anchoredPosition = Vector2.zero;

        // 动画：缩放 + 漂浮 + 淡出
        Sequence seq = DOTween.Sequence();
        seq.Append(rectTransform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack)); // 出现缩放弹出
        seq.Join(rectTransform.DOAnchorPosY(80f, lifeTime)); // 向上飘
        seq.Join(canvasGroup.DOFade(0f, lifeTime)); // 渐隐
    }

    public void OnRecycle()
    {
        lifeTimer = 0f;
        DOTween.Kill(rectTransform); // 杀死关联动画（重要，防止重复播放）
    }

    private void Update()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer > lifeTime)
        {
            PoolManager.Instance.Recycle(this.gameObject.name, this.gameObject);
        }
    }

    public void Setup(string damageText, Color color, bool isCritical = false)
    {
        isCritical = Random.Range(0f, 1f) > 0.5f ? true : false;

        if (isCritical)
        {
            text.fontSize = 60f;
            criticalIcon.gameObject.SetActive(true);
            criticalIcon.color = color;
        }
        else
        {
            text.fontSize = 35f;
            criticalIcon.gameObject.SetActive(false);
        }
        text.text = damageText;

        text.color = color;
        
    }
}
