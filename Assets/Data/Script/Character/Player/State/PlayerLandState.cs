using UnityEngine;

public class PlayerLandState : PlayerGroundState
{
    public PlayerLandState(PlayerManager _playerManager, string _animationName, bool _useRootMotionPart) : base(_playerManager, _animationName, _useRootMotionPart)
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

        AnimatorStateInfo stateInfo = playerManager.animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName(animationName) && playerManager.animator.IsInTransition(0))
        {
            ChangeState(playerManager.idleState);
            return;
        }

        if(stateInfo.IsName(animationName)  && playerManager.inputManager.movementInput.magnitude != 0f)
        {
            ChangeState(playerManager.idleState);
            playerManager.animator.CrossFadeInFixedTime(playerManager.idleState.animationName, 0.2f);
            return;
        }
    }
}
