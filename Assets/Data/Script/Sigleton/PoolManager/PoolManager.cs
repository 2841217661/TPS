using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoSingleton<PoolManager>
{
    private Dictionary<string, PrefabsPool> poolDict = new Dictionary<string, PrefabsPool>();

    [Header("-----------------需要使用到对象池的预制体------------------")]
    [Header("子弹及效果")]
    public GameObject bulletNormal_Ordinary; //普通_普通子弹
    public GameObject bulletNormal_Flame; //普通_火焰子弹
    public GameObject bulletTrack_Ordinary; //追踪_普通子弹
    //public GameObject bulletCase; //弹壳
    public GameObject fx_bulletHitEnemy_Normal_Ordinary; //特效_击中敌人(或者其他默认物体):普通_普通子弹
    public GameObject fx_bulletHitEnemy_Normal_Flame; //特效_击中敌人(或者其他默认物体):普通_火焰子弹
    public GameObject fx_bulletHitEnemy_Track_Ordinary;  //特效_击中敌人(或者其他默认物体):追踪_普通子弹
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
    public GameObject fx_bulletNormal_Ordinary_Fire; //特效_开火:普通_普通子弹
    public GameObject fx_bulletNormal_Flame_Fire; //特效_开火:普通_火焰子弹

    [Header("音效")]
    public GameObject sx_ak47;
    public GameObject sx_ak47_normal_flame;
    //public GameObject sx_shootHit_Body;
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
    //脚步声
    public GameObject sx_playerFoot_L;
    public GameObject sx_playerFoot_R;
    //Buff
    public GameObject sx_buff_灼烧;

    [Header("特效")]
    public GameObject 灼烧;
    public GameObject 淘汰回放爆炸;
    public GameObject 淘汰回放爆炸球;
    public GameObject 火焰印记爆炸;


    protected override void Init()
    {
        base.Init();

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        /**创建需要的对象池**/
        #region 子弹及效果
        //子弹形状
        CreatePool(bulletNormal_Ordinary.name, bulletNormal_Ordinary, 10, transform.Find("BulletPool/Pool_Bullet/Pool_BulletNormal/Pool_BulletNormal_Ordinary"));
        CreatePool(bulletNormal_Flame.name, bulletNormal_Flame, 10, transform.Find("BulletPool/Pool_Bullet/Pool_BulletNormal/Pool_BulletNormal_Flame"));
        CreatePool(bulletTrack_Ordinary.name, bulletTrack_Ordinary, 10, transform.Find("BulletPool/Pool_Bullet/Pool_BulletTrack/Pool_BulletTrack_Ordinary"));
        //CreatePool(bulletCase.name, bulletCase, 10, transform.Find("BulletPool/Pool_BulletCase"));
        //子弹命中敌人
        CreatePool(fx_bulletHitEnemy_Normal_Ordinary.name, fx_bulletHitEnemy_Normal_Ordinary, 10, transform.Find("BulletPool/Pool_BulletHitEnemy/Pool_FX_NormalBullet_Ordinary_HitEnemy"));
        CreatePool(fx_bulletHitEnemy_Normal_Flame.name, fx_bulletHitEnemy_Normal_Flame, 10, transform.Find("BulletPool/Pool_BulletHitEnemy/Pool_FX_NormalBullet_Flame_HitEnemy"));
        CreatePool(fx_bulletHitEnemy_Track_Ordinary.name, fx_bulletHitEnemy_Track_Ordinary, 10, transform.Find("BulletPool/Pool_BulletHitEnemy/Pool_FX_TrackBullet_Ordinary_HitEnemy"));
        //碰到障碍物
        CreatePool(fx_bulletHitObstacle.name, fx_bulletHitObstacle, 10, transform.Find("BulletPool/Pool_BulletHitObstacle/Pool_FX_BulletHitObstacle")); //默认障碍物
        CreatePool(fx_bulletHitObstacle_Bark.name, fx_bulletHitObstacle_Bark, 10, transform.Find("BulletPool/Pool_BulletHitObstacle/Pool_FX_BulletHitObstacle_Bark"));
        CreatePool(fx_bulletHitObstacle_Brick.name, fx_bulletHitObstacle_Brick, 10, transform.Find("BulletPool/Pool_BulletHitObstacle/Pool_FX_BulletHitObstacle_Brick"));
        CreatePool(fx_bulletHitObstacle_Concrete.name, fx_bulletHitObstacle_Concrete, 10, transform.Find("BulletPool/Pool_BulletHitObstacle/Pool_FX_BulletHitObstacle_Concrete"));
        CreatePool(fx_bulletHitObstacle_Dirt.name, fx_bulletHitObstacle_Dirt, 10, transform.Find("BulletPool/Pool_BulletHitObstacle/Pool_FX_BulletHitObstacle_Dirt"));
        CreatePool(fx_bulletHitObstacle_Glass.name, fx_bulletHitObstacle_Glass, 10, transform.Find("BulletPool/Pool_BulletHitObstacle/Pool_FX_BulletHitObstacle_Glass"));
        CreatePool(fx_bulletHitObstacle_Metal.name, fx_bulletHitObstacle_Metal, 10, transform.Find("BulletPool/Pool_BulletHitObstacle/Pool_FX_BulletHitObstacle_Metal"));
        CreatePool(fx_bulletHitObstacle_Plaster.name, fx_bulletHitObstacle_Plaster, 10, transform.Find("BulletPool/Pool_BulletHitObstacle/Pool_FX_BulletHitObstacle_Plaster"));
        CreatePool(fx_bulletHitObstacle_Rock.name, fx_bulletHitObstacle_Rock, 10, transform.Find("BulletPool/Pool_BulletHitObstacle/Pool_FX_BulletHitObstacle_Rock"));
        CreatePool(fx_bulletHitObstacle_Water.name, fx_bulletHitObstacle_Water, 10, transform.Find("BulletPool/Pool_BulletHitObstacle/Pool_FX_BulletHitObstacle_Water"));
        //开火
        CreatePool(fx_bulletNormal_Ordinary_Fire.name, fx_bulletNormal_Ordinary_Fire, 20, transform.Find("BulletPool/Pool_BulletFire/Pool_FX_BulletNormal_Ordinary_Fire"));
        CreatePool(fx_bulletNormal_Flame_Fire.name, fx_bulletNormal_Flame_Fire, 20, transform.Find("BulletPool/Pool_BulletFire/Pool_FX_BulletNormal_Flame_Fire"));
        #endregion

        #region 音效SFX
        //开火音效
        CreatePool(sx_ak47.name, sx_ak47, 20, transform.Find("SFXPool/Pool_SX_Shoot_AK47"));
        CreatePool(sx_ak47_normal_flame.name, sx_ak47_normal_flame, 20, transform.Find("SFXPool/Pool_SX_Shoot_Normal_Flame"));
        //子弹命中物体音效
        //CreatePool(sx_shootHit_Body.name, sx_shootHit_Body, 10, transform.Find("SFXPool/Pool_SX_ShootHit_Body"));
        CreatePool(sx_shootHit_Brick.name, sx_shootHit_Brick, 10, transform.Find("SFXPool/Pool_SX_ShootHit_Brick"));
        CreatePool(sx_shootHit_Bark.name, sx_shootHit_Bark, 10, transform.Find("SFXPool/Pool_SX_ShootHit_Bark"));
        CreatePool(sx_shootHit_Concrete.name, sx_shootHit_Concrete, 10, transform.Find("SFXPool/Pool_SX_ShootHit_Concrete"));
        CreatePool(sx_shootHit_Dirt.name, sx_shootHit_Dirt, 10, transform.Find("SFXPool/Pool_SX_ShootHit_Dirt"));
        CreatePool(sx_shootHit_Foliage.name, sx_shootHit_Foliage, 10, transform.Find("SFXPool/Pool_SX_ShootHit_Foliage"));
        CreatePool(sx_shootHit_Glass.name, sx_shootHit_Glass, 10, transform.Find("SFXPool/Pool_SX_ShootHit_Glass"));
        CreatePool(sx_shootHit_Metal.name, sx_shootHit_Metal, 10, transform.Find("SFXPool/Pool_SX_ShootHit_Metal"));
        CreatePool(sx_shootHit_Plaster.name, sx_shootHit_Plaster, 10, transform.Find("SFXPool/Pool_SX_ShootHit_Plaster"));
        CreatePool(sx_shootHit_Rock.name, sx_shootHit_Rock, 10, transform.Find("SFXPool/Pool_SX_ShootHit_Rock"));
        CreatePool(sx_shootHit_Water.name, sx_shootHit_Water, 10, transform.Find("SFXPool/Pool_SX_ShootHit_Water"));
        //脚步音效
        CreatePool(sx_playerFoot_L.name, sx_playerFoot_L, 10, transform.Find("SFXPool/Pool_SX_Player_Foot_L"));
        CreatePool(sx_playerFoot_R.name, sx_playerFoot_R, 10, transform.Find("SFXPool/Pool_SX_Player_Foot_R"));
        //Buff
        CreatePool(sx_buff_灼烧.name, sx_buff_灼烧, 10, transform.Find("SFXPool/Pool_SX_Buff_灼烧"));

        #endregion

        #region 特效VFX
        //buff特效
        CreatePool(灼烧.name, 灼烧, 10, transform.Find("VFXPool/Buff/灼烧"));
        CreatePool(淘汰回放爆炸.name, 淘汰回放爆炸, 10, transform.Find("VFXPool/Buff/淘汰回放爆炸"));
        CreatePool(淘汰回放爆炸球.name, 淘汰回放爆炸球, 10, transform.Find("VFXPool/Buff/淘汰回放爆炸球"));
        CreatePool(火焰印记爆炸.name, 火焰印记爆炸, 10, transform.Find("VFXPool/Buff/火焰印记爆炸"));
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

    public void Recycle(string key, GameObject obj,Transform parent = null)
    {
        if (!poolDict.ContainsKey(key))
        {
            Debug.LogError($"池不存在: {key}");
            return;
        }

        poolDict[key].Recycle(obj,parent);
    }

    public bool HasPool(string poolName)
    {
        return poolDict.ContainsKey(poolName);
    }

}
