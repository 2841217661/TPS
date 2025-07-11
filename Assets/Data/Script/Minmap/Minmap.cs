using UnityEngine;

public class Minmap : MonoBehaviour
{
    public Transform followTarget;

    private void LateUpdate()
    {
        transform.position = new Vector3(followTarget.position.x, transform.position.y, followTarget.position.z);
    }
}
