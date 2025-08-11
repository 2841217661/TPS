using Cinemachine;
using UnityEngine;

public class PlayerCameraManager : MonoBehaviour
{
    [Header("Camera Setting")]
    public float cameraRotateSpeed = 5f;

    private PlayerManager playerManager;

    public Camera playerCamera;
    public CinemachineVirtualCamera currentCamera;
    public CinemachineVirtualCamera normalCamera;
    public CinemachineVirtualCamera aimCamera;
    public CinemachineVirtualCamera throwCamera;


    [Header("Camera Follow Target")]
    [Tooltip("虚拟相机跟随的目标 Transform")]
    public Transform cameraFollowTarget;

    // 累积旋转角度
    private float yaw;
    private float pitch;

    [Header("Pitch限制")]
    public float minPitch = -30f;
    public float maxPitch = 45f;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();

        // 初始化旋转
        if (cameraFollowTarget != null)
        {
            Vector3 euler = cameraFollowTarget.rotation.eulerAngles;
            yaw = euler.y;
            pitch = euler.x;
        }
    }

    public void ChangePlayerCamera(CinemachineVirtualCamera targetCamera)
    {
        currentCamera.Priority = 1;
        targetCamera.Priority = 10;
        currentCamera = targetCamera;
    }

    public void SetCameraRotation()
    {
        Vector2 input = playerManager.inputManager.cameraInput;

        // 鼠标水平旋转
        yaw += input.x * cameraRotateSpeed * Time.deltaTime;

        // 鼠标垂直旋转
        pitch -= input.y * cameraRotateSpeed * Time.deltaTime;

        // 限制俯仰
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // 更新旋转
        if (cameraFollowTarget != null)
        {
            cameraFollowTarget.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }
    }

    //添加当前相机抖动
    public void AddCurrentCameraShark(float m_AmplitudeGain, float m_FrequencyGain)
    {
        CinemachineBasicMultiChannelPerlin c = currentCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        c.m_AmplitudeGain = m_AmplitudeGain;
        c.m_FrequencyGain = m_FrequencyGain;
    }

    //取消当前相机抖动
    public void RemoveCurrentCameraShark()
    {
        CinemachineBasicMultiChannelPerlin aim = aimCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        aim.m_AmplitudeGain = 0;
        aim.m_FrequencyGain = 0;

        CinemachineBasicMultiChannelPerlin normal = normalCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        normal.m_AmplitudeGain = 0;
        normal.m_FrequencyGain = 0;

        CinemachineBasicMultiChannelPerlin thro = throwCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        thro.m_AmplitudeGain = 0;
        thro.m_FrequencyGain = 0;
    }

    //相机脉冲抖动
    public void ApplyImpluseCameraShark(CinemachineImpulseSource impulseSource,float explosionForce)
    {
        impulseSource.GenerateImpulseWithForce(explosionForce);
    }
}
