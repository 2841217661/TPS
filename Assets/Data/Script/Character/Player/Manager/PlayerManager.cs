using UnityEngine;
using RootMotion.FinalIK;
using System.Collections.Generic;

public class PlayerManager : CharacterManager
{
    [Header("����ٶȱ���")]
    public float defaultShootSpeedMul;
    public float currentShootSpeedMul;

    [Header("����������")]
    public float defaultBulletCountMul;
    public float currentBulletCountMul;
    public int maxBulletCount;
    public int currentBulletCount;

    [Header("����")]
    public string ��ǰ��̬;
    public List<string> ��ǰӵ�е�Buff;

    [HideInInspector] public PlayerInputManager inputManager;
    [HideInInspector] public Animator animator;
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public PlayerCameraManager cameraManager;

    [Header("IK")]
    public AimIK aimIk;
    public Transform aimAimTarget;
    public LayerMask aimIkIgnoreLayer; //����׼ʱik�ὫaimIk��Ŀ��λ������ΪͶӰ��������λ�ã�������Щ���岻�ܱ�ͶӰ,�����ӵ�

    [Header("Locomotion")]
    public float animationMovementMul = 1f; //RootMotion���ƶ�����
    public float rotateSpeed; //ת���ٶ�
    public float jumpHeight; // ��Ծ�߶�
    public float airMoveSpeed; //�Ϳ�״̬�µ��ƶ��ٶ�
    public float inAirTimer; //�����ڿ��е�ʱ��

    [Header("Throw")]
    public Transform grenadeSpawnPoint;
    public GameObject grenadeTargetPrefab; //��׼ʱ����Ŀ���յ����ɵ�Ԥ����

    [Header("Aim")]
    public GameObject aimImage; //׼��ͼ��

    [Header("Shooter")]
    public Transform shooter;
    public CurrentUseBulletType currentUseBulletType;
    public float shootInterval; //������
    public float shootTimer;
    public enum CurrentUseBulletType
    {
        BulletNormal,
        BulletTrack,
    }

    private bool isMouseVisible;

    #region ��̬����
    public PlayerState currentState;
    public PlayerIdleState idleState { get; private set; }
    public PlayerWalkState walkState { get; private set; }
    public PlayerWalkEndState walkEndState { get; private set; }
    public PlayerRunState runState { get; private set; }
    public PlayerRunEndState runEndState { get; private set; }
    public PlayerAimState aimState { get; private set; }
    public PlayerLiftState liftState { get; private set; }
    public PlayerFallState fallState { get; private set; }
    public PlayerLandState landState { get; private set; }
    public PlayerThrowStartState throwStartState { get; private set; }
    public PlayerThrowLoopState throwLoopState { get; private set; }
    public PlayerThrowEndState throwEndState { get; private set; }
    public PlayerElbowStrikeState elbowStrikeState { get; private set; }
    public PlayerKickState kickState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();

        //���ԣ���ʼ��Ϸʱ�������
        //// ��Ϸ��ʼʱ�������
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked; // ��ѡ��������굽��Ϸ����
        //isMouseVisible = false;

        inputManager = GetComponent<PlayerInputManager>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        cameraManager = GetComponent<PlayerCameraManager>();

