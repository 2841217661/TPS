using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Pet_NearCheck : Conditional
{
    public SharedPetManager self;

    public override TaskStatus OnUpdate()
    {
        // ¼ÆËãÆ½·½¾àÀë£¬±ÜÃâ Mathf.Sqrt
        float sqrDistance = (self.Value.followTarget.position - self.Value.transform.position).sqrMagnitude;
        float nearDistanceSqr = self.Value.nearDistance * self.Value.nearDistance;

        return sqrDistance > nearDistanceSqr ? TaskStatus.Success : TaskStatus.Failure;
    }
}
