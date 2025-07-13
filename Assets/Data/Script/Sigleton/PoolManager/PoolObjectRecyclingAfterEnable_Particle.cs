using UnityEngine;
using System.Collections;

//带有粒子系统的对象池对象延迟回收
public class PoolObjectRecyclingAfterEnable_Particle : MonoBehaviour
{
    private ParticleSystem[] particleSystems;
    private Coroutine recycleCoroutine;

    private void Awake()
    {
        // 缓存所有子粒子
        particleSystems = GetComponentsInChildren<ParticleSystem>(true);
    }

    private void OnEnable()
    {
        // 每次启用，重新播放所有粒子（如果需要）
        foreach (var ps in particleSystems)
        {
            ps.Play(true);
        }

        // 启动协程
        if (recycleCoroutine != null)
        {
            StopCoroutine(recycleCoroutine);
        }
        recycleCoroutine = StartCoroutine(WaitAndRecycle());
    }

    private IEnumerator WaitAndRecycle()
    {
        // 每帧检测，直到所有粒子死亡
        while (true)
        {
            bool allDead = true;

            foreach (var ps in particleSystems)
            {
                if (ps.IsAlive(true))
                {
                    allDead = false;
                    break;
                }
            }

            if (allDead)
            {
                break;
            }

            yield return null;
        }
        string poolName = gameObject.name;
        PoolManager.Instance.Recycle(poolName, gameObject);
    }

    private void OnDisable()
    {
        // 停止协程
        if (recycleCoroutine != null)
        {
            StopCoroutine(recycleCoroutine);
            recycleCoroutine = null;
        }
    }
}
