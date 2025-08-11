using UnityEngine;

public class PlayerHitState : PlayerGroundState
{
    private float exitStateByInput = 0.7f; //当动画播放到70%的时候可以通过移动输入打断改状态
    public Vector3 hitTargetFaceDir; //应该面向受击来源
    private float rotationSpeed = 5f;
    private string[] animNames = new string[]
    {
        PlayerAnimationName.Hit_1,
        PlayerAnimationName.Hit_2,
        PlayerAnimationName.Hit_3,
        PlayerAnimationName.Hit_4
    };

    public PlayerHitState(PlayerManager _playerManager, string _animationName, bool _useRootMotionPart) : base(_playerManager, _animationName, _useRootMotionPart)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //随机播放一个受击动画
        string animName = animNames[Random.Range(0, animNames.Length)];
        playerManager.animator.CrossFadeInFixedTime(animName, 0.1f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        hitTargetFaceDir.y = 0f;
        playerManager.transform.rotation = Quaternion.Slerp(playerManager.transform.rotation, Quaternion.LookRotation(hitTargetFaceDir), rotationSpeed * Time.deltaTime);

        AnimatorStateInfo stateInfo = playerManager.animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsTag(animationName))
        {
            if (playerManager.animator.IsInTransition(0))
            {
                ChangeState(playerManager.idleState);

            }
            else if (stateInfo.normalizedTime > exitStateByInput)
            {
                if (playerManager.inputManager.movementInput != Vector2.zero)
                {
                    ChangeState(playerManager.idleState);
                }
            }
        }
    }
}
