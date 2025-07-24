using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFadePanel : BasePanel
{
    private Image image;

    [Header("渐进和渐出时间")]
    public float fadeInTime = 1f;
    public float fadeOutTime = 1f;

    private void Awake()
    {
        image = GetComponent<Image>();
        var color = image.color;
        color.a = 0f;
        image.color = color;
    }

    public Tween FadeIn(System.Action onStart = null, System.Action onComplete = null)
    {
        return image.DOFade(1f, fadeInTime)
            .SetEase(Ease.Linear)
            .SetUpdate(true)
            .OnStart(() => onStart?.Invoke())
            .OnComplete(() => onComplete?.Invoke());
    }

    public Tween FadeOut(System.Action onStart = null, System.Action onComplete = null)
    {
        return image.DOFade(0f, fadeOutTime)
            .SetEase(Ease.Linear)
            .SetUpdate(true)
            .OnStart(() => onStart?.Invoke())
            .OnComplete(() => onComplete?.Invoke());
    }
}
