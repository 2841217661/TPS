using UnityEngine;

public class PlayerRunState : PlayerGroundLocomotionState
{
    private float playFootInterval = 0.35f;
    private float playFootIntervaler;
    private bool isFootL;
    public PlayerRunState(PlayerManager _playerManager, string _animationName, bool _useRootMotionPart) : base(_playerManager, _animationName, _useRootMotionPart)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        playFootIntervaler = 0f;
        isFootL = false;
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

        if (playerManager.inputManager.movementInput.magnitude == 0)
        {
            ChangeState(playerManager.runEndState);
            playerManager.animator.CrossFadeInFixedTime(playerManager.runEndState.animationName, 0.2f);
        }
    }
}
