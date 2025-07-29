using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBuffInfoItem : MonoBehaviour
{
    public Image UI_Image_BuffIcon;
    public TextMeshProUGUI UI_Text_BuffLevel;
    public TextMeshProUGUI UI_Text_BuffDuraction;

    public BuffBase buff;

    private void Start()
    {
        UI_Image_BuffIcon.sprite = buff.BuffData.icon;
    }

    private void Update()
    {
        UI_Text_BuffLevel.text = buff.CurrentLevel.ToString();

        if (!buff.BuffData.isPermanent) //只有不是永久性buff才显示持续时间
        {
            UI_Text_BuffDuraction.text = buff.DurationScale.ToString();
        }
    }
}
