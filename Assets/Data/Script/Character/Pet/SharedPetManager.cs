using BehaviorDesigner.Runtime;
using UnityEngine;

[System.Serializable]
public class SharedPetManager : SharedVariable<PetManager>
{
    public static implicit operator SharedPetManager(PetManager value)
    {
        return new SharedPetManager { Value = value };
    }
}
