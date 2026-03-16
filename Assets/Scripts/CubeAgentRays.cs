using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CubeAgentRays: Agent
{
    public Transform Target;
    public Transform Goal;
    public bool hasTakenRed;
    
    public override void OnEpisodeBegin()
    {
        //in begin is rood nog niet geraakt
        hasTakenRed = false;
        if (this.transform.localPosition.y < 0)
        {
            this.transform.localPosition = new Vector3(0, 0.5f, 0);
            this.transform.localRotation = Quaternion.identity;
        }

        Target.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
    }
    //oberservations instellen
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(hasTakenRed);
    }


    public float speedMultiplier = 0.5f;
    public float rotationMultiplier = 5;
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {

        Vector3 controlSignal = Vector3.zero;
        controlSignal.z = actionBuffers.ContinuousActions[0];
        transform.Translate(controlSignal * speedMultiplier);

        transform.Rotate(0.0f, rotationMultiplier * actionBuffers.ContinuousActions[1], 0.0f);

        // Beloningen
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);
        float distanceToFlag = Vector3.Distance(this.transform.localPosition, Goal.localPosition);

        // Als rood geraakt wordt
        //max reward is 1.5
        if (!hasTakenRed)
        {
            if (distanceToTarget < 1.42f)
            {
                AddReward(0.5f);
                hasTakenRed = true;
            }
        } else {
            if (distanceToFlag < 1.42f)
            {
                AddReward(1.0f);
                EndEpisode();
            }
        }

        if (this.transform.localPosition.y < 0)
        {
            AddReward(-1f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }
}