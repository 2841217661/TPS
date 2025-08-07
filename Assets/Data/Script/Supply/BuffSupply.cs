using UnityEngine;

public class BuffSupply : MonoBehaviour
{

    private void Awake()
    {
        Minmap.Instance.AddMinmapIcon(this.transform, MinmapIconType.buff);
        Destroy(gameObject,60f); //在场景中不能待太久
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.OpenPanel("BuffSelectPanel", UIManager.Instance.UIRoot);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        Minmap.Instance.RemoveMinmapIcon(this.transform, MinmapIconType.buff);
    }
}
