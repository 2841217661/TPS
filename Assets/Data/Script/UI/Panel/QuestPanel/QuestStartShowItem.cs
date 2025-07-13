using TMPro;
using UnityEngine;

public class QuestStartShowItem : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    [SerializeField] private Animator animator;
    private float lifeTimer = 0f;

    [Header("UIÉèÖÃ")]
    [SerializeField] private TextMeshProUGUI Text_questId;
    [SerializeField] private TextMeshProUGUI Text_questStepName;

    private bool hasInitialized = false;

    public void Initialize(string _id, string _stepName)
    {
        Text_questId.text = _id;
        Text_questStepName.text = _stepName;
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
