using UnityEngine;

public class A : MonoBehaviour
{
    private void OnDisable()
    {
        Debug.Log("A±»Disable");
    }

    private void OnDestroy()
    {
        Debug.Log("A±»Destroy");
    }
}
