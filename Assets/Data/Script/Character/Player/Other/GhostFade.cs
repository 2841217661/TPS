using UnityEngine;

public class GhostFade : MonoBehaviour
{
    private Material mat;
    private Color color;
    private float fadeSpeed;

    public void Init(Material material, float lifetime)
    {
        mat = material;
        color = mat.color;
        fadeSpeed = color.a / lifetime;
    }

    void Update()
    {
        color.a -= fadeSpeed * Time.deltaTime;
        mat.color = color;
    }
}
