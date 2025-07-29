using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public Image UI_Image_ProfilePicture; //玩家头像
    public TextMeshProUGUI UI_Text_PlayerName; //玩家姓名
    public Image UI_Image_PlayerHealthBar; //玩家血条
    public Image UI_Image_PlayerExperienceBar; //玩家经验条
    public TextMeshProUGUI UI_Text_PlayerLevel; //玩家等级
    public Transform UI_Rect_BuffContent; //buff信息显示容器
    public GameObject BuffInfoItemPre; //单个buff信息


    //每当player添加了一个新buff时，都应该调用该方法
    public void AddPlayerBuffInfoItem(BuffBase _newBuff)
    {
        var item = Instantiate(BuffInfoItemPre,UI_Rect_BuffContent);
        PlayerBuffInfoItem playerBuffInfoItem = item.GetComponent<PlayerBuffInfoItem>();
        playerBuffInfoItem.buff = _newBuff;
    }

    //每当player移除了一个新buff时，都应该调用该方法
    public void RemovePlayerBuffInfoItem(BuffBase _targetBuff)
    {
        for(int i = 0; i < UI_Rect_BuffContent.childCount; i++)
        {
            if (UI_Rect_BuffContent.GetChild(i).GetComponent<PlayerBuffInfoItem>().buff == _targetBuff)
            {
                Destroy(UI_Rect_BuffContent.GetChild(i));
                return;
            }
        }
    }
}
