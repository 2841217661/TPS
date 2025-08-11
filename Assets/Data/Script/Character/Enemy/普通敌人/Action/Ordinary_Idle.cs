using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Ordinary_Idle : Action
{
    public SharedEnemyManager self;

    [SerializeField] private string[] animationName;
    [SerializeField] private float[] animationWeights; // 与 animationName 一一对应
    [SerializeField] private float minIdleTime = 1f;
    [SerializeField] private float maxIdleTime = 3f;

    private string currentAnimationName;
    private float currentIdleTime;
    private float idleTimer;

    public override void OnStart()
    {
        // 权重数组长度检查
        if (animationWeights.Length != animationName.Length)
        {
            Debug.LogError("权重数组和动画数组长度不一致！");
            return;
        }

        // 随机持续时间
        currentIdleTime = Random.Range(minIdleTime, maxIdleTime);
        idleTimer = 0f;

        // 获取加权随机动画
        currentAnimationName = GetWeightedRandomAnimation();

        // 播放动画
        self.Value.animator.CrossFade(currentAnimationName, 0.1f);
    }

    public override TaskStatus OnUpdate()
    {
        AnimatorStateInfo stateInfo = self.Value.animator.GetCurrentAnimatorStateInfo(0);

        // 如果是普通 Idle 动画，需要等待一段时间
        if (currentAnimationName == "Idle")
        {
            if (idleTimer > currentIdleTime)
            {
                return TaskStatus.Success;
            }
            idleTimer += Time.deltaTime;
            return TaskStatus.Running;
        }
        else
        {
            // 如果当前动画播放完成（非 Idle 动画）
            if (stateInfo.IsName(currentAnimationName) && stateInfo.normalizedTime >= 1f)
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }
    }

    /// <summary>
    /// 根据权重随机返回一个动画名
    /// </summary>
    private string GetWeightedRandomAnimation()
    {
        float totalWeight = 0f;
        for (int i = 0; i < animationWeights.Length; i++)
        {
            totalWeight += animationWeights[i];
        }

        float randomValue = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        for (int i = 0; i < animationName.Length; i++)
        {
            cumulative += animationWeights[i];
            if (randomValue <= cumulative)
            {
                return animationName[i];
            }
        }

        // 防止浮点数问题
        return animationName[animationName.Length - 1];
    }
}
