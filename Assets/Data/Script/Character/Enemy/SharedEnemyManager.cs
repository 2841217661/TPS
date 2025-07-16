using UnityEngine;
using BehaviorDesigner.Runtime;

[System.Serializable]
public class SharedEnemyManager : SharedVariable<EnemyManager>
{
    public static implicit operator SharedEnemyManager(EnemyManager value)
    {
        return new SharedEnemyManager { Value = value };
    }
}
