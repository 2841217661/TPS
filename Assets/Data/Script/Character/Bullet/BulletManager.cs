using UnityEngine;
public enum BulletState
{
    normal,
    track,
}
public class BulletManager : CharacterManager,IPoolable
{
    [Header("基本属性")]
    public float speed;
    public float damage;
    public float lifeTime;
    private float lifeTimer; //存活计时器
    public LayerMask ignoreCollisionLayer; //忽略对该层级碰撞(如player)
    public LayerMask obstacleLayer; //障碍物层级，当碰撞到障碍物时会在障碍物上实例特效,否则就实例子弹爆炸的特效
    private Vector3 previousPosition; //上一帧的位置
    private Vector3 currentPosition; //当前帧的位置

    public virtual void OnRecycle()
    {
        lifeTimer = 0f;
    }

    public virtual void OnSpawn()
    {
        previousPosition = transform.position;

        Debug.Log("实例一次子弹");
    }


    protected virtual void FixedUpdate()
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
        if(Physics.Raycast(previousPosition,currentPosition - previousPosition, out hit, Vector3.Distance(previousPosition,currentPosition),~ignoreCollisionLayer))
        {
            //Debug.Log("碰到了: " + hit.collider.gameObject.name);

            //调用被击中者的受击接口方法
            var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
            damageable?.TakeDamage(damage);

            PoolManager.Instance.Recycle(gameObject.name, gameObject); //回收子弹
                                                                       //实例特效
            //GenerateHitFX(hit.collider.gameObject.layer);

            // 1. 命中点
            Vector3 targetPoint = hit.point;

            // 2. 命中法线
            Vector3 normal = hit.normal;

            // 3. 根据障碍物的Tag生成特定特效
            GameObject _fx = GenerateHitObstacleEffect(hit.collider.gameObject, targetPoint);

            // 4. 让特效朝向命中表面
            _fx.transform.rotation = Quaternion.LookRotation(normal);
        }

    } 

    private GameObject GenerateHitObstacleEffect(GameObject _target, Vector3 _spawnPoint)
    {
        string _targetFXName = "";
        string _targetSXName = "";
        switch (_target.tag)
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
            default:
                _targetFXName = PoolManager.Instance.fx_bulletHitObstacle.name;
                _targetSXName = PoolManager.Instance.sx_shootHit_Body.name;
                break;
        }

        GameObject fx = PoolManager.Instance.Spawn(_targetFXName, _spawnPoint, Quaternion.identity);
        GameObject sx = PoolManager.Instance.Spawn(_targetSXName, _spawnPoint, Quaternion.identity);
        return fx;
    }
}
