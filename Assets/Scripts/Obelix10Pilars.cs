using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class Obelix10Pilars : Agent
{
    public float moveSpeed = 5f;
    public float turnSpeed = 200f;

    public GameObject menhirOnBack;
    public GameObject[] allMenhirs;
    public GameObject[] allDestinations;

    public Material emptyDestinationMat;
    public Material fullDestinationMat;

    private bool hasMenhir = false;
    private int menhirsDelivered = 0;
    private Rigidbody rb;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        // Reset agent state
        hasMenhir = false;
        menhirsDelivered = 0;
        menhirOnBack.SetActive(false);

        // Reset agent position and physics
        this.transform.localPosition = new Vector3(0, 0.5f, 0);
        this.transform.localRotation = Quaternion.identity;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Reset all menhirs to a random position
        foreach (GameObject m in allMenhirs)
        {
            m.SetActive(true);
            m.transform.localPosition = new Vector3(Random.Range(-8f, 8f), 0.5f, Random.Range(-8f, 8f));
            
            Rigidbody mRb = m.GetComponent<Rigidbody>();
            if (mRb != null)
            {
                mRb.linearVelocity = Vector3.zero;
                mRb.angularVelocity = Vector3.zero;
            }
        }

        // Reset all destinations and tags
        foreach (GameObject d in allDestinations)
        {
            d.tag = "Destination";
            d.GetComponent<MeshRenderer>().material = emptyDestinationMat;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Note: Space Size in the Inspector MUST be 2
        sensor.AddObservation(hasMenhir ? 1.0f : 0.0f);
        sensor.AddObservation((float)menhirsDelivered / allMenhirs.Length);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int moveAction = actions.DiscreteActions[0];
        int turnAction = actions.DiscreteActions[1];

        // 0 = idle, 1 = forward, 2 = backward
        float moveAmount = 0f;
        if (moveAction == 1) moveAmount = 1f;
        if (moveAction == 2) moveAmount = -1f;

        // 0 = idle, 1 = turn right, 2 = turn left
        float turnAmount = 0f;
        if (turnAction == 1) turnAmount = 1f;
        if (turnAction == 2) turnAmount = -1f;

        transform.Translate(Vector3.forward * moveAmount * moveSpeed * Time.fixedDeltaTime);
        transform.Rotate(Vector3.up * turnAmount * turnSpeed * Time.fixedDeltaTime);

        // Very small time penalty to encourage speed without punishing exploration too hard
        AddReward(-0.00005f);

        // Check if agent fell off the platform
        if (this.transform.localPosition.y < 0)
        {
            SetReward(-1.0f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) discreteActionsOut[0] = 1;
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) discreteActionsOut[0] = 2;
        else discreteActionsOut[0] = 0;

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) discreteActionsOut[1] = 1;
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) discreteActionsOut[1] = 2;
        else discreteActionsOut[1] = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Pick up a menhir
        if (collision.gameObject.CompareTag("Menhir"))
        {
            if (!hasMenhir)
            {
                hasMenhir = true;
                menhirOnBack.SetActive(true);
                
                // Clear positive reward for finding the menhir
                AddReward(0.5f);
                collision.gameObject.SetActive(false); 
            }
        }
        // Deliver the menhir to a destination
        else if (collision.gameObject.CompareTag("Destination"))
        {
            if (hasMenhir)
            {
                MeshRenderer destMesh = collision.gameObject.GetComponent<MeshRenderer>();
                
                // Ensure the destination isn't already full
                if (destMesh.sharedMaterial != fullDestinationMat)
                {
                    destMesh.material = fullDestinationMat;
                    hasMenhir = false;
                    menhirOnBack.SetActive(false);
                    menhirsDelivered++;

                    // Strong reward for a successful delivery
                    AddReward(1.0f);

                    // Make the destination invisible to the ray perception sensor
                    collision.gameObject.tag = "Untagged";

                    // Check if all menhirs are delivered
                    if (menhirsDelivered >= allMenhirs.Length)
                    {
                        // Jackpot reward for finishing the entire task
                        AddReward(2.0f);
                        EndEpisode();
                    }
                }
            }
        }
    }
}