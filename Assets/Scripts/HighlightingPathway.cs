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
        
        // sets the number of positions in a LineRenderer component equal to the number of waypoints in the path.
        lineRenderer.positionCount = waypoints.Length;

        //sets the positions of the LineRenderer to the positions of the waypoints in the path.
        lineRenderer.SetPositions(waypoints);


        // Draw line from player position to the first waypoint
        Debug.DrawLine(this.transform.position, waypoints[0], Color.red, 10f);

        //Create the line to move from ClosestWP till end
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            Debug.DrawLine(waypoints[i], waypoints[i + 1], Color.red, 10f);
        }

    }

    
}

