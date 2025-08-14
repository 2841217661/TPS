using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Pet_EnemyCheck : Conditional
{
    public SharedPetManager self;

    public override TaskStatus OnUpdate()
    {
        if (self.Value.attackTarget != null) return TaskStatus.Success;

        //以followe为中心，检测远距离内是否存在敌人，如果存在敌人，获取最近的一个敌人
        Collider[] colliders = Physics.OverlapSphere(self.Value.followTarget.position, self.Value.farDistance, self.Value.attackLayer);
        if (colliders.Length > 0)
        {
            float minDis = Mathf.Infinity;
            Collider minDisCollider = null;
            for (int i = 0; i < colliders.Length; i++)
            {
                float dis = Vector3.Distance(self.Value.followTarget.position, colliders[i].transform.position);
                if (dis < minDis)
                {
                    minDisCollider = colliders[i];
                    minDis = dis;
                }
            }

            self.Value.attackTarget = minDisCollider.transform;
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}
