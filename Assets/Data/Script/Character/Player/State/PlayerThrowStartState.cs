using UnityEngine;

public class PlayerThrowStartState : PlayerGroundState
{
    public PlayerThrowStartState(PlayerManager _playerManager, string _animationName, bool _useRootMotionPart) : base(_playerManager, _animationName, _useRootMotionPart)
    {
    }

    public override void Enter()
    {
        base.Enter();

        playerManager.cameraManager.ChangePlayerCamera(playerManager.cameraManager.throwCamera);

        //面向前方
        Vector3 faceDir = playerManager.cameraManager.playerCamera.transform.forward;
        faceDir.y = 0;
        faceDir = faceDir.normalized;
        playerManager.transform.rotation = Quaternion.LookRotation(faceDir);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        AnimatorStateInfo stateInfo = playerManager.animator.GetCurrentAnimatorStateInfo(0);
        //this -> throwLoop
        if(stateInfo.IsName(animationName) && playerManager.animator.IsInTransition(0))
        {
            ChangeState(playerManager.throwLoopState);
            playerManager.animator.CrossFadeInFixedTime(playerManager.throwLoopState.animationName, 0.2f);
        }

        //旋转
        Vector2 aimInput = playerManager.inputManager.cameraInput;
        float horizontalDelta = aimInput.x;
        float rotationAmount = horizontalDelta * playerManager.cameraManager.cameraRotateSpeed * Time.deltaTime;
        playerManager.transform.Rotate(0f, rotationAmount, 0f);
    }
}
