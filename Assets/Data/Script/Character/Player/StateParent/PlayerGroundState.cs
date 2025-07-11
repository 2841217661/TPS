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

        //Ӧ������Ч��
        playerManager.HandleGravity();

        //��Ծ�������
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
