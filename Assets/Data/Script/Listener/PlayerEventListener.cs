using UnityEngine;

public class PlayerEventListener : MonoBehaviour
{
    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        if (EventManager.Instance == null) return;

    }

}
