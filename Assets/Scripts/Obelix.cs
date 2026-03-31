using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class ObelixAgent : Agent
{
    public float moveSpeed = 5f;
    public float turnSpeed = 200f;

    public GameObject menhirOnBack;
    public GameObject menhirInScene;
    public GameObject destinationInScene;
    public Material emptyDestinationMat;
    public Material fullDestinationMat;
    private bool hasMenhir = false;
    private Rigidbody rb;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        // Reset status
        hasMenhir = false;
        menhirOnBack.SetActive(false);

        // reset snelheid en terug zetten
        if (this.transform.localPosition.y < 0)
        {
            this.transform.localPosition = new Vector3(0, 0.5f, 0);
            this.transform.localRotation = Quaternion.identity;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        menhirInScene.SetActive(true);
        menhirInScene.transform.localPosition = new Vector3(Random.Range(-4f, 4f), 0.5f, Random.Range(-4f, 4f));

        Rigidbody menhirRB = menhirInScene.GetComponent<Rigidbody>();
        if (menhirRB != null)
        {
            menhirRB.linearVelocity = Vector3.zero;
            menhirRB.angularVelocity = Vector3.zero;
        }

        destinationInScene.transform.localPosition = new Vector3(Random.Range(-4f, 4f), 0.5f, Random.Range(-4f, 4f));
        destinationInScene.GetComponent<MeshRenderer>().material = emptyDestinationMat;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        // heeft menhir ja of nee
        sensor.AddObservation(hasMenhir ? 1.0f : 0.0f);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int moveAction = actions.DiscreteActions[0];
        int turnAction = actions.DiscreteActions[1];

        // 0 = stil, 1 = vooruit, 2 = achteruit
        float moveAmount = 0f;
        if (moveAction == 1) moveAmount = 1f;
        if (moveAction == 2) moveAmount = -1f;

        // 0 = stil, 1 = rechts, 2 = links
        float turnAmount = 0f;
        if (turnAction == 1) turnAmount = 1f;
        if (turnAction == 2) turnAmount = -1f;

        transform.Translate(Vector3.forward * moveAmount * moveSpeed * Time.fixedDeltaTime);
        transform.Rotate(Vector3.up * turnAmount * turnSpeed * Time.fixedDeltaTime);

        AddReward(-0.001f);

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
        if (collision.gameObject.CompareTag("Menhir"))
        {
            if (!hasMenhir)
            {
                hasMenhir = true;
                menhirOnBack.SetActive(true);
                AddReward(0.1f);
                collision.gameObject.SetActive(false);
            }
            else
            {
                AddReward(-0.1f);
            }
        }
        else if (collision.gameObject.CompareTag("Destination"))
        {
            if (hasMenhir)
            {
                collision.gameObject.GetComponent<MeshRenderer>().material = fullDestinationMat;
                SetReward(1.0f);
                EndEpisode();
            }
        }
    }
}