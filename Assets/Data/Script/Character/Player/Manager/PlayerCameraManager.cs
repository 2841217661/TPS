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

    public CinemachineBasicMultiChannelPerlin aimCameraFireShakeNoise {  get; private set; }

    [Header("Camera Follow Target")]
    [Tooltip("������������Ŀ�� Transform")]
    public Transform cameraFollowTarget;

    // �ۻ���ת�Ƕ�
    private float yaw;
    private float pitch;

    [Header("Pitch����")]
    public float minPitch = -30f;
    public float maxPitch = 70f;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();

        // ��ʼ����ת
        if (cameraFollowTarget != null)
        {
            Vector3 euler = cameraFollowTarget.rotation.eulerAngles;
            yaw = euler.y;
            pitch = euler.x;
        }

        aimCameraFireShakeNoise = aimCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
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

        // ���ˮƽ��ת
        yaw += input.x * cameraRotateSpeed * Time.deltaTime;

        // ��괹ֱ��ת
        pitch -= input.y * cameraRotateSpeed * Time.deltaTime;

        // ���Ƹ���
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // ������ת
        if (cameraFollowTarget != null)
        {
            cameraFollowTarget.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }
    }
}
