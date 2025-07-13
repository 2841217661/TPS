using UnityEngine;

public class PlayerFallState : PlayerAirLocomotionState
{
    public PlayerFallState(PlayerManager _playerManager, string _animationName, bool _useRootMotionPart) : base(_playerManager, _animationName, _useRootMotionPart)
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

        //fall -> land
        if (playerManager.isGrounded && playerManager.yVelocity.y < 0f)
        {
            if(playerManager.inputManager.movementInput != Vector2.zero)
            {
                ChangeState(playerManager.idleState);
                playerManager.animator.CrossFadeInFixedTime(playerManager.idleState.animationName, 0.2f);
            }
            else
            {
                ChangeState(playerManager.landState);
                playerManager.animator.CrossFadeInFixedTime(playerManager.landState.animationName, 0.2f);
            }
        }
    }
}
