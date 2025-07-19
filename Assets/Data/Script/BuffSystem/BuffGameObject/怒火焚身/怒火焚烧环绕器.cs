using System.Collections.Generic;
using UnityEngine;

//只能作用于玩家
public class 怒火焚烧环绕器 : MonoBehaviour
{
    [HideInInspector] public Transform orbitTransform; //以谁的位置为基准
    public GameObject orbitObject; //环绕物
    public float orbitRadius; //环绕半径
    public float orbitRotateSpeed; //环绕速度:每秒环绕多少°,正为顺时针，负为逆时针
    public List<GameObject> orbitObjectList = new List<GameObject>(5); //环绕物容器


    private void Awake()
    {
        orbitTransform = GameManager.Instance.playerManager.transform.Find("OrbitSpawner");
        transform.position = orbitTransform.position;
    }

    private void Update()
    {
        //更新位置
        transform.position = orbitTransform.position;
        
        //旋转并面向自己
        for(int i = 0; i < orbitObjectList.Count; i++)
        {
            orbitObjectList[i].transform.RotateAround(transform.position, Vector3.up, orbitRotateSpeed * Time.deltaTime);
            orbitObjectList[i].transform.localRotation = Quaternion.LookRotation(transform.position - orbitObjectList[i].transform.position);
        }
    }

    //添加火焰
    public void AddOrbitObject(int _count)
    {
        for (int i = 0; i < _count; i++)
        {
            //在环绕器位置生成
            GameObject obj = Instantiate(orbitObject,transform);
            orbitObjectList.Add(obj);
        }

        //分配位置,此时所有
        float angleInterval = 360 / orbitObjectList.Count;
        for (int i = 0; i < orbitObjectList.Count; i++)
        {
            orbitObjectList[i].transform.localPosition = Vector3.zero; //先全部重置位置

            float angle = i * angleInterval;
            orbitObjectList[i].transform.position = transform.position + transform.forward * orbitRadius; //起始旋转点
            //以tansform为圆心
            orbitObjectList[i].transform.RotateAround(transform.position,Vector3.up, angle);
        }
    }
}