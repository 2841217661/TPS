using UnityEngine;
using static PlayerManager;

public class PlayerAimState : PlayerGroundState
{
    private float playFootInterval = 0.5f;
    private float playFootIntervaler;
    private bool isFootL;
    public PlayerAimState(PlayerManager _playerManager, string _animationName, bool _useRootMotionPart) : base(_playerManager, _animationName, _useRootMotionPart)
    {
    }

    public override void Enter()
    {
        base.Enter();

        playerManager.shootTimer = 0f;

        playerManager.aimImage.SetActive(true); //显示准星

        playerManager.aimIk.solver.IKPositionWeight = 1;
        playerManager.cameraManager.ChangePlayerCamera(playerManager.cameraManager.aimCamera);

        Vector3 faceDir = playerManager.cameraManager.playerCamera.transform.forward;
        faceDir.y = 0;
        faceDir = faceDir.normalized;
        playerManager.transform.rotation = Quaternion.LookRotation(faceDir);
    }

    public override void Exit()
    {
        base.Exit();

        playFootIntervaler = 0f;
        isFootL = false;

        playerManager.shootTimer = 0f;

        playerManager.aimImage.SetActive(false);


        playerManager.aimIk.solver.IKPositionWeight = 0;

        playerManager.cameraManager.ChangePlayerCamera(playerManager.cameraManager.normalCamera);

        playerManager.animator.SetBool("Fire", false);
        playerManager.animator.SetLayerWeight(2, 0f);
    }

    public override void LateUpdate()
    {
        base.LateUpdate();

        Vector2 aimInput = playerManager.inputManager.cameraInput;

        // 只取水平输入
        float horizontalDelta = aimInput.x;

        // 旋转角度 = 鼠标X * 灵敏度 * deltaTime
        float rotationAmount = horizontalDelta * playerManager.cameraManager.cameraRotateSpeed * Time.deltaTime;

        // 绕Y轴旋转
        playerManager.transform.Rotate(0f, rotationAmount, 0f);

        RaycastHit hit;

        Vector3 startPos = playerManager.cameraManager.playerCamera.transform.position;
        Vector3 dir = playerManager.cameraManager.playerCamera.transform.forward;

        if (Physics.Raycast(startPos, dir, out hit, 100f, ~playerManager.aimIkIgnoreLayer))
        {
            // 如果打到东西，绿色射线，长度到hit.point
            playerManager.aimAimTarget.position = hit.point;

            float distance = Vector3.Distance(startPos, hit.point);
            DrawDebugAimLine(startPos, dir, distance, Color.green);
        }
        else
        {
            // 没打到任何东西，红色射线，长度100
            playerManager.aimAimTarget.position = startPos + dir * 100f;
            DrawDebugAimLine(startPos, dir, 100f, Color.red);
        }

    }
    public override void Update()
    {
        base.Update();

        playFootIntervaler += Time.deltaTime;
        if (playFootIntervaler >= playFootInterval)
        {
            if (isFootL)
            {
                PoolManager.Instance.Spawn(PoolManager.Instance.sx_playerFoot_R.name, playerManager.transform.position, Quaternion.identity);
            }
            else
            {
                PoolManager.Instance.Spawn(PoolManager.Instance.sx_playerFoot_L.name, playerManager.transform.position, Quaternion.identity);
            }
            isFootL = !isFootL;
            playFootIntervaler = 0f;
        }

        //this -> idle
        if (!playerManager.inputManager.GetAimInput())
        {
            ChangeState(playerManager.idleState);
            playerManager.animator.CrossFadeInFixedTime(playerManager.idleState.animationName, 0.2f);
            return;
        }
        else
        {
            // 获取移动输入
            Vector2 moveInput = playerManager.inputManager.movementInput;

            // 更新Animator参数
            playerManager.animator.SetFloat("MoveX", moveInput.x, 0.15f, Time.deltaTime);
            playerManager.animator.SetFloat("MoveY", moveInput.y, 0.15f, Time.deltaTime);
        }

        //按住播放开火动画
        if (playerManager.inputManager.GetFireInput())
        {
            playerManager.animator.SetBool("Fire", true);
            playerManager.animator.SetLayerWeight(2, 1f);

            //aimCamera持续抖动
            playerManager.cameraManager.aimCameraFireShakeNoise.m_AmplitudeGain = 0.2f;
            playerManager.cameraManager.aimCameraFireShakeNoise.m_FrequencyGain = 20f;
;        }
        else
        {
            playerManager.animator.SetBool("Fire", false);
            playerManager.animator.SetLayerWeight(2, 0f);

            //取消抖动
            playerManager.cameraManager.aimCameraFireShakeNoise.m_AmplitudeGain = 0f;
            playerManager.cameraManager.aimCameraFireShakeNoise.m_FrequencyGain = 0f;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        playerManager.shootTimer += Time.fixedDeltaTime;

        if (playerManager.shootTimer >= playerManager.shootInterval && playerManager.inputManager.GetFireInput())
        {
            GenerateBullet();
            GenerateShootSFX();
            playerManager.shootTimer = 0f;
        }
    }

    //生成子弹
    private void GenerateBullet()
    {
        switch (playerManager.currentUseBulletType)
        {
            case CurrentUseBulletType.BulletNormal:
                PoolManager.Instance.Spawn(PoolManager.Instance.bulletNormal.name, playerManager.shooter.position, playerManager.shooter.rotation);
                PoolManager.Instance.Spawn(PoolManager.Instance.fx_bulletFire.name, playerManager.shooter.position, playerManager.shooter.rotation);
                break;
            case CurrentUseBulletType.BulletTrack:
                PoolManager.Instance.Spawn(PoolManager.Instance.bulletTrack.name, playerManager.shooter.position, playerManager.shooter.rotation);
                PoolManager.Instance.Spawn(PoolManager.Instance.fx_bulletFire.name, playerManager.shooter.position, playerManager.shooter.rotation);
                break;
            default:
                Debug.LogWarning("不存在子弹类型: " + playerManager.currentUseBulletType);
                break;
        }
    }

    private void GenerateShootSFX()
    {
        PoolManager.Instance.Spawn(PoolManager.Instance.sx_ak47.name, playerManager.shooter.position, Quaternion.identity);
    }

    //调试(需要删除)
    private void DrawDebugAimLine(Vector3 startPos, Vector3 dir, float length, Color color)
    {
        Debug.DrawRay(startPos, dir * length, color);
    }
}
