using UnityEngine;

public class trMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector3 inputDirection;

    private void Update()
    {
        // 获取输入方向（不带 Y）
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (inputDirection.sqrMagnitude > 0.01f)
        {
            // 直接修改位置实现移动
            Vector3 move = inputDirection * moveSpeed * Time.deltaTime;
            transform.position += move;
        }
    }
}