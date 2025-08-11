using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Ordinary_KnockUpCheck : Conditional
{
    public SharedEnemyManager self;

    public override TaskStatus OnUpdate()
    {
        return self.Value.state == EnemyState.KnockUp ? TaskStatus.Success : TaskStatus.Failure;
    }
}
