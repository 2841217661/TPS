using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(PlayerManager _playerManager, string _animationName, bool _useRootMotionPart) : base(_playerManager, _animationName, _useRootMotionPart)
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

        //应用重力效果
        playerManager.HandleGravity();

        //跳跃输入检检测
        if (playerManager.inputManager.GetJumpInput())
        {
            ChangeState(playerManager.liftState);
            playerManager.animator.CrossFadeInFixedTime(playerManager.liftState.animationName, 0.2f);
            return;
        }

        //ground -> fall
        if (!playerManager.GroundCheck() && playerManager.inAirTimer > 0.1f)
        {
            ChangeState(playerManager.fallState);
            playerManager.animator.CrossFadeInFixedTime(playerManager.fallState.animationName, 0.2f);
            return;
        }


    }
}
