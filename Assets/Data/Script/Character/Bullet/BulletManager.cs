using UnityEngine;
public enum BulletState
{
    normal,
    track,
}
public class BulletManager : CharacterManager,IPoolable
{
    [Header("��������")]
    public float speed;
    public float damage;
    public float lifeTime;
    private float lifeTimer; //����ʱ��
    public LayerMask ignoreCollisionLayer; //���ԶԸò㼶��ײ(��player)
    public LayerMask obstacleLayer; //�ϰ���㼶������ײ���ϰ���ʱ�����ϰ�����ʵ����Ч,�����ʵ���ӵ���ը����Ч
    private Vector3 previousPosition; //��һ֡��λ��
    private Vector3 currentPosition; //��ǰ֡��λ��

    public virtual void OnRecycle()
    {
        lifeTimer = 0f;
    }

    public virtual void OnSpawn()
    {
        previousPosition = transform.position;

        Debug.Log("ʵ��һ���ӵ�");
    }


    protected virtual void FixedUpdate()
    {
        lifeTimer += Time.fixedDeltaTime;
        if (lifeTimer >= lifeTime)
        {
            PoolManager.Instance.Recycle(gameObject.name, gameObject); //�����ӵ�
            return;
        }

        CollisionDetected();
    }

    //Ϊ�˷�ֹ���ٴ�ģ������һ֡��λ����ǰ֡��λ�÷���һ�����ߣ���һ������⵽�ľ���Ŀ������
    protected virtual void CollisionDetected()
    {
        currentPosition = transform.position + transform.forward * speed * Time.fixedDeltaTime;
        transform.position = currentPosition;

        RaycastHit hit;
        if(Physics.Raycast(previousPosition,currentPosition - previousPosition, out hit, Vector3.Distance(previousPosition,currentPosition),~ignoreCollisionLayer))
        {
            //Debug.Log("������: " + hit.collider.gameObject.name);

            //���ñ������ߵ��ܻ��ӿڷ���
            var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
            damageable?.TakeDamage(damage);

            PoolManager.Instance.Recycle(gameObject.name, gameObject); //�����ӵ�
                                                                       //ʵ����Ч
            //GenerateHitFX(hit.collider.gameObject.layer);

            // 1. ���е�
            Vector3 targetPoint = hit.point;

            // 2. ���з���
            Vector3 normal = hit.normal;

            // 3. �����ϰ����Tag�����ض���Ч
            GameObject _fx = GenerateHitObstacleEffect(hit.collider.gameObject, targetPoint);

            // 4. ����Ч�������б���
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
