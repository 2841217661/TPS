using UnityEngine;
using System.Collections;

public class DestroyAfterParticlePlay : MonoBehaviour
{
    private ParticleSystem[] particleSystems;

    private void Awake()
    {
        // 获取所有子物体中的粒子系统
        particleSystems = GetComponentsInChildren<ParticleSystem>(true);
    }

    private void OnEnable()
    {
        // 启动协程
        StartCoroutine(WaitAndDestroy());
    }

    private IEnumerator WaitAndDestroy()
    {
        // 确保粒子开始播放
        foreach (var ps in particleSystems)
        {
            ps.Play(true);
        }

        // 等待所有粒子死亡
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
                break;

            yield return null;
        }

        Destroy(gameObject);
    }
}
