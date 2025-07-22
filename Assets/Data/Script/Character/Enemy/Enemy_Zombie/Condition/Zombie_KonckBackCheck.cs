using BehaviorDesigner.Runtime.Tasks;

public class Zombie_KonckBackCheck : Conditional
{
    public SharedEnemyManager self;
    public override TaskStatus OnUpdate()
    {
        return self.Value.state == EnemyState.KnockBack ? TaskStatus.Success : TaskStatus.Failure;
    }
}
