using UnityEngine;
using DG.Tweening;
using TMPro;

public class QuestFinishShowItem : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    [SerializeField] private Animator animator;
    private float lifeTimer = 0f;

    [Header("UI设置")]
    [SerializeField] private TextMeshProUGUI Text_questType;
    [SerializeField] private TextMeshProUGUI Text_questId;

    private bool hasInitialized = false;

    public void Initialize(QuestType _type, string _id)
    {
        switch (_type)
        {
            case QuestType.Main:
                Text_questType.text = "主线";
                break;
            case QuestType.Branch:
                Text_questType.text = "支线";
                break ;
        }
        Text_questId.text = _id;
        hasInitialized = true;
    }

    private void Awake()
    {
        animator.SetBool("Enter", true);
    }

    private void Update()
    {
        if (!hasInitialized)
            return;

        lifeTimer += Time.deltaTime;

        if (lifeTimer > lifeTime)
        {
            animator.SetBool("Enter", false);
        }
    }
}
