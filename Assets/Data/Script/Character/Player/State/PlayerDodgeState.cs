using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : PlayerState
{
    private float dodgeTime = 0.2f;
    private float dodgeTimer;
    private float dodgeSpeedAcc = 5;

    public float ghostLifetime = 0.3f;  // 残影存活时间
    public float ghostCreatInterval = 0.06f; //残影创建间隔
    private float ghostCreatTimer; //残影创建计时器

    public PlayerDodgeState(PlayerManager _playerManager, string _animationName, bool _useRootMotionPart) : base(_playerManager, _animationName, _useRootMotionPart)
    {
    }

    public override void Enter()
    {
        base.Enter();

        ghostCreatTimer = 0f;
        CreateGhost(playerManager.transform); //立即创建一次

        playerManager.animator.Play(animationName);

        dodgeTimer = 0f;
        playerManager.animationMovementMul += dodgeSpeedAcc;

        Vector3 moveDir;
        if (playerManager.inputManager.movementInput == Vector2.zero)
        {
            moveDir = playerManager.transform.forward;
        }
        else
        {
            Vector3 camForward = playerManager.cameraManager.currentCamera.transform.forward;
            Vector3 camRight = playerManager.cameraManager.currentCamera.transform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            moveDir = camForward * playerManager.inputManager.movementInput.y
                            + camRight * playerManager.inputManager.movementInput.x;
        }
        playerManager.transform.rotation = Quaternion.LookRotation(moveDir);
    }

    public override void Exit()
    {
        base.Exit();

        playerManager.animationMovementMul -= dodgeSpeedAcc;
    }

    public override void Update()
    {
        base.Update();

        if(dodgeTimer > dodgeTime)
        {
            ChangeState(playerManager.idleState);
            playerManager.animator.CrossFadeInFixedTime(playerManager.idleState.animationName, 0.2f);
            return;
        }

        dodgeTimer += Time.deltaTime;


        if(ghostCreatTimer > ghostCreatInterval)
        {
            CreateGhost(playerManager.transform);
            ghostCreatTimer = 0;
        }
        else
        {
            ghostCreatTimer += Time.deltaTime;
        }
    }

    //创建一个残影
    public void CreateGhost(Transform target)
    {
        // 创建残影父物体
        GameObject ghost = new GameObject("Ghost");
        ghost.transform.position = target.position;
        ghost.transform.rotation = target.rotation;
        ghost.transform.localScale = target.localScale;

        // 遍历角色所有 SkinnedMeshRenderer
        foreach (SkinnedMeshRenderer smr in target.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            GameObject smrObj = new GameObject(smr.name);
            smrObj.transform.SetParent(ghost.transform);
            smrObj.transform.position = smr.transform.position;
            smrObj.transform.rotation = smr.transform.rotation;
            smrObj.transform.localScale = smr.transform.localScale;

            MeshRenderer mr = smrObj.AddComponent<MeshRenderer>();
            MeshFilter mf = smrObj.AddComponent<MeshFilter>();

            // 烘焙当前动画帧的网格
            Mesh mesh = new Mesh();
            smr.BakeMesh(mesh);
            mf.mesh = mesh;

            // 给残影材质单独实例
            Material matInstance = new Material(playerManager.ghostMaterial);
            mr.material = matInstance;

            // 添加淡出脚本
            smrObj.AddComponent<GhostFade>().Init(matInstance, ghostLifetime);
        }

        GameObject.Destroy(ghost, ghostLifetime);
    }
}
