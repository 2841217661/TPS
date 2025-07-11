using UnityEngine;
using RootMotion.FinalIK;
using System.Collections.Generic;

public class PlayerManager : CharacterManager
{
    [Header("射击速度倍率")]
    public float defaultShootSpeedMul;
    public float currentShootSpeedMul;

    [Header("弹容量倍率")]
    public float defaultBulletCountMul;
    public float currentBulletCountMul;
    public int maxBulletCount;
    public int currentBulletCount;

    [Header("测试")]
    public string 当前姿态;
    public List<string> 当前拥有的Buff;

    [HideInInspector] public PlayerInputManager inputManager;
    [HideInInspector] public Animator animator;
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public PlayerCameraManager cameraManager;

    [Header("IK")]
    public AimIK aimIk;
    public Transform aimAimTarget;
    public LayerMask aimIkIgnoreLayer; //当瞄准时ik会将aimIk的目标位置设置为投影到的物体位置，但是有些物体不能被投影,例如子弹

    [Header("Locomotion")]
    public float animationMovementMul = 1f; //RootMotion的移动倍率
    public float rotateSpeed; //转向速度
    public float jumpHeight; // 跳跃高度
    public float airMoveSpeed; //滞空状态下的移动速度
    public float inAirTimer; //下落在空中的时间

    [Header("Throw")]
    public Transform grenadeSpawnPoint;
    public GameObject grenadeTargetPrefab; //瞄准时，在目标终点生成的预制体

    [Header("Aim")]
    public GameObject aimImage; //准星图标

    [Header("Shooter")]
    public Transform shooter;
    public CurrentUseBulletType currentUseBulletType;
    public float shootInterval; //射击间隔
    public float shootTimer;
    public enum CurrentUseBulletType
    {
        BulletNormal,
        BulletTrack,
    }

    private bool isMouseVisible;

    #region 姿态声明
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

        //测试：开始游戏时隐藏鼠标
        //// 游戏开始时隐藏鼠标
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked; // 可选：锁定鼠标到游戏窗口
        //isMouseVisible = false;

        inputManager = GetComponent<PlayerInputManager>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        cameraManager = GetComponent<PlayerCameraManager>();

        #region 姿态实例
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

    //初始化玩家属性值
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

        //持续获取输入
        inputManager.GetAllInput();

        currentState.Update();

        #region 进行调试
        当前姿态 = currentState.ToString();
        //List<string> buffList = new List<string>();
        //for(int i = 0; i < buffSystem.buffs.Count; i++)
        //{
        //    buffList.Add(buffSystem.buffs[i].BuffData.buffName);
        //}
        //当前拥有的Buff = buffList;
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    buffSystem.AddBuff<B_风之祝福>();
        //}
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    buffSystem.RemoveBuff<B_风之祝福>();
        //}
        #endregion

        buffSystem.Update();

        //测试：鼠标隐藏/显示
        // 检测鼠标中键按下
        if (Input.GetMouseButtonDown(2)) // 2代表鼠标中键
        {
            //ToggleMouseVisibility();
        }
    }

    private void ToggleMouseVisibility()
    {
        isMouseVisible = !isMouseVisible;
        Cursor.visible = isMouseVisible;

        // 同步锁定状态
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

    #region 应用重力
    [Header("应用重力")]
    public Vector3 yVelocity; //垂直速度
    public float groundedYVelocity = -2f; //在地面上的垂直速度,使角色贴紧地面
    public float gravity; //重力加速度
    public bool useGravity = true; //是否需要应用重力效果
    public void HandleGravity(bool _user = true)
    {
        if (!_user) return;

        isGrounded = GroundCheck();

        //如果在地面上;有两种情况，第一种：从高出下落到地面，此时的判断条件为isGrounded为true并且yVelocity.y < 0;
        //                         第二种：当从地面跳跃时，刚开始虽然角色离开了地面，但是任然地面检测到了地面
        if (isGrounded && yVelocity.y < 0f) //下落到地面
        {
            yVelocity.y = groundedYVelocity;
            inAirTimer = 0f;
        }
        else //下落中,下落时的重力加速度会大一些，不然下落的时候感觉会轻飘飘的
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

            //限制y的速度，如果太大了会造成相机抖动
            yVelocity.y = Mathf.Clamp(yVelocity.y, -20f, 50f);

            //垂直方向移动
        }

        if (useGravity)
        {
            characterController.Move(yVelocity * Time.deltaTime);
        }
    }
    #endregion

    #region 地面检测
    [Header("地面检测")]
    public bool isGrounded;
    public LayerMask groundLayer; //检测层级
    public float groundCheckSphereRadius; //使用球型检测，检测的半径,与characterControll半径一致最为合适
    public bool GroundCheck()
    {
        Vector3 lowestPoint = new Vector3(
            transform.position.x,
            characterController.bounds.min.y + groundCheckSphereRadius - 0.05f, // 上移一点
            transform.position.z
        );

        return Physics.CheckSphere(lowestPoint, groundCheckSphereRadius, groundLayer);
    }
    #endregion

    #region 攻击范围检测

    [Header("攻击范围检测")]
    public float attackCheckRadius;
    public LayerMask attackEnemyCheckLayer; // 敌人层级
    public LayerMask attackDoorCheckLayer;  // 门层级
    public Vector3 attackOffsetPoint;       // 注意这里是局部偏移点

    public CharacterManager[] EnemyAttackCheck()
    {
        // 将局部偏移转换为世界坐标
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


    #region 场景绘制
    private void OnDrawGizmosSelected()
    {
        #region 绘制地面检测
        // 绘制地面检测范围
        if (characterController != null)
        {
            Gizmos.color = Color.gray;
            Vector3 lowestPoint = new Vector3(
                transform.position.x,
                characterController.bounds.min.y + groundCheckSphereRadius - 0.05f,
                transform.position.z
            );
            Gizmos.DrawSphere(lowestPoint, groundCheckSphereRadius); // 实心球
        }
        #endregion

        #region 绘制攻击范围检测
        Gizmos.color = Color.red;

        // 同样这里也改为局部偏移转换
        Vector3 attackCenter = transform.TransformPoint(attackOffsetPoint);

        Gizmos.DrawWireSphere(attackCenter, attackCheckRadius);
        #endregion
    }
    #endregion

    #region 动画事件




    #endregion
}
