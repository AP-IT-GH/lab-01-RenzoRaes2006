using UnityEngine;

public class FollowWaypoint : MonoBehaviour
{
public GameObject[] waypoints;
    int currentWP = 0;

    public float speed = 5.0f;
    public float rotSpeed = 3.0f;
    public float accuracy = 3.0f;
    

    void Update()
    {
        if (waypoints.Length == 0) return;

        Vector3 lookAtGoal = new Vector3(waypoints[currentWP].transform.position.x, 
                                        transform.position.y, 
                                        waypoints[currentWP].transform.position.z);
        
        Vector3 direction = lookAtGoal - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotSpeed);

        if (Vector3.Distance(transform.position, lookAtGoal) < accuracy)
        {
            currentWP++;
            
            if (currentWP >= waypoints.Length)
            {
                currentWP = 0;
            }
        }

        transform.Translate(0, 0, speed * Time.deltaTime);
    }
}
