using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Ordinary_Dizzy : Action
{
    public SharedEnemyManager self;

    public override void OnStart()
    {
        base.OnStart();

        self.Value.animator.CrossFade("Dizzy", 0.1f);
    }

    public override TaskStatus OnUpdate()
    {
        if(self.Value.state == EnemyState.Dizzy)
        {
            return TaskStatus.Running;
        }

        return TaskStatus.Success;
    }
}
