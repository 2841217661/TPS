using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class Ordinary_Death : Action
{
    public SharedEnemyManager self;

    public override void OnStart()
    {
        self.Value.OnDeath();
    }
}
