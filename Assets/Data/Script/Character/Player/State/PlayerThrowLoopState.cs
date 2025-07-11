using UnityEngine;

public class PlayerThrowLoopState : PlayerGroundState
{
    private LineRenderer lineRenderer;
    private GameObject lineRendererObject; // �洢GameObject����
    private GameObject currentTargetIndicator;

    public PlayerThrowLoopState(PlayerManager _playerManager, string _animationName, bool _useRootMotionPart)
        : base(_playerManager, _animationName, _useRootMotionPart)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // ����LineRenderer
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

        // �л����
        playerManager.cameraManager.ChangePlayerCamera(playerManager.cameraManager.normalCamera);

        // ���ٹ켣
        if (lineRendererObject != null)
        {
            GameObject.Destroy(lineRendererObject);
            lineRenderer = null;
            lineRendererObject = null;
        }

        // �������б��
        if (currentTargetIndicator != null)
        {
            GameObject.Destroy(currentTargetIndicator);
            currentTargetIndicator = null;
        }
    }

    public override void Update()
    {
        base.Update();

        //�˳��߼�
        if (playerManager.inputManager.GetThrowInput())
        {
            ChangeState(playerManager.idleState);
            playerManager.animator.CrossFadeInFixedTime(playerManager.idleState.animationName, 0.2f);
            return;
        }

        //��ת
        Vector2 aimInput = playerManager.inputManager.cameraInput;
        float horizontalDelta = aimInput.x;
        float rotationAmount = horizontalDelta * playerManager.cameraManager.cameraRotateSpeed * Time.deltaTime;
        playerManager.transform.Rotate(0f, rotationAmount, 0f);

        //��ʾ�켣
        if(aimInput != Vector2.zero)
            UpdateTrajectory();

        //�׳�
        if (playerManager.inputManager.GetFireInput())
        {
            ChangeState(playerManager.throwEndState);
            playerManager.animator.CrossFade(playerManager.throwEndState.animationName, 0.2f);
            return;
        }
    }

    private void UpdateTrajectory()
    {
        Vector3 startPos = playerManager.grenadeSpawnPoint.position;
        Vector3 startVelocity = playerManager.cameraManager.playerCamera.transform.forward * 15f + Vector3.up * 5f;

        int steps = 30;
        float timeStep = 0.1f;

        Vector3[] points = new Vector3[steps];

        for (int i = 0; i < steps; i++)
        {
            float t = i * timeStep;
            Vector3 point = startPos + t * startVelocity + 0.5f * Physics.gravity * t * t;
            points[i] = point;

            if (i > 0)
            {
                Ray ray = new Ray(points[i - 1], points[i] - points[i - 1]);
                float dist = Vector3.Distance(points[i - 1], points[i]);

                if (Physics.Raycast(ray, out RaycastHit hit, dist))
                {
                    points[i] = hit.point;

                    //ʵ�����ƶ�����ָʾ��
                    if (currentTargetIndicator == null)
                    {
                        currentTargetIndicator = GameObject.Instantiate(
                            playerManager.grenadeTargetPrefab,
                            hit.point,
                            Quaternion.identity);
                    }
                    else
                    {
                        currentTargetIndicator.transform.position = hit.point;
                    }

                    //ֻ���Ƶ����е�
                    Vector3[] shortened = new Vector3[i + 1];
                    System.Array.Copy(points, shortened, i + 1);
                    lineRenderer.positionCount = shortened.Length;
                    lineRenderer.SetPositions(shortened);
                    return;
                }
            }
        }

        //������
        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);

        //������ʱ���Ƴ�ָʾ��
        if (currentTargetIndicator != null)
        {
            GameObject.Destroy(currentTargetIndicator);
            currentTargetIndicator = null;
        }
    }
}
