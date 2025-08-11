public abstract class PlayerState
{
    public PlayerManager playerManager;
    public string animationName;
    public bool useRootMotion;

    /// <summary> 是否允许被打断 </summary>
    public virtual bool CanBeInterrupted => true;

    /// <summary> 状态优先级（高优先级可以打断低优先级） </summary>
    public virtual int Priority => 0;

    public PlayerState(PlayerManager _playerManager, string _animationName, bool _useRootMotionPart)
    {
        playerManager = _playerManager;
        animationName = _animationName;
        useRootMotion = _useRootMotionPart;
    }

    public virtual void Enter() { }
    public virtual void Update() {
        ////this -> dodge
        //if (playerManager.inputManager.GetDodgeInput())
        //{
        //    ChangeState(playerManager.dodgeState);
        //    return;
        //}
    }
    public virtual void Exit() { }
    public virtual void FixedUpdate() { }
    public virtual void LateUpdate() { }

    public void ChangeState(PlayerState _newState)
    {
        // 判断是否可打断
        if (playerManager.currentState.CanBeInterrupted && _newState.Priority >= playerManager.currentState.Priority)
        {
            playerManager.currentState.Exit();

            playerManager.currentState = _newState;
            playerManager.currentState.Enter();
        }
    }
}
