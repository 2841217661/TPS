using UnityEngine;
public enum BulletType
{
    normal,
    track,
}
public enum BulletMotionState
{
    normal,
    track,
}
public class BulletManager : CharacterManager,IPoolable
{
    [Header("基本属性")]
    public BulletType bulletType; //子弹类型
    public float speed;
    public float damage;
    public float lifeTime;
    private float lifeTimer; //存活计时器
    private Vector3 previousPosition; //上一帧的位置
    private Vector3 currentPosition; //当前帧的位置

    public virtual void OnRecycle()
    {
        lifeTimer = 0f;
    }

    public virtual void OnSpawn()
    {
        previousPosition = transform.position;

        //执行子弹生成的事件 
        EventManager.Instance.bulletEvent.BulletSpawn(bulletType);
    }


    protected override void FixedUpdate()
    {
        lifeTimer += Time.fixedDeltaTime;
        if (lifeTimer >= lifeTime)
        {
            PoolManager.Instance.Recycle(gameObject.name, gameObject); //回收子弹
            return;
        }

        CollisionDetected();
    }

    //为了防止高速穿模：从上一帧的位置向当前帧的位置发射一条射线，第一个被检测到的就是目标物体
    protected virtual void CollisionDetected()
    {
        currentPosition = transform.position + transform.forward * speed * Time.fixedDeltaTime;
        transform.position = currentPosition;

        RaycastHit hit;
        if(Physics.Raycast(previousPosition,currentPosition - previousPosition, out hit, Vector3.Distance(previousPosition,currentPosition),~GameManager.Instance.playerManager.notDamageLayer, QueryTriggerInteraction.Ignore))
        {
            //Debug.Log("碰到了: " + hit.collider.gameObject.name);

            //触发子弹命中事件回调
            EventManager.Instance.bulletEvent.BulletHitObject(bulletType,hit.transform.GetComponent<CharacterManager>());

            //调用被击中者的受击接口方法
            var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
            damageable?.TakeDamage(damage,this, TakeDamageType.Light);

            PoolManager.Instance.Recycle(gameObject.name, gameObject); //回收子弹
                                                                       //实例特效
            //GenerateHitFX(hit.collider.gameObject.layer);

            // 1. 命中点
            Vector3 targetPoint = hit.point;

            // 2. 命中法线
            Vector3 normal = hit.normal;

            // 3. 根据障碍物的Tag生成特定特效和音效
            (GameObject _fx, GameObject _) = GenerateHitVFXAndSFX(hit.collider.gameObject, targetPoint);

            // 4. 让特效朝向命中表面
            _fx.transform.rotation = Quaternion.LookRotation(normal);
        }
    } 


    //所有子弹命中障碍物时的音效和特效都是一样的，所以只需要区别不同子弹命中敌人和其他目标时情况
    private (string, string) GetHitVFXAndSFXName(string _hitTag)
    {
        string _targetFXName = "";
        string _targetSXName = "";
        switch (_hitTag)
        {
            case "Bark":
                _targetFXName = PoolManager.Instance.fx_bulletHitObstacle_Bark.name;
                _targetSXName = PoolManager.Instance.sx_shootHit_Bark.name;
                break;
            case "Brick":
                _targetFXName = PoolManager.Instance.fx_bulletHitObstacle_Brick.name;
                _targetSXName = PoolManager.Instance.sx_shootHit_Brick.name;
                break;
            case "Concrete":
                _targetFXName = PoolManager.Instance.fx_bulletHitObstacle_Concrete.name;
                _targetSXName = PoolManager.Instance.sx_shootHit_Concrete.name;
                break;
            case "Dirt":
                _targetFXName = PoolManager.Instance.fx_bulletHitObstacle_Dirt.name;
                _targetSXName = PoolManager.Instance.sx_shootHit_Dirt.name;
                break;
            case "Glass":
                _targetFXName = PoolManager.Instance.fx_bulletHitObstacle_Glass.name;
                _targetSXName = PoolManager.Instance.sx_shootHit_Glass.name;
                break;
            case "Metal":
                _targetFXName = PoolManager.Instance.fx_bulletHitObstacle_Metal.name;
                _targetSXName = PoolManager.Instance.sx_shootHit_Metal.name;
                break;
            case "Plaster":
                _targetFXName = PoolManager.Instance.fx_bulletHitObstacle_Plaster.name;
                _targetSXName = PoolManager.Instance.sx_shootHit_Plaster.name;
                break;
            case "Rock":
                _targetFXName = PoolManager.Instance.fx_bulletHitObstacle_Rock.name;
                _targetSXName = PoolManager.Instance.sx_shootHit_Rock.name;
                break;
            case "Water":
                _targetFXName = PoolManager.Instance.fx_bulletHitObstacle_Water.name;
                _targetSXName = PoolManager.Instance.sx_shootHit_Water.name;
                break;
            case "Enemy":
                (_targetFXName, _targetSXName) = GetFXAndSXByOtherHitObject(this);
                break;
            default: //碰到特定障碍物以外的物体
                if(bulletType == BulletType.normal)
                {
                    NormalBulletManager normalBulletManager = this as NormalBulletManager;
                    if(normalBulletManager.normalBulletType == NormalBulletType.flame)
                    {
                        _targetFXName = PoolManager.Instance.fx_bulletHitEnemy_Normal_Flame.name;
                    }
                    else if(normalBulletManager.normalBulletType == NormalBulletType.ordinary)
                    {
                        _targetFXName = PoolManager.Instance.fx_bulletHitObstacle.name;
                    }
                }
                else //追踪子弹
                {

                    //追踪子弹打在墙上和敌人身上的效果是一样的
                    _targetFXName = PoolManager.Instance.fx_bulletHitEnemy_Track_Ordinary.name;
                }
                break;
        }

        return (_targetFXName, _targetSXName);
    }

    private (string _targetFXName, string _targetSXName) GetFXAndSXByOtherHitObject(BulletManager manager)
    {

        string targetFXName = "", targetSXName = "";
        switch(manager.bulletType)
        {
            case BulletType.track:
                TrackBulletManager trackBulletManager = manager as TrackBulletManager;
                switch (trackBulletManager.trackBulletType)
                {
                    case TrackBulletType.ordinary:
                        targetFXName = PoolManager.Instance.fx_bulletHitEnemy_Track_Ordinary.name;
                        break;
                    default:
                        Debug.LogError("子弹类型错误");
                        break;
                }
                break;
            case BulletType.normal:
                NormalBulletManager normalBulletManager = manager as NormalBulletManager;
                switch (normalBulletManager.normalBulletType)
                {
                    case NormalBulletType.ordinary:
                        targetFXName = PoolManager.Instance.fx_bulletHitEnemy_Normal_Ordinary.name;
                        break;
                    case NormalBulletType.flame:
                        targetFXName = PoolManager.Instance.fx_bulletHitEnemy_Normal_Flame.name;
                        break;
                    default:
                        Debug.LogError("子弹类型错误");
                        break;
                }
                break;
        }
        return (targetFXName, targetSXName);
    }

    private (GameObject,GameObject) GenerateHitVFXAndSFX(GameObject _target, Vector3 _spawnPoint)
    {
        (string _targetFXName,string _targetSXName) = GetHitVFXAndSFXName(_target.tag);

        GameObject fx = PoolManager.Instance.Spawn(_targetFXName, _spawnPoint, Quaternion.identity);

        GameObject sx = null;
        if (_targetSXName != "")
        {
           sx = PoolManager.Instance.Spawn(_targetSXName, _spawnPoint, Quaternion.identity);
        }

        return (fx,sx);
    }
}
