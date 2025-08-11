using UnityEngine;

public class PlayerThrowEndState : PlayerGroundState
{
    private float exitByInputTime = 0.7f;
    public PlayerThrowEndState(PlayerManager _playerManager, string _animationName, bool _useRootMotionPart) : base(_playerManager, _animationName, _useRootMotionPart)
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

        //this -> idle
        AnimatorStateInfo stateInfo = playerManager.animator.GetCurrentAnimatorStateInfo(0);
        if(stateInfo.IsName(animationName))
        {
            if (playerManager.animator.IsInTransition(0))
            {
                ChangeState(playerManager.idleState);
            }
            else if(stateInfo.normalizedTime > exitByInputTime)
            {
                if(playerManager.inputManager.movementInput != Vector2.zero)
                {
                    ChangeState(playerManager.idleState);
                }
            }
        }
    }
}
