using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Ordinary_VectoryCheck : Conditional
{
    public SharedEnemyManager self;

    public override TaskStatus OnUpdate()
    {
        return self.Value.state == EnemyState.Vectory ? TaskStatus.Success : TaskStatus.Failure;
    }
}
