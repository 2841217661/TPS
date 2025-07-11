using UnityEngine;

public class PlayerLiftState : PlayerAirLocomotionState
{
    public PlayerLiftState(PlayerManager _playerManager, string _animationName, bool _useRootMotionPart) : base(_playerManager, _animationName, _useRootMotionPart)
    {
    }

    public override void Enter()
    {
        base.Enter();

        playerManager.yVelocity.y = Mathf.Sqrt(playerManager.jumpHeight * -2f * playerManager.gravity);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //lift -> idle
        if (playerManager.isGrounded && playerManager.yVelocity.y < 0f)
        {
            ChangeState(playerManager.idleState);
            playerManager.animator.CrossFadeInFixedTime(playerManager.idleState.animationName, 0.2f);
            return;
        }

        //lift -> fall
        AnimatorStateInfo stateInfo = playerManager.animator.GetCurrentAnimatorStateInfo(0);
        if(stateInfo.IsName(animationName) && playerManager.animator.IsInTransition(0))
        {
            ChangeState(playerManager.fallState);
            return;
        }
    }
}
