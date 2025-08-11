using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Ordinary_KonckCheck : Conditional
{
    public SharedEnemyManager self;
    public override TaskStatus OnUpdate()
    {
        if(self.Value.state == EnemyState.KnockUp
            || self.Value.state == EnemyState.KnockBack
            || self.Value.state == EnemyState.Dizzy)
        {
            return TaskStatus.Success;
        }
        
        return TaskStatus.Failure;
    }
}
