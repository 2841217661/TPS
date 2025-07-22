using UnityEngine;

public class BulletEventListener : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.Instance.bulletEvent.onBulletHitObject += BulletHitObject;
        EventManager.Instance.bulletEvent.onBulletSpawn += BulletSpawn;
    }

    private void OnDisable()
    {
        if (EventManager.Instance == null) return;

        EventManager.Instance.bulletEvent.onBulletHitObject -= BulletHitObject;
        EventManager.Instance.bulletEvent.onBulletSpawn -= BulletSpawn;
    }

    //子弹命中事件方法
    private void BulletHitObject(BulletType _type, CharacterManager _manaer)
    {
        switch (_type)
        {
            case BulletType.normal:
                Debug.Log("碰撞的子弹是: " +  _type); 
                break;
            case BulletType.track:
                Debug.Log("碰撞的子弹是: " +  _type);
                break;
        }
    }

    private void BulletSpawn(BulletType _type)
    {
        switch (_type)
        {
            case BulletType.normal:
                Debug.Log("生成的子弹是: " +  _type);
                break;
            case BulletType.track:
                Debug.Log("生成的子弹是: " +  _type);
                break;
        }
    }
}
