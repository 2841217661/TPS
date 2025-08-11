using UnityEditor;
using UnityEngine;

public class PlayerThrowLoopState : PlayerGroundState
{
    private LineRenderer lineRenderer;
    private GameObject lineRendererObject;
    private GameObject targetMarker; // 命中点标记
    private float throwForce = 15f; //投射力度

    private Vector3 throwDirection;

    public PlayerThrowLoopState(PlayerManager _playerManager, string _animationName, bool _useRootMotionPart)
        : base(_playerManager, _animationName, _useRootMotionPart)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // 创建LineRenderer
        if (lineRenderer == null)
        {
            lineRendererObject = new GameObject("GrenadeTrajectory");
            lineRenderer = lineRendererObject.AddComponent<LineRenderer>();
            lineRenderer.positionCount = 0;
            lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
            lineRenderer.material.color = Color.green;
            lineRenderer.widthMultiplier = 0.03f;
        }

        lineRenderer.enabled = true;

    }

    public override void Exit()
    {
        base.Exit();

        lineRenderer.enabled = false;

        // 切换相机
        playerManager.cameraManager.ChangePlayerCamera(playerManager.cameraManager.normalCamera);

        // 销毁轨迹和标记
        if (lineRendererObject != null)
        {
            GameObject.Destroy(lineRendererObject);
            lineRenderer = null;
            lineRendererObject = null;
        }

        ClearTargetMarker(); // 新增：退出时清除标记
    }

    private void ClearTargetMarker()
    {
        if (targetMarker != null)
        {
            GameObject.Destroy(targetMarker);
            targetMarker = null;
        }
    }

    public override void Update()
    {
        base.Update();
        
        //退出逻辑
        if (playerManager.inputManager.GetThrowInput())
        {
            ChangeState(playerManager.idleState);
            playerManager.animator.CrossFadeInFixedTime(playerManager.idleState.animationName, 0.2f);
            return;
        }

        //旋转
        Vector2 aimInput = playerManager.inputManager.cameraInput;
        float horizontalDelta = aimInput.x;
        float rotationAmount = horizontalDelta * playerManager.cameraManager.cameraRotateSpeed * Time.deltaTime;
        playerManager.transform.Rotate(0f, rotationAmount, 0f);

        //抛出
        if (playerManager.inputManager.GetFireInput())
        {
            ThrowGrenade();
            ChangeState(playerManager.throwEndState);
            playerManager.animator.CrossFade(playerManager.throwEndState.animationName, 0.2f);
            return;
        }
    }
    private void ThrowGrenade()
    {
        // 实例化当前选中的消耗品预制体
        var throwItem = GameManager.Instance.UseCurrentSelectItem(playerManager.currentSelectConItemSO.itemId);
        throwItem.transform.position = playerManager.grenadeSpawnPoint.position;

        // 获取刚体并精确配置
        Rigidbody rb = throwItem.GetComponent<Rigidbody>();

        // 计算与预测完全一致的初速度
        Vector3 throwVelocity = GetPredictedThrowVelocity();
        rb.linearVelocity = throwVelocity;

        // 添加随机旋转（可选）
        rb.AddTorque(new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)),
            ForceMode.Impulse);
    }

    private Vector3 GetPredictedThrowVelocity()
    {
        return throwDirection * throwForce; // 与预测相同的力度
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
        UpdateTrajectory();
    }

    private void UpdateTrajectory()
    {
        Vector3 startPos = playerManager.grenadeSpawnPoint.position;

        // 分解Rotation
        Quaternion horizontalRotation = Quaternion.Euler(0f, playerManager.transform.eulerAngles.y, 0f);
        Quaternion pitchRotation = Quaternion.Euler(playerManager.cameraManager.playerCamera.transform.eulerAngles.x, 0f, 0f);

        // 合成投掷方向
        Quaternion finalRotation = horizontalRotation * pitchRotation;
        throwDirection = finalRotation * Vector3.forward;

        // 加一个向上偏移
        throwDirection += Vector3.up * 0.5f;
        throwDirection.Normalize();

        // 抛射初速度
        Vector3 startVelocity = throwDirection * throwForce;

        int steps = 30;
        float timeStep = 0.1f;
        Vector3[] points = new Vector3[steps];
        bool hasHit = false; // 是否命中标志

        for (int i = 0; i < steps; i++)
        {
            float t = i * timeStep;
            Vector3 point = startPos + t * startVelocity + 0.5f * Physics.gravity * t * t;
            points[i] = point;

            if (i > 0)
            {
                Ray ray = new Ray(points[i - 1], points[i] - points[i - 1]);
                float dist = Vector3.Distance(points[i - 1], points[i]);

                if (Physics.Raycast(ray, out RaycastHit hit, dist, ~playerManager.notDamageLayer))
                {
                    points[i] = hit.point;
                    hasHit = true;

                    // 更新或创建命中点标记
                    UpdateTargetMarker(hit.point);

                    // 只绘制到命中点
                    Vector3[] shortened = new Vector3[i + 1];
                    System.Array.Copy(points, shortened, i + 1);
                    lineRenderer.positionCount = shortened.Length;
                    lineRenderer.SetPositions(shortened);
                    break;
                }
            }
        }

        // 无命中时处理
        if (!hasHit)
        {
            lineRenderer.positionCount = points.Length;
            lineRenderer.SetPositions(points);
            ClearTargetMarker(); // 无命中时清除标记
        }
    }

    // 新增：更新命中点标记
    private void UpdateTargetMarker(Vector3 hitPoint)
    {
        if (targetMarker == null)
        {
            targetMarker = GameObject.Instantiate(
                playerManager.grenadeTargetPrefab,
                hitPoint,
                Quaternion.identity);
        }
        else
        {
            targetMarker.transform.position = hitPoint;
        }
    }
}