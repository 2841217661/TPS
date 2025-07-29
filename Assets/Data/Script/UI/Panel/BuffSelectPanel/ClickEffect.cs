using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class ClickEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private bool isPointerOver = false;

    public BuffData buffData;
    public Image icon;
    public TextMeshProUGUI description;
    [HideInInspector] public BuffSelectPanel buffSelectPanel;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        icon.sprite = buffData.icon;
        description.text = buffData.buffDescript;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
        // 鼠标悬停时轻微放大
        rectTransform.DOScale(1.05f, 0.15f).SetEase(Ease.OutQuad);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
        // 鼠标离开时恢复正常大小（如果没有点击状态）
        rectTransform.DOScale(1f, 0.15f).SetEase(Ease.OutQuad);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 鼠标按下时缩小
        rectTransform.DOScale(0.9f, 0.1f).SetEase(Ease.OutQuad);
        canvasGroup.alpha = 0.7f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 松开后，根据是否在悬停状态决定缩放
        float targetScale = isPointerOver ? 1.05f : 1f;
        rectTransform.DOScale(targetScale, 0.15f).SetEase(Ease.OutBack);
        canvasGroup.alpha = 1f;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().interactable = false;

        // 点击动画
        rectTransform.DOScale(1.15f, 0.08f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                rectTransform.DOScale(1f, 0.08f).SetEase(Ease.OutQuad);
            });

        // 延迟关闭面板
        DOVirtual.DelayedCall(0.2f, () =>
        {
            buffSelectPanel.ClosePanel();
        });
    }
}
