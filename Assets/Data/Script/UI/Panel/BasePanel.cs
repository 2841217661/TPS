using UnityEngine;

public class BasePanel : MonoBehaviour
{
    protected bool isRemove = false; //��Ǹ�����Ƿ��Ѿ����Ƴ���
    protected new string name; //�������࣬���ؼ̳еĸ����name�ֶ�

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

        //�Ƴ��Ѵ����Ļ���
        if (UIManager.Instance.panelDict.ContainsKey(name))
        {
            UIManager.Instance.panelDict.Remove(name);
        }
    }
}
