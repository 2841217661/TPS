using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    private EnemyManager enemyManager;

    public Image healthBar1; //空血条
    public Image healthBar2; //过渡血条
    public Image healthBar3; //主血条

    private void Awake()
    {
        enemyManager = GetComponentInParent<EnemyManager>();
    }

    private void Update()
    {
        //始终面向主相机
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.back,
                 Camera.main.transform.rotation * Vector3.up);

        //血条与血量同步
        healthBar3.fillAmount = enemyManager.currentHealthValue / (float)enemyManager.maxHealthValue;

        //过渡血条平滑过渡
        healthBar2.fillAmount = Mathf.Lerp(healthBar2.fillAmount, healthBar3.fillAmount, 0.5f);
    }
}
