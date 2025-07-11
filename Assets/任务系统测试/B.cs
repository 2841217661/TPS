using UnityEngine;

public class B : MonoBehaviour
{
    private void OnDisable()
    {
        Debug.Log("B±»Disable");
    }

    private void OnDestroy()
    {
        Debug.Log("B±»Destroy");
    }
}