        #region ��̬ʵ��
        idleState = new PlayerIdleState(this, PlayerAnimationName.Idle, true);
        walkState = new PlayerWalkState(this, PlayerAnimationName.Walk, true);
        walkEndState = new PlayerWalkEndState(this, PlayerAnimationName.WalkEnd, true);
        runState = new PlayerRunState(this, PlayerAnimationName.Run, true);
        runEndState = new PlayerRunEndState(this, PlayerAnimationName.RunEnd, true);
        aimState = new PlayerAimState(this, PlayerAnimationName.Aim, true);
        liftState = new PlayerLiftState(this, PlayerAnimationName.Lift, false);
        fallState = new PlayerFallState(this, PlayerAnimationName.Fall, false);
        landState = new PlayerLandState(this, PlayerAnimationName.Land, false);
        throwStartState = new PlayerThrowStartState(this, PlayerAnimationName.ThrowStart, true);
        throwLoopState = new PlayerThrowLoopState(this, PlayerAnimationName.ThrowLoop, true);
        throwEndState = new PlayerThrowEndState(this, PlayerAnimationName.ThrowEnd, true);
        elbowStrikeState = new PlayerElbowStrikeState(this, PlayerAnimationName.ElbowStrike, true);
        kickState = new PlayerKickState(this, PlayerAnimationName.Kick, true);
        #endregion
    }



    protected override void Start()
    {
        base.Start();

        InitPlayerAttributeValue();


        currentState = idleState;
        currentState.Enter();

    }

    //��ʼ���������ֵ
    private void InitPlayerAttributeValue()
    {
        currentBulletCount = maxBulletCount;
        currentHealthValue = maxHealthValue;
    }

    private void OnAnimatorMove()
    {
        if (currentState.useRootMotion)
        {
            Vector3 delta = animator.deltaPosition * animationMovementMul;
            characterController.Move(delta);
        }
    }



    protected override void Update()
    {
        base.Update();

        //������ȡ����
        inputManager.GetAllInput();

        currentState.Update();

        #region ���е���
        ��ǰ��̬ = currentState.ToString();
        //List<string> buffList = new List<string>();
        //for(int i = 0; i < buffSystem.buffs.Count; i++)
        //{
        //    buffList.Add(buffSystem.buffs[i].BuffData.buffName);
        //}
        //��ǰӵ�е�Buff = buffList;
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    buffSystem.AddBuff<B_��֮ף��>();
        //}
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    buffSystem.RemoveBuff<B_��֮ף��>();
        //}
        #endregion

        buffSystem.Update();

        //���ԣ��������/��ʾ
        // �������м�����
        if (Input.GetMouseButtonDown(2)) // 2��������м�
        {
            //ToggleMouseVisibility();
        }
    }

    private void ToggleMouseVisibility()
    {
        isMouseVisible = !isMouseVisible;
        Cursor.visible = isMouseVisible;

        // ͬ������״̬
        Cursor.lockState = isMouseVisible ? CursorLockMode.None : CursorLockMode.Locked;

    }

    private void FixedUpdate()
    {

        currentState.FixedUpdate();

        buffSystem.FixedUpdate();
    }
    [HideInInspector] public bool disableCameraRotate;
    private void LateUpdate()
    {
        currentState.LateUpdate();

        if (!disableCameraRotate)
        {
            cameraManager.SetCameraRotation();
        }
    }

    #region Ӧ������
    [Header("Ӧ������")]
    public Vector3 yVelocity; //��ֱ�ٶ�
    public float groundedYVelocity = -2f; //�ڵ����ϵĴ�ֱ�ٶ�,ʹ��ɫ��������
    public float gravity; //�������ٶ�
    public bool useGravity = true; //�Ƿ���ҪӦ������Ч��
    public void HandleGravity(bool _user = true)
    {
        if (!_user) return;

        isGrounded = GroundCheck();

        //����ڵ�����;�������������һ�֣��Ӹ߳����䵽���棬��ʱ���ж�����ΪisGroundedΪtrue����yVelocity.y < 0;
        //                         �ڶ��֣����ӵ�����Ծʱ���տ�ʼ��Ȼ��ɫ�뿪�˵��棬������Ȼ�����⵽�˵���
        if (isGrounded && yVelocity.y < 0f) //���䵽����
        {
            yVelocity.y = groundedYVelocity;
            inAirTimer = 0f;
        }
        else //������,����ʱ���������ٶȻ��һЩ����Ȼ�����ʱ��о�����ƮƮ��
        {
            if (yVelocity.y < 0f)
            {
                yVelocity.y += gravity * 1.5f * Time.deltaTime;
                //yVelocity.y += gravity * Time.deltaTime;
            }
            else
            {
                yVelocity.y += gravity * Time.deltaTime;
            }

            inAirTimer += Time.deltaTime;

            //����y���ٶȣ����̫���˻�����������
            yVelocity.y = Mathf.Clamp(yVelocity.y, -20f, 50f);

            //��ֱ�����ƶ�
        }

        if (useGravity)
        {
            characterController.Move(yVelocity * Time.deltaTime);
        }
    }
    #endregion

    #region ������
    [Header("������")]
    public bool isGrounded;
    public LayerMask groundLayer; //���㼶
    public float groundCheckSphereRadius; //ʹ�����ͼ�⣬���İ뾶,��characterControll�뾶һ����Ϊ����
    public bool GroundCheck()
    {
        Vector3 lowestPoint = new Vector3(
            transform.position.x,
            characterController.bounds.min.y + groundCheckSphereRadius - 0.05f, // ����һ��
            transform.position.z
        );

        return Physics.CheckSphere(lowestPoint, groundCheckSphereRadius, groundLayer);
    }
    #endregion

    #region ������Χ���

    [Header("������Χ���")]
    public float attackCheckRadius;
    public LayerMask attackEnemyCheckLayer; // ���˲㼶
    public LayerMask attackDoorCheckLayer;  // �Ų㼶
    public Vector3 attackOffsetPoint;       // ע�������Ǿֲ�ƫ�Ƶ�

    public CharacterManager[] EnemyAttackCheck()
    {
        // ���ֲ�ƫ��ת��Ϊ��������
        Vector3 attackCenter = transform.TransformPoint(attackOffsetPoint);

        Collider[] colliders = Physics.OverlapSphere(attackCenter, attackCheckRadius, attackEnemyCheckLayer);
        CharacterManager[] managers = new CharacterManager[colliders.Length];

        for (int i = 0; i < colliders.Length; i++)
        {
            managers[i] = colliders[i].GetComponent<CharacterManager>();
        }

        return managers;
    }

    public CharacterManager[] DoorAttackCheck()
    {
        Vector3 attackCenter = transform.TransformPoint(attackOffsetPoint);

        Collider[] colliders = Physics.OverlapSphere(attackCenter, attackCheckRadius, attackDoorCheckLayer);
        CharacterManager[] managers = new CharacterManager[colliders.Length];

        for (int i = 0; i < colliders.Length; i++)
        {
            managers[i] = colliders[i].GetComponent<CharacterManager>();
        }

        return managers;
    }

    #endregion


    #region ��������
    private void OnDrawGizmosSelected()
    {
        #region ���Ƶ�����
        // ���Ƶ����ⷶΧ
        if (characterController != null)
        {
            Gizmos.color = Color.gray;
            Vector3 lowestPoint = new Vector3(
                transform.position.x,
                characterController.bounds.min.y + groundCheckSphereRadius - 0.05f,
                transform.position.z
            );
            Gizmos.DrawSphere(lowestPoint, groundCheckSphereRadius); // ʵ����
        }
        #endregion

        #region ���ƹ�����Χ���
        Gizmos.color = Color.red;

        // ͬ������Ҳ��Ϊ�ֲ�ƫ��ת��
        Vector3 attackCenter = transform.TransformPoint(attackOffsetPoint);

        Gizmos.DrawWireSphere(attackCenter, attackCheckRadius);
        #endregion
    }
    #endregion

    #region �����¼�




    #endregion
}
