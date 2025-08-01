using UnityEngine;

public class NPC_TraderManager : NPCManager
{
    //挑选商品：
    public void SelectProducts()
    {
        Debug.Log("准备挑选商品");
    }


    //没钱：
    public void NoMoney()
    {
        Debug.Log("没有钱");

        Destroy(gameObject);
    }
}
