using UnityEngine;

public class PlayerState
{
    public PlayerManager playerManager;

    public string animationName;

    public bool useRootMotion;

    public PlayerState(PlayerManager _playerManager, string _animationName, bool _useRootMotionPart)
    {
        playerManager = _playerManager;
        animationName = _animationName;
        useRootMotion = _useRootMotionPart;
    }

    public virtual void Enter()
    {
        //�����߼�

    }

    public virtual void Update()
    {
        // �����߼�
    }

    public virtual void Exit()
    {
        // �˳��߼�

    }

    public virtual void FixedUpdate()
    {

    }

    public virtual void LateUpdate()
    {

    }

    public void ChangeState(PlayerState _newState)
    {
        playerManager.currentState.Exit(); // ת����̬ʱ���õ�ǰ��̬���˳�����
        playerManager.currentState = _newState; // ����ǰ����̬ת��Ϊ�µ���̬
        playerManager.currentState.Enter(); // �����µ���̬�Ľ��뷽��
    }
}