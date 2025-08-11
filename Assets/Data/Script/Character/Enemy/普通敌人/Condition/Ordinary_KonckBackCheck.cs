using BehaviorDesigner.Runtime.Tasks;

public class Ordinary_KonckBackCheck : Conditional
{
    public SharedEnemyManager self;
    public override TaskStatus OnUpdate()
    {
        return self.Value.state == EnemyState.KnockBack ? TaskStatus.Success : TaskStatus.Failure;
    }
}
