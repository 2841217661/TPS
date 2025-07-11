using UnityEngine;
using System.Collections;

//��������ϵͳ�Ķ���ض����ӳٻ���
public class PoolObjectRecyclingAfterEnable_Particle : MonoBehaviour
{
    private ParticleSystem[] particleSystems;
    private Coroutine recycleCoroutine;

    private void Awake()
    {
        // ��������������
        particleSystems = GetComponentsInChildren<ParticleSystem>(true);
    }

    private void OnEnable()
    {
        // ÿ�����ã����²����������ӣ������Ҫ��
        foreach (var ps in particleSystems)
        {
            ps.Play(true);
        }

        // ����Э��
        if (recycleCoroutine != null)
        {
            StopCoroutine(recycleCoroutine);
        }
        recycleCoroutine = StartCoroutine(WaitAndRecycle());
    }

    private IEnumerator WaitAndRecycle()
    {
        // ÿ֡��⣬ֱ��������������
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
        // ֹͣЭ��
        if (recycleCoroutine != null)
        {
            StopCoroutine(recycleCoroutine);
            recycleCoroutine = null;
        }
    }
}
