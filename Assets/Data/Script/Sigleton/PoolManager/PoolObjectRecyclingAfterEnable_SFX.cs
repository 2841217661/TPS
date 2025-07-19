using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PoolObjectRecyclingAfterEnable_SFX : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        //3D音效效果
        audioSource.spatialBlend = 1f;

        // 设置衰减方式为线性
        audioSource.rolloffMode = AudioRolloffMode.Linear;

        audioSource.minDistance = 2f;    // 2米内音量最大
        audioSource.maxDistance = 10f;   // 超过10米逐渐听不到

    }

    private void OnEnable()
    {
        // 随机音调
        audioSource.pitch = Random.Range(0.8f, 1.2f);

        // 如果想要随机起始播放位置(有些音效不适合)
        // audioSource.time = Random.Range(0f, audioSource.clip.length);

        // 播放
        audioSource.Play();

        StartCoroutine(CheckRecycle());
    }


    private IEnumerator CheckRecycle()
    {
        // 等待clip播放完
        yield return new WaitForSeconds(audioSource.clip.length);

        // 回收对象
        PoolManager.Instance.Recycle(gameObject.name, gameObject);
    }
}
