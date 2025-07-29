using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamageScreenEffect : MonoBehaviour
{
    [SerializeField] private Image redImage;
    [SerializeField] private float flashDuration = 0.4f; // 总闪烁时间
    [SerializeField] private float maxAlpha = 0.6f;       // 红色最高透明度

    private Sequence damageFlashSeq;

    private void Awake()
    {
        redImage.color = new Color(1, 0, 0, 0); // 初始为透明
    }

    public void PlayFlash()
    {
        // 如果正在播放中，先杀掉它
        if (damageFlashSeq != null && damageFlashSeq.IsActive())
            damageFlashSeq.Kill();

        // 创建新的闪烁动画
        damageFlashSeq = DOTween.Sequence();
        damageFlashSeq.Append(redImage.DOFade(maxAlpha, flashDuration * 0.25f)) // 快速显现
                       .Append(redImage.DOFade(0, flashDuration * 0.75f))       // 再慢慢淡出
                       .SetEase(Ease.OutQuad);
    }
}
