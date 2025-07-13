using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMeunManager : MonoBehaviour
{
    private AsyncOperation asyncOperation;
    private float timer = 0f;
    private bool loadingStarted = false;
    private bool loadCompleted = false;

    // 至少显示2秒
    private float minLoadTime = 2f;

    private void Update()
    {


        if (loadingStarted)
        {
            // 累加加载时间
            timer += Time.deltaTime;

            // 如果加载进度达到90%，表示已经加载完成
            if (asyncOperation.progress >= 0.9f)
            {
                loadCompleted = true;
            }

            // 当加载完成，且时间大于2秒，才切换场景
            if (loadCompleted && timer >= minLoadTime)
            {
                asyncOperation.allowSceneActivation = true;
                LoadeSceneManager.Instance.EndLoad();
                // 重置
                loadingStarted = false;
                timer = 0f;
                loadCompleted = false;
            }
        }
    }

    public void LoadScene()
    {
        if (!loadingStarted)
        {
            // 开始加载
            asyncOperation = SceneManager.LoadSceneAsync("GameScene");
            asyncOperation.allowSceneActivation = false; // 先不切换
            LoadeSceneManager.Instance.StartLoad();
            loadingStarted = true;
        }
    }
}
