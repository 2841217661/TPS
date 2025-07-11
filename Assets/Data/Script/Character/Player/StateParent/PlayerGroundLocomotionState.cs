using UnityEngine;

public class PlayerGroundLocomotionState : PlayerGroundState
{
    public PlayerGroundLocomotionState(PlayerManager _playerManager, string _animationName, bool _useRootMotionPart) : base(_playerManager, _animationName, _useRootMotionPart)
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

        //this -> aim
        if (playerManager.inputManager.GetAimInput())
        {
            ChangeState(playerManager.aimState);
            playerManager.animator.CrossFadeInFixedTime(playerManager.aimState.animationName, 0.2f);
        }
        //this -> throwStart
        else if (playerManager.inputManager.GetThrowInput())
        {
            ChangeState(playerManager.throwStartState);
            playerManager.animator.CrossFadeInFixedTime(playerManager.throwStartState.animationName, 0.2f);
            return;
        }


        //�����˶���̬��,����һ�ι�����
        //ǰ���е��� -> ����� ǰ���޵��� -> ǰ������ -> ����
        if (playerManager.inputManager.GetAttackInput())
        {
            //ǰ���е���
            if (playerManager.EnemyAttackCheck().Length > 0)
            {
                //this -> elbowStrike
                if (!playerManager.animator.GetCurrentAnimatorStateInfo(0).IsName(playerManager.elbowStrikeState.animationName))
                {
                    ChangeState(playerManager.elbowStrikeState);
                    playerManager.animator.CrossFadeInFixedTime(playerManager.elbowStrikeState.animationName, 0.2f);
                    return;
                }
            }
            //ǰ��û�е��ˣ�����
            else if(playerManager.DoorAttackCheck().Length > 0)
            {
                //this -> kick
                if (!playerManager.animator.GetCurrentAnimatorStateInfo(0).IsName(playerManager.kickState.animationName))
                {
                    ChangeState(playerManager.kickState);
                    playerManager.animator.CrossFadeInFixedTime(playerManager.kickState.animationName, 0.2f);
                    return;
                }
            }
            //this -> elbowStrike
            if (!playerManager.animator.GetCurrentAnimatorStateInfo(0).IsName(playerManager.elbowStrikeState.animationName))
            {
                ChangeState(playerManager.elbowStrikeState);
                playerManager.animator.CrossFadeInFixedTime(playerManager.elbowStrikeState.animationName, 0.2f);
                return;
            }
        }
    }

    private void HandleRotate()
    {
        // ����Ŀ����ת����
        Vector3 targetRotationDirection = playerManager.cameraManager.playerCamera.transform.forward * playerManager.inputManager.movementInput.y
                                        + playerManager.cameraManager.playerCamera.transform.right * playerManager.inputManager.movementInput.x;

        targetRotationDirection.Normalize();
        targetRotationDirection.y = 0f;

        // û���������
        if (targetRotationDirection == Vector3.zero)
        {
            targetRotationDirection = playerManager.transform.forward;
        }

        // ����Ŀ����ת
        Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);

        float rotationSpeed = playerManager.rotateSpeed;


        // ƽ����ת��Ŀ��
        Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        playerManager.transform.rotation = targetRotation;
    }

}
