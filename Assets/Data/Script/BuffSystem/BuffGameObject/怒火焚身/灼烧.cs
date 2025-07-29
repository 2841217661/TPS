using UnityEngine;

public class 灼烧 : MonoBehaviour
{
    [HideInInspector] public Transform target;
    [SerializeField] private Vector3 positionOffset;

    private void Update()
    {
        if (target == null)
        {
            PoolManager.Instance.Recycle(gameObject.name, gameObject);
            return;
        }

        transform.position = target.position + positionOffset;
        transform.rotation = target.rotation;
    }
}
