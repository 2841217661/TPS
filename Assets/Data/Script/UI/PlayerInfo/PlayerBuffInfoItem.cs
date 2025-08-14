using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBuffInfoItem : MonoBehaviour
{
    public Image UI_Image_BuffIcon;
    public Image UI_Image_ResidulDurationBg;
    public TextMeshProUGUI UI_Text_BuffLevel;

    public BuffBase buff;

    private void Start()
    {
        UI_Image_BuffIcon.sprite = buff.buffData.icon;
    }

    private void Update()
    {
        UI_Text_BuffLevel.text = buff.CurrentLevel.ToString();

        if (!buff.buffData.isPermanent) //只有不是永久性buff才显示持续时间
        {
            UI_Image_ResidulDurationBg.fillAmount = 1 - (buff.ResidualDuration / buff.buffData.maxDuration);
        }
    }
}
