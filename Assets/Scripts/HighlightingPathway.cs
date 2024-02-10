// using System.Collections;
// using System.Collections.Generic;
// using System.Drawing;
// using Unity.VisualScripting;
// using UnityEngine;
// using UnityEngine.AI;

// public class HighlightingPathway : MonoBehaviour
// {
//     public Material highlightMaterial; // Set this to a material for 
//     public GameObject MainPlayer;

//     private NavMeshAgent navMeshAgent;
//     private Material originalMaterial;

//     bool isFinding = false;
//     float time = 0f;

//     // LineRenderer lineRenderer;

//     //Trying with startLocation. If works will use end location
//     Vector3 endLocation = new Vector3(89.6f,0f,-100f); 

//     void Update()
//     {
//         if(isFinding)
//         {
//             time+=Time.deltaTime;
//             Debug.Log("Time = "+time);
//             if(time>=20f)
//             {
//                 navMeshAgent.isStopped = true;
//                 isFinding = false;
//                 time = 0f;
//             }
//         }
//     }

//     void Start()
//     {
//         navMeshAgent = GetComponent<NavMeshAgent>();
//         originalMaterial = GetComponent<Renderer>().material;

//     }

//     // Call this method when the hint button is clicked
//     public void OnHintButtonClicked()
//     {


//         // // Get the global position of agent.
//         Vector3 globalPosition = MainPlayer.transform.position;

//         this.transform.position = globalPosition;

//         Debug.Log("Main Player "+globalPosition);
//         Debug.Log("Agent Position "+this.transform.position);

//         // Highlight the NavMesh path
//         HighlightPath();

//     }

//     IEnumerator AgentPositioning()
//     {
//         yield return new WaitForSeconds(1f);
//         // Get the global position of MainPlayer.
//         Vector3 globalPosition = MainPlayer.transform.position;

//         // Find the parent object (if any).
//         Transform parentTransform = this.transform.parent;

//         if (parentTransform != null)
//         {
//             // Convert global position to local position of the parent object.
//             Vector3 localPosition = parentTransform.InverseTransformPoint(globalPosition);

//             // Set the local position of the object with the script.
//             this.transform.localPosition = localPosition;

//             Debug.Log("Main Player " + MainPlayer.transform.position);
//             Debug.Log("Agent Position " + this.transform.position);

//             yield return new WaitForSeconds(1f);

//             // Highlight the NavMesh path
//             HighlightPath();
//         }

//     }

//     void HighlightPath()
//     {
//         isFinding = true;
//         navMeshAgent.isStopped = false;
//         navMeshAgent.SetDestination(endLocation);


//         // // Apply highlight material
//         // GetComponent<Renderer>().material = highlightMaterial;

//         // // Restore the original material
//         // GetComponent<Renderer>().material = originalMaterial;
//     }



//     // void SetPathway()
//     // {
//     //     Vector3 GameObjectCoor = this.transform.position;

//     //     lineRenderer = GetComponent<LineRenderer>();
//     //     if(lineRenderer!=null)
//     //     {
//     //         //Accessing the Current position of the line renderer
//     //        Vector3[] existingPos = new Vector3[lineRenderer.positionCount];
//     //        lineRenderer.GetPositions(existingPos);

//     //        //Add the current position to the line renderer pos
//     //        Vector3[] newPos = new Vector3[existingPos.Length+1];
//     //        existingPos.CopyTo(newPos,0);

//     //        if(newPos[newPos.Length-2]!=GameObjectCoor || GameObjectCoor!=Vector3.zero)
//     //        newPos[newPos.Length-1] = GameObjectCoor;

//     //        //Update line renderer pos
//     //        lineRenderer.positionCount = newPos.Length;
//     //        lineRenderer.SetPositions(newPos);
//     //     }
//     //     else
//     //     {
//     //         Debug.Log("FAULT");
//     //     }
//     // }
// }

using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class HighlightingPathway : MonoBehaviour
{
    public Vector3 target; 
    //public GameObject Player;

    NavMeshAgent navMeshAgent;
    LineRenderer lineRenderer;
    bool isFinding = false;
    float time = 0f;
    GameObject navMeshSurfaceObject;
    NavMeshSurface navMeshSurface;

    void Start()
    {
        //Finds Maze game object
        navMeshSurfaceObject = GameObject.Find("Maze");
        navMeshAgent = GetComponent<NavMeshAgent>();

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    void Update()
   {
        if(isFinding)
        {
            time+=Time.deltaTime;
            Debug.Log("Time = "+time);
            if(time>=15f)
            {
                lineRenderer.enabled = false;
                isFinding = false;
                time = 0f;
            }
       }
    }

    // public void CalculateAndVisualizePath()
    // {
    //     lineRenderer.enabled = true;
    //     Vector3 playerPos = Player.transform.position;
    //     NavMeshPath path = new NavMeshPath();
    //     if (navMeshAgent.CalculatePath(target, path))
    //     {
    //         VisualizePath(path,playerPos);
    //     }
    // }

    public void CalculateAndVisualizePath()
    {
        lineRenderer.enabled = true;
        NavMeshPath path = new NavMeshPath();

        //Gets the nav mesh surface from the Maze game object
        navMeshSurface = navMeshSurfaceObject.GetComponent<NavMeshSurface>();
        navMeshSurface.BuildNavMesh();

        if (navMeshAgent.CalculatePath(target, path))
        {

            // Debug.DrawRay(this.transform.position,target,Color.red,10f);
            VisualizePath(path);
        }
        else
        {
            Debug.Log("Error");
        }
    }

    void VisualizePath(NavMeshPath path)
    {
        isFinding = true;
        // Visualize the path by drawing lines between waypoints
        Vector3[] waypoints = path.corners;

        lineRenderer.positionCount = waypoints.Length;
        lineRenderer.SetPositions(waypoints);


        // Draw line from player position to the first waypoint
        Debug.DrawLine(this.transform.position, waypoints[0], Color.red, 10f);

        //Create the line to move from ClosestWP till end
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            Debug.DrawLine(waypoints[i], waypoints[i + 1], Color.red, 10f);
        }

    }

    // void VisualizePath(NavMeshPath path,Vector3 startPos)
    // {
    //     isFinding = true;
    //     // Visualize the path by drawing lines between waypoints
    //     Vector3[] waypoints = path.corners;

    //     lineRenderer.positionCount = waypoints.Length;
    //     lineRenderer.SetPositions(waypoints);

    //     //Finding the closest waypoint index
    //     int ClosestWP=ClosestWaypoint(startPos,waypoints);

    //     // Draw line from player position to the first waypoint
    //     Debug.DrawLine(startPos, waypoints[ClosestWP], Color.red, 10f);

    //     //Create the line to move from ClosestWP till end
    //     for (int i = ClosestWP; i < waypoints.Length - 1; i++)
    //     {
    //         Debug.DrawLine(waypoints[i], waypoints[i + 1], Color.red, 10f);
    //     }

    // }

    // int ClosestWaypoint(Vector3 position,Vector3[] waypoints)
    // {
    //     float minDistance = float.MaxValue;
    //     int closestIndex = 0;

    //     for(int i=1;i<waypoints.Length-1;i++)
    //     {
    //         float distance = Vector3.Distance(position,waypoints[i]);
    //         if(distance<minDistance)
    //         {
    //             minDistance = distance;
    //             closestIndex = i;
    //         }
    //     }
    //     return closestIndex;
    // }
}

