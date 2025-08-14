using UnityEngine;

public class MagicicanManager : PetManager
{

    protected override void Awake()
    {
        base.Awake();

        buffSystem = new BuffSystem(this);
    }

    #region 动画事件
    //普通攻击
    public void NormalAttack()
    {
        //在攻击点生成普通魔法球和攻击释放特效
        PoolManager.Instance.Spawn(PoolManager.Instance.普通魔法球_释放.name, attackPoint.position, transform.rotation);
        PoolManager.Instance.Spawn(PoolManager.Instance.普通魔法球_飞行.name, attackPoint.position, transform.rotation);
    }
    //技能攻击
    public void SkillAttack()
    {
        //在攻击点生成强大魔法球和攻击释放特效

    }
    #endregion
}
