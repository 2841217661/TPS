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

        playerManager.aimImage.SetActive(true); //��ʾ׼��

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

        // ֻȡˮƽ����
        float horizontalDelta = aimInput.x;

        // ��ת�Ƕ� = ���X * ������ * deltaTime
        float rotationAmount = horizontalDelta * playerManager.cameraManager.cameraRotateSpeed * Time.deltaTime;

        // ��Y����ת
        playerManager.transform.Rotate(0f, rotationAmount, 0f);

        RaycastHit hit;

        Vector3 startPos = playerManager.cameraManager.playerCamera.transform.position;
        Vector3 dir = playerManager.cameraManager.playerCamera.transform.forward;

        if (Physics.Raycast(startPos, dir, out hit, 100f, ~playerManager.aimIkIgnoreLayer))
        {
            // ����򵽶�������ɫ���ߣ����ȵ�hit.point
            playerManager.aimAimTarget.position = hit.point;

            float distance = Vector3.Distance(startPos, hit.point);
            DrawDebugAimLine(startPos, dir, distance, Color.green);
        }
        else
        {
            // û���κζ�������ɫ���ߣ�����100
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
            // ��ȡ�ƶ�����
            Vector2 moveInput = playerManager.inputManager.movementInput;

            // ����Animator����
            playerManager.animator.SetFloat("MoveX", moveInput.x, 0.15f, Time.deltaTime);
            playerManager.animator.SetFloat("MoveY", moveInput.y, 0.15f, Time.deltaTime);
        }

        //��ס���ſ��𶯻�
        if (playerManager.inputManager.GetFireInput())
        {
            playerManager.animator.SetBool("Fire", true);
            playerManager.animator.SetLayerWeight(2, 1f);

            //aimCamera��������
            playerManager.cameraManager.aimCameraFireShakeNoise.m_AmplitudeGain = 0.2f;
            playerManager.cameraManager.aimCameraFireShakeNoise.m_FrequencyGain = 20f;
;        }
        else
        {
            playerManager.animator.SetBool("Fire", false);
            playerManager.animator.SetLayerWeight(2, 0f);

            //ȡ������
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

    //�����ӵ�
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
                Debug.LogWarning("�������ӵ�����: " + playerManager.currentUseBulletType);
                break;
        }
    }

    private void GenerateShootSFX()
    {
        PoolManager.Instance.Spawn(PoolManager.Instance.sx_ak47.name, playerManager.shooter.position, Quaternion.identity);
    }

    //����(��Ҫɾ��)
    private void DrawDebugAimLine(Vector3 startPos, Vector3 dir, float length, Color color)
    {
        Debug.DrawRay(startPos, dir * length, color);
    }
}
