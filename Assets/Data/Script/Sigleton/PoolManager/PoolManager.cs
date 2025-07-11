using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoSingleton<PoolManager>
{
    private Dictionary<string, PrefabsPool> poolDict = new Dictionary<string, PrefabsPool>();

    [Header("-----------------需要使用到对象池的预制体------------------")]
    [Header("子弹及效果")]
    public GameObject bulletNormal; //普通子弹
    public GameObject bulletTrack; //追踪子弹
    public GameObject bulletCase; //弹壳
    public GameObject fx_bulletHitEnemy; //特效_击中敌人
    //特效_击中障碍物
    public GameObject fx_bulletHitObstacle;
    public GameObject fx_bulletHitObstacle_Bark;
    public GameObject fx_bulletHitObstacle_Brick;
    public GameObject fx_bulletHitObstacle_Concrete;
    public GameObject fx_bulletHitObstacle_Dirt;
    public GameObject fx_bulletHitObstacle_Glass;
    public GameObject fx_bulletHitObstacle_Metal;
    public GameObject fx_bulletHitObstacle_Plaster;
    public GameObject fx_bulletHitObstacle_Rock;
    public GameObject fx_bulletHitObstacle_Water;
    public GameObject fx_bulletFire; //特效_开火

    [Header("音效")]
    public GameObject sx_ak47;
    public GameObject sx_shootHit_Body;
    public GameObject sx_shootHit_Brick;
    public GameObject sx_shootHit_Bark;
    public GameObject sx_shootHit_Concrete;
    public GameObject sx_shootHit_Dirt;
    public GameObject sx_shootHit_Foliage;
    public GameObject sx_shootHit_Glass;
    public GameObject sx_shootHit_Metal;
    public GameObject sx_shootHit_Plaster;
    public GameObject sx_shootHit_Rock;
    public GameObject sx_shootHit_Water;

    public GameObject sx_playerFoot_L;
    public GameObject sx_playerFoot_R;

    protected override void Init()
    {
        base.Init();

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        /**创建需要的对象池**/
        #region 子弹及效果
        CreatePool(bulletNormal.name, bulletNormal, 20, transform.Find("BulletPool/Pool_BulletNormal"));
        CreatePool(bulletTrack.name, bulletTrack, 20, transform.Find("BulletPool/Pool_BulletTrack"));
        CreatePool(bulletCase.name, bulletCase, 20, transform.Find("BulletPool/Pool_BulletCase"));
        CreatePool(fx_bulletHitEnemy.name, fx_bulletHitEnemy, 20, transform.Find("BulletPool/Pool_FX_BulletHitEnemy"));
        CreatePool(fx_bulletHitObstacle.name, fx_bulletHitObstacle, 20, transform.Find("BulletPool/Pool_FX_BulletHitObstacle"));
        CreatePool(fx_bulletHitObstacle_Bark.name, fx_bulletHitObstacle_Bark, 20, transform.Find("BulletPool/Pool_FX_BulletHitObstacle_Bark"));
        CreatePool(fx_bulletHitObstacle_Brick.name, fx_bulletHitObstacle_Brick, 20, transform.Find("BulletPool/Pool_FX_BulletHitObstacle_Brick"));
        CreatePool(fx_bulletHitObstacle_Concrete.name, fx_bulletHitObstacle_Concrete, 20, transform.Find("BulletPool/Pool_FX_BulletHitObstacle_Concrete"));
        CreatePool(fx_bulletHitObstacle_Dirt.name, fx_bulletHitObstacle_Dirt, 20, transform.Find("BulletPool/Pool_FX_BulletHitObstacle_Dirt"));
        CreatePool(fx_bulletHitObstacle_Glass.name, fx_bulletHitObstacle_Glass, 20, transform.Find("BulletPool/Pool_FX_BulletHitObstacle_Glass"));
        CreatePool(fx_bulletHitObstacle_Metal.name, fx_bulletHitObstacle_Metal, 20, transform.Find("BulletPool/Pool_FX_BulletHitObstacle_Metal"));
        CreatePool(fx_bulletHitObstacle_Plaster.name, fx_bulletHitObstacle_Plaster, 20, transform.Find("BulletPool/Pool_FX_BulletHitObstacle_Plaster"));
        CreatePool(fx_bulletHitObstacle_Rock.name, fx_bulletHitObstacle_Rock, 20, transform.Find("BulletPool/Pool_FX_BulletHitObstacle_Rock"));
        CreatePool(fx_bulletHitObstacle_Water.name, fx_bulletHitObstacle_Water, 20, transform.Find("BulletPool/Pool_FX_BulletHitObstacle_Water"));
        CreatePool(fx_bulletFire.name, fx_bulletFire, 20, transform.Find("BulletPool/Pool_FX_BulletFire"));
        #endregion

        #region 音效SFX
        //开火音效
        CreatePool(sx_ak47.name, sx_ak47, 20, transform.Find("SFXPool/Pool_SX_Shoot_AK47"));
        //子弹命中物体音效
        CreatePool(sx_shootHit_Body.name, sx_shootHit_Body, 10, transform.Find("SFXPool/Pool_SX_ShootHit_Body"));
        CreatePool(sx_shootHit_Brick.name, sx_shootHit_Brick, 10, transform.Find("SFXPool/Pool_SX_ShootHit_Brick"));
        CreatePool(sx_shootHit_Bark.name, sx_shootHit_Bark, 10, transform.Find("SFXPool/Pool_SX_ShootHit_Bark"));
        CreatePool(sx_shootHit_Concrete.name, sx_shootHit_Concrete, 10, transform.Find("SFXPool/Pool_SX_ShootHit_Concrete"));
        CreatePool(sx_shootHit_Dirt.name, sx_shootHit_Dirt, 20, transform.Find("SFXPool/Pool_SX_ShootHit_Dirt"));
        CreatePool(sx_shootHit_Foliage.name, sx_shootHit_Foliage, 10, transform.Find("SFXPool/Pool_SX_ShootHit_Foliage"));
        CreatePool(sx_shootHit_Glass.name, sx_shootHit_Glass, 10, transform.Find("SFXPool/Pool_SX_ShootHit_Glass"));
        CreatePool(sx_shootHit_Metal.name, sx_shootHit_Metal, 10, transform.Find("SFXPool/Pool_SX_ShootHit_Metal"));
        CreatePool(sx_shootHit_Plaster.name, sx_shootHit_Plaster, 10, transform.Find("SFXPool/Pool_SX_ShootHit_Plaster"));
        CreatePool(sx_shootHit_Rock.name, sx_shootHit_Rock, 10, transform.Find("SFXPool/Pool_SX_ShootHit_Rock"));
        CreatePool(sx_shootHit_Water.name, sx_shootHit_Water, 10, transform.Find("SFXPool/Pool_SX_ShootHit_Water"));
        //脚步音效
        CreatePool(sx_playerFoot_L.name, sx_playerFoot_L, 10, transform.Find("SFXPool/Pool_SX_Player_Foot_L"));
        CreatePool(sx_playerFoot_R.name, sx_playerFoot_R, 10, transform.Find("SFXPool/Pool_SX_Player_Foot_R"));
        #endregion
    }
    public void CreatePool(string key, GameObject prefab, int initialSize, Transform parent = null)
    {
        if (poolDict.ContainsKey(key)) return;

        PrefabsPool pool = new PrefabsPool(prefab, initialSize, parent);
        poolDict[key] = pool;
    }

    public GameObject Spawn(string key, Vector3 position, Quaternion rotation)
    {
        if (!poolDict.ContainsKey(key))
        {
            Debug.LogError($"池不存在: {key}");
            return null;
        }

        return poolDict[key].Spawn(position, rotation);
    }

    public void Recycle(string key, GameObject obj)
    {
        if (!poolDict.ContainsKey(key))
        {
            Debug.LogError($"池不存在: {key}");
            return;
        }

        poolDict[key].Recycle(obj);
    }
}
