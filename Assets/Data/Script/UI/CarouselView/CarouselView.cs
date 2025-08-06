using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CircularRollUIType
{
    Horizontal,
    Vertical
}

public abstract class CarouselView : MonoBehaviour
{
    [Header("切换速度")]
    public float speed; //切换时item的移动速度

    [Header("滚动模式")]
    public CircularRollUIType rollUIType; //滚动模式
    public Transform itemContent;

    [Header("位置偏移量")]
    public float m_xOffset;
    public float m_yOffset;

    [Header("透明度设置")]
    [Tooltip("每次跨度是上一次alpha的多少倍")]
    public float m_alphaStepValue;

    [Header("缩放设置")]
    [Tooltip("每次跨度是上一次缩放的多少倍")]
    public float m_scaleStepValue;

    [Header("个数")]
    public List<GameObject> m_items;
    public int m_itemCount
    {
        get { return m_items.Count; }
    }
    public Dictionary<int,Transform> m_itemDic = new Dictionary<int, Transform>(); //索引:item
    private int halfCount { get { return m_itemCount / 2; } }

    protected virtual void Awake()
    {
        //删除所有子物体
        for (int i = itemContent.childCount - 1; i >= 0; i--)
        {
            Destroy(itemContent.GetChild(i).gameObject);
        }

        //生成物体，并添加入字典
        for (int i = 0; i < m_itemCount; i++)
        {
            var item = Instantiate(m_items[i], transform.position, Quaternion.identity, itemContent);
            m_itemDic[i] = item.GetComponent<Transform>();
        }

        InitOffsetAndAlphaAndScale();
    }

    private void InitSibling()
    {
        if (m_itemCount % 2 == 0) // 偶数个，中心元素只有一个
        {
            for (int i = halfCount - 1; i > 0; i--)
            {
                m_itemDic[i].transform.SetAsLastSibling();
                m_itemDic[m_itemCount - i].transform.SetAsLastSibling();
            }

            m_itemDic[0].transform.SetAsLastSibling();           // 最后渲染
            m_itemDic[halfCount].transform.SetAsFirstSibling();  // 最先渲染（中心）
        }
        else // 奇数个，中心元素是两个
        {
            for (int i = halfCount; i > 0; i--)
            {
                m_itemDic[i].transform.SetAsLastSibling();
                m_itemDic[m_itemCount - i].transform.SetAsLastSibling();
            }

            m_itemDic[0].transform.SetAsLastSibling(); // 最后渲染
        }

    }


    /// <summary>
    /// 初始化物体，并设置位置偏移、透明度、缩放
    /// </summary>
    private void InitOffsetAndAlphaAndScale()
    {
        //设置偏移
        float angle = 360 / m_itemCount;
        foreach (var _item in m_itemDic)
        {
            if (rollUIType == CircularRollUIType.Horizontal)
            {
                //水平：规定向右为正方向
                //x偏移
                float xOffset = Mathf.Sin(angle * _item.Key * Mathf.Deg2Rad) * m_xOffset;
                float zOffset = Mathf.Cos(angle * _item.Key * Mathf.Deg2Rad) * m_xOffset;
                _item.Value.localPosition += new Vector3(xOffset, 0f, zOffset);
            }
            else
            { 
                //垂直：规定向上为正方向
                float yOffset = Mathf.Sin(angle * _item.Key * Mathf.Deg2Rad) * m_yOffset;
                float zOffset = Mathf.Cos(angle * _item.Key * Mathf.Deg2Rad) * m_yOffset;
                _item.Value.localPosition += new Vector3(0f, yOffset, zOffset);
            }
        }

        //继续设置偏移,透明度，缩放
        int bundle = halfCount;
        float preAlpha = 1f;
        float preScale = 1f;
        if (m_itemCount % 2 == 0) //最后会单一个元素{
        {
            for (int i = 1; i <= bundle; i++)
            {
                if (rollUIType == CircularRollUIType.Horizontal)
                {
                    float yOffset = i * m_yOffset;
                    m_itemDic[i].GetComponent<RectTransform>().localPosition += new Vector3(0f, yOffset, 0f);
                    if(i != bundle)
                    {
                        m_itemDic[m_itemCount - i].GetComponent<RectTransform>().localPosition += new Vector3(0f, yOffset, 0f);
                    }
                }
                else
                {
                    float xOffset = i * m_xOffset;
                    m_itemDic[i].GetComponent<RectTransform>().localPosition += new Vector3(xOffset, 0f, 0f);
                    if (i != bundle)
                    {
                        m_itemDic[m_itemCount - i].GetComponent<RectTransform>().localPosition += new Vector3(xOffset, 0f, 0f);
                    }
                }

                float nextAlpha = preAlpha * m_alphaStepValue;
                float nextScale = preScale * m_scaleStepValue;

                m_itemDic[i].GetComponent<CanvasGroup>().alpha = nextAlpha;
                m_itemDic[i].GetComponent <RectTransform>().localScale = Vector3.one * nextScale;

                if (i != bundle)
                {
                    m_itemDic[m_itemCount - i].GetComponent<CanvasGroup>().alpha = nextAlpha;
                    m_itemDic[m_itemCount - i].GetComponent<RectTransform>().localScale = Vector3.one * nextScale;
                }

                preAlpha = nextAlpha;
                preScale = nextScale;
            }
        }
        else //最后一个bundle是两个
        {
            for (int i = 1; i <= bundle; i++)
            {
                if (rollUIType == CircularRollUIType.Horizontal)
                {
                    float yOffset = i * m_yOffset;
                    m_itemDic[i].GetComponent<RectTransform>().localPosition += new Vector3(0f, yOffset, 0f);
                    m_itemDic[m_itemCount - i].GetComponent<RectTransform>().localPosition += new Vector3(0f, yOffset, 0f);
                }
                else
                {
                    float xOffset = i * m_xOffset;
                    m_itemDic[i].GetComponent<RectTransform>().localPosition += new Vector3(xOffset, 0f, 0f);
                    m_itemDic[m_itemCount - i].GetComponent<RectTransform>().localPosition += new Vector3(xOffset, 0f, 0f);
                }

                float nextAlpha = preAlpha * m_alphaStepValue;
                float nextScale = preScale * m_scaleStepValue;

                m_itemDic[i].GetComponent<CanvasGroup>().alpha = nextAlpha;
                m_itemDic[i].GetComponent<RectTransform>().localScale = Vector3.one * nextScale;

                m_itemDic[m_itemCount - i].GetComponent<CanvasGroup>().alpha = nextAlpha;
                m_itemDic[m_itemCount - i].GetComponent<RectTransform>().localScale = Vector3.one * nextScale;

                preAlpha = nextAlpha;
                preScale = nextScale;
            }
        }

        InitSibling();

        OnAfterInif();
    }

