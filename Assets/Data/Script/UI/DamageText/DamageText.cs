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

        rectTransform.localScale = Vector3.zero;
        canvasGroup.alpha = 1f;
        rectTransform.anchoredPosition = Vector2.zero;

        // 先杀掉旧动画，确保不会重叠播放
        DOTween.Kill(rectTransform);

        seq = DOTween.Sequence();

        // 强力弹出（scale 动画）
        seq.Append(rectTransform.DOScale(1.2f, 0.1f).SetEase(Ease.OutQuad)); // 快速放大
        seq.Append(rectTransform.DOScale(0.9f, 0.1f).SetEase(Ease.InQuad));  // 缩回一点
        seq.Append(rectTransform.DOScale(1.0f, 0.1f).SetEase(Ease.OutElastic)); // 恢复到正常大小

        // 并行：往上飘 + 渐隐（lifeTime - 0.3秒）
        float floatDuration = lifeTime - 0.3f;
        seq.Join(rectTransform.DOAnchorPosY(80f, floatDuration).SetEase(Ease.OutCubic));
        seq.Join(canvasGroup.DOFade(0f, floatDuration).SetEase(Ease.InQuad));
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
