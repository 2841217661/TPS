using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Pet_FarCheck : Conditional
{
    public SharedPetManager self;

    public override TaskStatus OnUpdate()
    {
        // ¼ÆËãÆ½·½¾àÀë£¬±ÜÃâ Mathf.Sqrt
        float sqrDistance = (self.Value.followTarget.position - self.Value.transform.position).sqrMagnitude;
        float farDistanceSqr = self.Value.farDistance * self.Value.farDistance;

        if(sqrDistance > farDistanceSqr)
        {
            self.Value.attackTarget = null;
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}