    private bool isInChange;

    public void OnClickPre()
    {
        StartCoroutine(ChangePreItem());
    }

    public void OnClickNext()
    {
        StartCoroutine(ChangeNextItem());
    }

    private IEnumerator ChangePreItem()
    {
        if (isInChange) yield break;

        for (int i = 0; i < m_itemCount; i++)
        {
            if (i != m_itemCount - 1) //不是最后一个item
            {
                //以后面的元素作为目标，开启移动协程
                StartCoroutine(MoveToTarget(m_itemDic[i].transform, m_itemDic[i + 1].transform));
            }
            else //最后一个元素
            {
                //最后一个元素需要向第一个元素移动
                StartCoroutine(MoveToTarget(m_itemDic[i].transform, m_itemDic[0].transform));
            }
        }

        Transform lastItem = m_itemDic[m_itemCount - 1];

        for (int i = m_itemCount - 1; i >= 0; i--)
        {
            if(i !=  0)
            {
                m_itemDic[i] = m_itemDic[i - 1];
            }
            else
            {
                m_itemDic[0] = lastItem;
            }
        }

        //重新设置渲染顺序
        InitSibling();

        isInChange = true;
        yield return new WaitForSeconds(1/speed + 0.1f);
        isInChange = false;

        OnPreClick();
    }

    private IEnumerator ChangeNextItem()
    {
        if (isInChange) yield break;

        for (int i = 0; i < m_itemCount; i++) 
        {
            if(i != 0) //不是第一个item
            {
                //以前面的元素作为目标，开启移动协程
                StartCoroutine(MoveToTarget(m_itemDic[i].transform, m_itemDic[i - 1].transform));
            }
            else //第一个元素
            {
                //第一个元素需要向最后一个元素移动
                StartCoroutine(MoveToTarget(m_itemDic[0].transform, m_itemDic[m_itemCount - 1].transform));
            }
        }

        Transform lastItem = m_itemDic[0];

        for (int i = 0; i < m_itemCount; i++)
        {
            if (i != m_itemCount - 1)
            {
                m_itemDic[i] = m_itemDic[i + 1];
            }
            else
            {
                m_itemDic[i] = lastItem;
            }
        }

        //重新设置渲染顺序
        InitSibling();

        isInChange = true;
        yield return new WaitForSeconds(1/speed + 0.1f);
        isInChange = false;

        OnNextClick();
    }

    

    private IEnumerator MoveToTarget(Transform _tr, Transform _target)
    {
        Vector3 targetPosition = _target.localPosition; //目标位置
        //在移动过程中，也逐渐改变缩放、透明度
        CanvasGroup cg = _tr.GetComponent<CanvasGroup>();
        float startAlpha = cg.alpha; //当前alpha
        float targetAlpha = _target.GetComponent<CanvasGroup>().alpha; //目标alpha
        Vector3 startScale = _tr.localScale; //当前缩放
        Vector3 targetScale = _target.localScale; //目标缩放
        float totalDistance = Vector3.Distance(_tr.localPosition, targetPosition); //总距离
        float movedDistance = 0f; //当前已经移动的距离
        yield return null;


        //移动速度与距离成正比，无论距离如何，移动的时间始终为1/Speed
        float tempSpeed = (_tr.transform.localPosition - targetPosition).magnitude * speed;
        while (_tr.localPosition != targetPosition)
        {
            Vector3 previousPosition = _tr.localPosition;

            _tr.localPosition = Vector3.MoveTowards(_tr.localPosition, targetPosition, tempSpeed * Time.deltaTime);

            movedDistance += (_tr.localPosition - previousPosition).magnitude; //累计已经移动的距离
            float t = Mathf.Clamp01(movedDistance / totalDistance);
            cg.alpha = Mathf.Lerp(startAlpha,targetAlpha,t);
            _tr.localScale = Vector3.Lerp(startScale,targetScale,t);
            yield return null;
        }

        _tr.localPosition = targetPosition;
        cg.alpha = targetAlpha;
        _tr.localScale = targetScale;
    }

    #region 重写方法
    //初始化完成后执行
    protected virtual void OnAfterInif() { }

    //Pre切换完成后执行
    protected virtual void OnPreClick() { }

    //Next切换完成后执行
    protected virtual void OnNextClick() { }

    #endregion
}
