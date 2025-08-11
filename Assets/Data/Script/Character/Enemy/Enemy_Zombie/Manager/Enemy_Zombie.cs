using UnityEngine;
using UnityEngine.AI;

public class Enemy_Zombie : Enemy_Ordinary
{
    public override void TakeDamage(float _value, CharacterManager _source, Vector3 _damagePosition, DamageIntensity _type, DamageElement _element)
    {
        if (state == EnemyState.Death) return;

        if (state == EnemyState.Patrol)
        {
            //此时应该发现玩家
            target = GameManager.Instance.playerManager.transform;
            //播放警戒语音
            PoolManager.Instance.Spawn(PoolManager.Instance.sx_僵尸_警戒.name, transform.position, Quaternion.identity, true);
        }

        //判断是否发生暴击
        if (Random.Range(0, 1f) < GameManager.Instance.playerManager.currentCrticalMul)
        {
            DamageEvent(_value * 2f, _damagePosition, _element, true);
        }
        else
        {
            DamageEvent(_value, _damagePosition, _element, false);
        }

        switch (_type)
        {
            case DamageIntensity.Light: //持续受到轻击会被击退
                currentBackResistance -= _value;
                if (currentBackResistance <= 0f)
                {
                    state = EnemyState.KnockBack;
                    currentBackResistance = backResistance;
                }
                break;
            case DamageIntensity.Middle: //受到较重的攻击会被击退
                state = EnemyState.KnockBack;
                currentBackResistance = backResistance;
                break;
            case DamageIntensity.Heavy: //受到重击会被击飞
                state = EnemyState.KnockUp;
                currentBackResistance = backResistance;
                break;
        }

        currentHealthValue -= _value;
        if (currentHealthValue <= 0f)
        {
            state = EnemyState.Death;
        }
    }

    public override void ApplyExplosionForce(Vector3 explosionPosition, float force, float radius, float upwardModifier = 0.5F)
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.AddExplosionForce(force, explosionPosition, radius, upwardModifier, ForceMode.Impulse);
        }
    }

    public override void OnDeath()
    {
        base.OnDeath();

        Destroy(gameObject);

        PoolManager.Instance.Spawn(PoolManager.Instance.zombie_death_explode.name, transform.position + Vector3.up, transform.rotation);
        PoolManager.Instance.Spawn(PoolManager.Instance.sx_僵尸_死亡爆炸.name, transform.position, transform.rotation);
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        #region 绘制普通攻击范围
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + transform.rotation * normalAttackoffset, normalAttackRadius);
        #endregion
    }

    #region 自己的动画事件
    public void Play_SX_攻击()
    {
        PoolManager.Instance.Spawn(PoolManager.Instance.sx_僵尸_攻击.name, transform.position, Quaternion.identity, true);
    }

    public void Play_SX_KnockBack()
    {
        PoolManager.Instance.Spawn(PoolManager.Instance.sx_僵尸_被击退.name, transform.position, Quaternion.identity, true);
    }

    [Header("普通攻击设置")]
    public float normalAttackRadius; //普通攻击范围半径
    public Vector3 normalAttackoffset; //以原点 + offset 为中心
    public void TryNormalAttack()
    {
        Collider[] collisions = Physics.OverlapSphere(transform.position + transform.rotation * normalAttackoffset, normalAttackRadius, targetLayer);
        if (collisions.Length > 0) //敌人在范围内
        {
            collisions[0].GetComponent<IDamageable>()?.TakeDamage(currentAttackValue, this, Vector3.zero, DamageIntensity.Light, DamageElement.Physical);
        }
    }

    #endregion
}
