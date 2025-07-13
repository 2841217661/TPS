using UnityEngine;

public class LoadeSceneManager : MonoSingleton<LoadeSceneManager>
{
    public GameObject loadScenePre;
    private GameObject loadScene;
    
    protected override void Init()
    {
        base.Init();

        DontDestroyOnLoad(gameObject);
    }

    public void StartLoad()
    {
        loadScene = Instantiate(loadScenePre,this.transform);
    }

    public void EndLoad()
    {
        loadScene.GetComponent<Animator>().SetTrigger("End");
    }
}
