using UnityEngine;

public class PartalManager : CharacterManager
{
    [Header("怪物生成设置")]
    public float generateMinInterval;  //生成最小间隔
    public float generateMaxInterval; //生成最大间隔
    public GameObject[] enemys; //该传送门可生成的怪物类型
    private float generateTimer; //生成计时器
    private float currentGenerateInterval; //当前生成的时间间隔

    protected override void Start()
    {
        base.Start();

        currentGenerateInterval = Random.Range(generateMinInterval, generateMaxInterval);
    }

    protected override void Update()
    {
        base.Update();

        if (GameManager.Instance.currentEnemyCount >= GameManager.Instance.maxEnemyCount) return;

        TryGenerateEnemyRandom();
    }

    //尝试随机生成一个敌人
    private void TryGenerateEnemyRandom()
    {
        generateTimer += Time.deltaTime;
        if(generateTimer > currentGenerateInterval)
        {
            //TODO:生成一个敌人
            Debug.Log("生成一个敌人////////////////////////////////////////////////");
            GameManager.Instance.currentEnemyCount++;
            int index = Random.Range(0, enemys.Length);
            Instantiate(enemys[index],transform.position,transform.rotation);

            currentGenerateInterval = Random.Range(generateMinInterval, generateMaxInterval);
            generateTimer = 0;
        }
    }
}
