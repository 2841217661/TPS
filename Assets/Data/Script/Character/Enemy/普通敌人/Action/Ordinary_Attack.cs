using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Ordinary_Attack : Action
{
    public SharedEnemyManager self;
    private Vector3 targetRotateDir;

    public override void OnStart()
    {
        self.Value.animator.CrossFade("Attack", 0.2f);
        self.Value.state = EnemyState.Attack;
        Vector3 targetDir = self.Value.target.position - self.Value.transform.position;
        targetDir.y = 0f;
        targetRotateDir = targetDir;
    }

    public override TaskStatus OnUpdate()
    {
        AnimatorStateInfo stateInfo = self.Value.animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Attack") && self.Value.animator.IsInTransition(0))
        {
            return TaskStatus.Success;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetRotateDir);
        self.Value.transform.rotation = Quaternion.RotateTowards(self.Value.transform.rotation, targetRotation, self.Value.rotateSpeed * Time.deltaTime);
        return TaskStatus.Running;
    }
}
