using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Pet_Idle : Action
{
    public SharedPetManager self;
    public float idleMinTime;
    public float idleMaxTime;
    private float currentIdleTime;
    private float idleTimer;
    public override void OnStart()
    {
        base.OnStart();

        idleTimer = 0f;
        currentIdleTime = Random.Range(idleMinTime, idleMaxTime);

        self.Value.animator.CrossFade("Idle", 0.25f);

        self.Value.agent.enabled = false;


    }

    public override TaskStatus OnUpdate()
    {
        idleTimer += Time.deltaTime;
        if(idleTimer > currentIdleTime)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}
