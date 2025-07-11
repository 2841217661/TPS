using UnityEngine;

public class PlayerAirLocomotionState : PlayerAirState
{
    public PlayerAirLocomotionState(PlayerManager _playerManager, string _animationName, bool _useRootMotionPart) : base(_playerManager, _animationName, _useRootMotionPart)
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

        HandleRotate();
        HandleMove();
    }

    private void HandleMove()
    {
        // 计算目标方向
        Vector3 targetMoveDirection = playerManager.cameraManager.playerCamera.transform.forward * playerManager.inputManager.movementInput.y
                                        + playerManager.cameraManager.playerCamera.transform.right * playerManager.inputManager.movementInput.x;

        targetMoveDirection.Normalize();
        targetMoveDirection.y = 0f;

        playerManager.characterController.Move(targetMoveDirection * playerManager.airMoveSpeed * Time.deltaTime);
    }

    private void HandleRotate()
    {
        // 计算目标旋转方向
        Vector3 targetRotationDirection = playerManager.cameraManager.playerCamera.transform.forward * playerManager.inputManager.movementInput.y
                                        + playerManager.cameraManager.playerCamera.transform.right * playerManager.inputManager.movementInput.x;

        targetRotationDirection.Normalize();
        targetRotationDirection.y = 0f;

        // 没有相机输入
        if (targetRotationDirection == Vector3.zero)
        {
            targetRotationDirection = playerManager.transform.forward;
        }

        // 计算目标旋转
        Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);

        float rotationSpeed = playerManager.rotateSpeed;


        // 平滑旋转到目标
        Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        playerManager.transform.rotation = targetRotation;
    }
}
