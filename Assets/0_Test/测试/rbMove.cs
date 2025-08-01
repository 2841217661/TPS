using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class rbMove : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody rb;
    private Vector3 inputDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // 防止因碰撞导致旋转
    }

    private void Update()
    {
        // 获取输入方向（不带 Y）
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (inputDirection.sqrMagnitude > 0.01f)
        {
            // 计算新位置并移动
            Vector3 move = inputDirection * moveSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + move);
        }
    }
}
