using UnityEngine;
using static PlayerManager;

public class PlayerAimState : PlayerGroundState
{
    private float playFootInterval = 0.5f;
    private float playFootIntervaler;
    private bool isFootL;
    private Vector3 smoothAimPosition;
    public PlayerAimState(PlayerManager _playerManager, string _animationName, bool _useRootMotionPart) : base(_playerManager, _animationName, _useRootMotionPart)
    {
    }

    public override void Enter()
    {
        base.Enter();

        playerManager.shootTimer = 0f;
        playerManager.aimImage.SetActive(true);

        playerManager.aimIk.solver.IKPositionWeight = 1;
        playerManager.cameraManager.ChangePlayerCamera(playerManager.cameraManager.aimCamera);

        Vector3 faceDir = playerManager.cameraManager.playerCamera.transform.forward;
        faceDir.y = 0;
        faceDir = faceDir.normalized;
        playerManager.transform.rotation = Quaternion.LookRotation(faceDir);

        // 初始化平滑位置
        Vector3 startPos = playerManager.cameraManager.playerCamera.transform.position;
        smoothAimPosition = startPos + playerManager.cameraManager.playerCamera.transform.forward * 10f;
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

        float horizontalDelta = aimInput.x;
        float rotationAmount = horizontalDelta * playerManager.cameraManager.cameraRotateSpeed * Time.deltaTime;
        playerManager.transform.Rotate(0f, rotationAmount, 0f);

        Vector3 camPos = playerManager.cameraManager.playerCamera.transform.position;
        Vector3 camDir = playerManager.cameraManager.playerCamera.transform.forward;
        Vector3 targetPos = camPos + camDir * 10f;

        // 平滑插值
        smoothAimPosition = Vector3.Lerp(smoothAimPosition, targetPos, Time.deltaTime * 20f);

        playerManager.aimAimTarget.position = smoothAimPosition;

        //调试线
        DrawDebugAimLine(camPos, camDir, 10f, Color.red);
    }
    public override void Update()
    {
        base.Update();

        //播放脚步音效
        if(playerManager.inputManager.movementInput != Vector2.zero)
        {
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
            playerManager.cameraManager.AddCurrentCameraShark(0.2f, 8f);
        }
        else
        {
            playerManager.animator.SetBool("Fire", false);
            playerManager.animator.SetLayerWeight(2, 0f);

            //取消抖动
            playerManager.cameraManager.RemoveCurrentCameraShark();
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        playerManager.shootTimer += Time.fixedDeltaTime;

        if (playerManager.shootTimer >= playerManager.currentShootSpeed && playerManager.inputManager.GetFireInput())
        {
            GenerateBullet();
            GenerateShootSFX();
            playerManager.shootTimer = 0f;
        }
    }

    //生成子弹
    private void GenerateBullet()
    {
        Vector3 shootOrigin = playerManager.shooter.position;
        Vector3 camPos = playerManager.cameraManager.playerCamera.transform.position;
        Vector3 camForward = playerManager.cameraManager.playerCamera.transform.forward;

        Vector3 targetPoint;

        if (Physics.Raycast(camPos, camForward, out RaycastHit hit, 100f,~playerManager.notDamageLayer))
        {
            // 命中目标
            targetPoint = hit.point;
        }
        else
        {
            // 没命中，用前方远点
            targetPoint = camPos + camForward * 100f;
        }

        // 计算方向（从枪口 -> 瞄准点）
        Vector3 shootDir = (targetPoint - shootOrigin).normalized;

        // 添加一定角度的偏移
        float maxAngleOffset = 1f;

        // 生成一个随机方向的偏移角度
        float offsetYaw = Random.Range(-maxAngleOffset, maxAngleOffset);     // 水平方向偏移
        float offsetPitch = Random.Range(-maxAngleOffset, maxAngleOffset);   // 垂直方向偏移

        // 对 shootDir 施加旋转
        Quaternion offsetRotation = Quaternion.Euler(offsetPitch, offsetYaw, 0f);
        Vector3 offsetDir = offsetRotation * shootDir;

        Quaternion shootRotation = Quaternion.LookRotation(offsetDir);


        switch (playerManager.currentUseBulletType)
        {
            case CurrentUseBulletType.BulletNormal_Ordinary:
                PoolManager.Instance.Spawn(PoolManager.Instance.bulletNormal_Ordinary.name, shootOrigin, shootRotation);
                PoolManager.Instance.Spawn(PoolManager.Instance.fx_bulletNormal_Ordinary_Fire.name, shootOrigin, shootRotation);
                break;
            case CurrentUseBulletType.BulletNormal_Flame:
                PoolManager.Instance.Spawn(PoolManager.Instance.bulletNormal_Flame.name, shootOrigin, shootRotation);
                PoolManager.Instance.Spawn(PoolManager.Instance.fx_bulletNormal_Flame_Fire.name, shootOrigin, shootRotation);
                break;
            case CurrentUseBulletType.BulletTrack_Ordinary:
                PoolManager.Instance.Spawn(PoolManager.Instance.bulletTrack_Ordinary.name, shootOrigin, shootRotation);
                PoolManager.Instance.Spawn(PoolManager.Instance.fx_bulletNormal_Ordinary_Fire.name, shootOrigin, shootRotation);
                break;
            default:
                Debug.LogWarning("不存在子弹类型: " + playerManager.currentUseBulletType);
                break;
        }
    }


    private void GenerateShootSFX()
    {
        switch (playerManager.currentUseBulletType)
        {
            case CurrentUseBulletType.BulletNormal_Ordinary:
                PoolManager.Instance.Spawn(PoolManager.Instance.sx_ak47.name, playerManager.shooter.position, Quaternion.identity);
                break;
            case CurrentUseBulletType.BulletNormal_Flame:
                PoolManager.Instance.Spawn(PoolManager.Instance.sx_ak47_normal_flame.name, playerManager.shooter.position, Quaternion.identity);
                break;
            default:
                Debug.LogWarning("开火类型与当前子弹类型不匹配？？？");
                break;
        }
    }

    //调试(需要删除)
    private void DrawDebugAimLine(Vector3 startPos, Vector3 dir, float length, Color color)
    {
        Debug.DrawRay(startPos, dir * length, color);
    }
}
