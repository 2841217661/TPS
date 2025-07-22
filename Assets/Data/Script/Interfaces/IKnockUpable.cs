using UnityEngine;

public interface IKnockUpable
{
    void ApplyExplosionForce(Vector3 explosionPosition, float force, float radius, float upwardModifier = 0.5f);

}
