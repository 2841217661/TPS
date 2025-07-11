using UnityEngine;

public class PlayerIdleState : PlayerGroundLocomotionState
{
    public PlayerIdleState(PlayerManager _playerManager, string _animationName, bool _useRootMotionPart) : base(_playerManager, _animationName, _useRootMotionPart)
    {
    }

    public override void Enter()
    {
        base.Enter();

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (playerManager.inputManager.movementInput != Vector2.zero)
        {
            string animName = "";

            //获取当前玩家应当移动的方向向量
            Vector3 targetRotationDirection = playerManager.cameraManager.playerCamera.transform.forward * playerManager.inputManager.movementInput.y
                                            + playerManager.cameraManager.playerCamera.transform.right * playerManager.inputManager.movementInput.x;

            targetRotationDirection.y = 0f;
            targetRotationDirection.Normalize();

            float angle = Vector3.SignedAngle(
                playerManager.transform.forward,
                targetRotationDirection,
                Vector3.up
            );


            if (angle > 45f && angle < 135f)
            {
                animName = "WalkStart90_R";
            }
            else if (angle < -45f && angle > -135f)
            {
                animName = "WalkStart90_L";
            }
            else if (angle >= -180 && angle <= -135f)
            {
                animName = "WalkStart180_L";
            }
            else if (angle >= 135f && angle <= 180f)
            {
                animName = "WalkStart180_R";
            }
            else
            {
                animName = "WalkStart";
            }

            ChangeState(playerManager.walkState);
            playerManager.animator.CrossFadeInFixedTime(playerManager.walkState.animationName, 0.2f);


            playerManager.animator.CrossFadeInFixedTime(animName, 0.2f, 1);
        }
    }
}
