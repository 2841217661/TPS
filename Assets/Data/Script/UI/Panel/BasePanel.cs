using UnityEngine;

public class BasePanel : MonoBehaviour
{
    protected bool isRemove = false; //标记该面板是否已经被移除了
    protected new string name; //对于子类，隐藏继承的父类的name字段

    public virtual void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public virtual void OpenPanel(string name)
    {
        this.name = name;
        SetActive(true);
    }

    public virtual void ClosePanel()
    {
        isRemove = true;
        //SetActive(false);
        Destroy(gameObject);

        //移除已打开面板的缓存
        if (UIManager.Instance.panelDict.ContainsKey(name))
        {
            UIManager.Instance.panelDict.Remove(name);
        }
    }
}
