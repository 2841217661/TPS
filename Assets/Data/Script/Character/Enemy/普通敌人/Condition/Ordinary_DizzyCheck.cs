using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Ordinary_DizzyCheck : Conditional
{
    public SharedEnemyManager self;

    public override TaskStatus OnUpdate()
    {
        return self.Value.state == EnemyState.Dizzy ? TaskStatus.Success : TaskStatus.Failure;
    }
}
