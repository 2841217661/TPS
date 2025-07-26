using UnityEngine;

public class PlayerMinmapArrow : MonoBehaviour
{
    public RectTransform arrow;
    public RectTransform cameraDir;

    private void LateUpdate()
    {
        // 获取玩家Y轴旋转角度(0-360)
        float playerRotationY = GameManager.Instance.playerManager.transform.eulerAngles.y;

        // 设置箭头旋转(注意UI旋转是反向的)
        arrow.localRotation = Quaternion.Euler(0, 0, -playerRotationY);

        float cameraRotationY = Camera.main.transform.eulerAngles.y;
        cameraDir.localRotation = Quaternion.Euler(0,0, -cameraRotationY);
    }
}