using System.Collections.Generic;
using UnityEngine;

public class Minmap : MonoSingleton<Minmap>
{
    private Transform followTarget;
    [HideInInspector] public Camera minimapCamera;

    protected override void Init()
    {
        base.Init();

        minimapCamera = GetComponent<Camera>();
    }
    private void Start()
    {
        followTarget = GameManager.Instance.playerManager.transform;
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(followTarget.position.x, transform.position.y, followTarget.position.z);
    }

    // 判断目标是否在小地图显示范围内
    public bool IsInsideMinimapCircle(Transform target)
    {
        Vector3 viewportPos = minimapCamera.WorldToViewportPoint(target.position);

        // 在摄像机前面
        if (viewportPos.z <= 0) return false;

        // 计算中心偏移（以0.5为中心）
        float dx = viewportPos.x - 0.5f;
        float dy = viewportPos.y - 0.5f;

        // 判断是否在圆形半径内（单位圆，半径0.5）
        return dx * dx + dy * dy <= 0.25f;
    }

    //生成小地图索引图标
    public void AddMinmapIcon(Transform _target, MinmapIconType _iconType)
    {
        GameObject minmapIcon = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Minmap/MinmapIcon"));
        MinmapIcon minmap = minmapIcon.GetComponent<MinmapIcon>();
        minmap.Init(_target, _iconType);
        minmapIconList.Add(minmapIcon);
    }

    private List<GameObject> minmapIconList = new List<GameObject>(5);
    //删除小地图索引图标
    public void RemoveMinmapIcon(Transform _target, MinmapIconType _iconType)
    {
        Debug.LogWarning("开始寻找");
        foreach (GameObject minmapIcon in minmapIconList)
        {
            MinmapIcon minmap = minmapIcon.GetComponent<MinmapIcon>();
            if(minmap.target == _target && minmap.minmapIconType == _iconType)
            {
                Debug.LogWarning("找到了");
                minmapIconList.Remove(minmapIcon);
                Destroy(minmapIcon);
                return;
            }
        }
        Debug.LogWarning("没有找到");
    }
}
