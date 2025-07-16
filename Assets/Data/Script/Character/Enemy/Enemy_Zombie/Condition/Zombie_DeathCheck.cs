using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Zombie_DeathCheck : Conditional
{
    public SharedEnemyManager self;

    public override TaskStatus OnUpdate()
    {
        return self.Value.currentHealthValue <= 0f ? TaskStatus.Success : TaskStatus.Failure;
    }
}
