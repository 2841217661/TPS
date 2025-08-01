using DG.Tweening;
using System;

public static class UIUtils
{
    /// <summary>
    /// 屏幕淡入 → 执行中间操作 → 等待 → 淡出 → 结束
    /// </summary>
    /// <param name="delay">淡入完成后等待的时间</param>
    /// <param name="onFadeInStart">淡入开始时执行</param>
    /// <param name="onFadeInComplete">淡入完成后执行</param>
    /// <param name="onFadeOutStart">淡出开始时执行</param>
    /// <param name="onFadeOutComplete">淡出完成后执行</param>
    public static void ScreenFadeTransition(
        float delay,
        Action onFadeInStart = null,
        Action onFadeInComplete = null,
        Action onFadeOutStart = null,
        Action onFadeOutComplete = null
    )
    {
        // 打开黑幕面板
        ScreenFadePanel panel = UIManager.Instance.OpenPanel("ScreenFadePanel", UIManager.Instance.UIRoot) as ScreenFadePanel;

        panel.FadeIn(
            onStart: () =>
            {
                onFadeInStart?.Invoke();
            },
            onComplete: () =>
            {
                onFadeInComplete?.Invoke();

                // 延迟后执行 FadeOut
                DOVirtual.DelayedCall(delay, () =>
                {
                    panel.FadeOut(
                        onStart: () =>
                        {
                            onFadeOutStart?.Invoke();
                        },
                        onComplete: () =>
                        {
                            onFadeOutComplete?.Invoke();
                            panel.ClosePanel();
                        });
                });
            });
    }
}
