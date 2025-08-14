using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Pet_Attack : Action
{
    public SharedPetManager self;
    private float rotationSpeed = 10f;

    public override void OnStart()
    {
        base.OnStart();

        self.Value.animator.CrossFade("Attack", 0.1f);
    }

    public override TaskStatus OnUpdate()
    {
        AnimatorStateInfo stateInfo = self.Value.animator.GetCurrentAnimatorStateInfo(0);
        if(stateInfo.IsName("Attack") && self.Value.animator.IsInTransition(0))
        {
            return TaskStatus.Success;
        }


        //攻击时面向目标
        Vector3 faceDir = self.Value.attackTarget.transform.position - self.Value.transform.position;
        Quaternion rotation = Quaternion.LookRotation(faceDir);
        self.Value.transform.rotation = Quaternion.Slerp(self.Value.transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        return TaskStatus.Running;
    }
}
