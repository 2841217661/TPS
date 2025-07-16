using BehaviorDesigner.Runtime.Tasks;

public class Zombie_DamageCheck : Conditional
{
    public SharedEnemyManager self;
    public override TaskStatus OnUpdate()
    {
        return self.Value.state == EnemyState.Damage ? TaskStatus.Success : TaskStatus.Failure;
    }
}
