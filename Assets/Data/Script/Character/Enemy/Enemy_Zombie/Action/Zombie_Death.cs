using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
public class Zombie_Death : Action
{
    public SharedEnemyManager self;


    public override TaskStatus OnUpdate()
    {
        self.Value.OnDeath();
        return TaskStatus.Success;
    }
}
