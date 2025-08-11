using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Ordinary_Vectory : Action
{
    public SharedEnemyManager self;

    public override void OnStart()
    {
        self.Value.animator.CrossFade("Victory", 0.2f);
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Running;
    }
}
