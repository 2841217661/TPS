using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PoolObjectRecyclingAfterEnable_SFX : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        //3D��ЧЧ��
        audioSource.spatialBlend = 1f;

        audioSource.minDistance = 3f;    // 3�����������
        audioSource.maxDistance = 20f;   // ����20����������

    }

    private void OnEnable()
    {
        // �������
        audioSource.pitch = Random.Range(0.9f, 1.1f);

        // �����Ҫ�����ʼ����λ��(��Щ��Ч���ʺ�)
        // audioSource.time = Random.Range(0f, audioSource.clip.length);

        // ����
        audioSource.Play();

        StartCoroutine(CheckRecycle());
    }


    private IEnumerator CheckRecycle()
    {
        // �ȴ�clip������
        yield return new WaitForSeconds(audioSource.clip.length);

        // ���ն���
        PoolManager.Instance.Recycle(gameObject.name, gameObject);
    }
}
