using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FollowPAth : MonoBehaviour
{
   public WPManager wpManager;
    public TMP_Dropdown waypointDropdown;
    public float speed = 5.0f;
    public float rotationSpeed = 2.0f;
    public float accuracy = 1.0f;

    private GameObject currentNode;
    private int currentPathIndex = 0;
    private bool isMoving = false;

    void Start()
    {
        // 1. Vul de dropdown automatisch met de namen van de waypoints
        waypointDropdown.ClearOptions();
        List<string> options = new List<string>();
        
        foreach (GameObject wp in wpManager.waypoints)
        {
            options.Add(wp.name);
        }
        waypointDropdown.AddOptions(options);

        // Luister naar veranderingen in de dropdown
        waypointDropdown.onValueChanged.AddListener(delegate { GoToSelectedWaypoint(); });

        // Startpositie bepalen (dichtstbijzijnde waypoint)
        currentNode = FindClosestWaypoint();
    }

    void GoToSelectedWaypoint()
    {
        int index = waypointDropdown.value;
        GameObject target = wpManager.waypoints[index];

        // Bereken het pad met jouw A* algoritme
        // We starten vanaf het punt waar de tank het laatst was (currentNode)
        bool pathFound = wpManager.graph.AStar(currentNode, target);

        if (pathFound)
        {
            currentPathIndex = 0;
            isMoving = true;
        }
    }

    void LateUpdate()
    {
        if (!isMoving || wpManager.graph.pathList.Count == 0) return;

        // Pak het huidige doel uit de pathList
        GameObject goal = wpManager.graph.pathList[currentPathIndex].getID();

        // Rotatie naar het doel
        Vector3 lookAtGoal = new Vector3(goal.transform.position.x, transform.position.y, goal.transform.position.z);
        Vector3 direction = lookAtGoal - transform.position;

        if (direction.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);
        }

        // Bewegen
        transform.Translate(0, 0, speed * Time.deltaTime);

        // Check of we er zijn
        if (Vector3.Distance(transform.position, goal.transform.position) < accuracy)
        {
            currentNode = goal; // Update waar we nu zijn
            currentPathIndex++;

            if (currentPathIndex >= wpManager.graph.pathList.Count)
            {
                isMoving = false;
            }
        }
    }

    GameObject FindClosestWaypoint()
    {
        GameObject closest = null;
        float distance = Mathf.Infinity;
        foreach (GameObject wp in wpManager.waypoints)
        {
            float diff = (wp.transform.position - transform.position).sqrMagnitude;
            if (diff < distance)
            {
                closest = wp;
                distance = diff;
            }
        }
        return closest;
    }
}
