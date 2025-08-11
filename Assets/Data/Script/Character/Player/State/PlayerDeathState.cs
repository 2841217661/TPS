using UnityEngine;

public class PlayerDeathState : PlayerState
{
    public override bool CanBeInterrupted => false; // 受击中不能被其他状态打断
    public override int Priority => 10; // 高优先级
    public PlayerDeathState(PlayerManager _playerManager, string _animationName, bool _useRootMotionPart) : base(_playerManager, _animationName, _useRootMotionPart)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //随机播放一个死亡动画
        playerManager.animator.CrossFadeInFixedTime(Random.Range(0, 1f) > 0.5f ? PlayerAnimationName.Death_1 : PlayerAnimationName.Death_2, 0.2f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
