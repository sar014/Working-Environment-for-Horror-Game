using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.Analytics;
using System.IO;

//This class stores the markers and the nodes 
public class PathMarker
{
    public MapLocation location;
    public float G;
    public float H;
    public float F;
    public GameObject marker;
    public PathMarker parent;

    //Constructor for PathMarker class
    public PathMarker(MapLocation l,float g,float h,float f,GameObject marker,PathMarker p)
    {
        location = l;
        G=g;
        H=h;
        F= f;
        this.marker = marker;
        parent = p;
    }

    //Overriding the equals method to check if one pathmarker is same as another
    public override bool Equals(object obj)
    {
        if((obj==null)|| !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            return location.Equals(((PathMarker)obj).location);
        }
    }

    public override int GetHashCode()
    {
        return 0;
    }



}

//This will actually run the A* star Algo
public class FindPathAStar : MonoBehaviour
{
    //Get hold of the Maze
    public Maze maze;

    public Material closedMaterial;
    public Material openMaterial;

    List<PathMarker> open = new List<PathMarker>();
    List<PathMarker> closed = new List<PathMarker>();

    //Used to Display the prefabs when the code runs
    public GameObject start;
    public GameObject end;
    public GameObject pathP;

    PathMarker goalNode;
    PathMarker startNode;

    PathMarker lastPos;
    bool done = false;

    void RemoveAllMarkers()
    {
        GameObject[] markers = GameObject.FindGameObjectsWithTag("marker");
        foreach(GameObject m in markers)
        {
            Destroy(m);
        }
    }

    void BeginSearch()
    {
        done = false;
        RemoveAllMarkers();

        //List for storing empty spaces in the maze
        List<MapLocation> locations = new List<MapLocation>();
        for(int z=1;z<maze.depth-1;z++)
            for(int x = 1;x<maze.width-1;x++)
            {
                //1 means walls and 0 means space (coded in the maze script)
                if(maze.map[x,z]!=1)
                {
                    locations.Add(new MapLocation(x,z));
                }
            }
        
        //Shuffling the list
        locations.Shuffle();

        // * maze.scale because the maze is also scaled to a certain value
        Vector3 startLocation = new Vector3(locations[0].x * maze.scale,0,locations[0].z * maze.scale);
        //Calling Path Marker constructor
        startNode = new PathMarker(new MapLocation(locations[0].x,locations[0].z),0,0,0,
                            Instantiate(start,startLocation,Quaternion.identity),null);

        
        Vector3 goalLocation = new Vector3(locations[1].x * maze.scale,0,locations[1].z * maze.scale);
        //Calling Path Marker constructor
        goalNode = new PathMarker(new MapLocation(locations[1].x,locations[1].z),0,0,0,
                            Instantiate(end,goalLocation,Quaternion.identity),null);


        open.Clear();
        closed.Clear();
        open.Add(startNode);
        lastPos = startNode;
    }

    void Search(PathMarker thisNode)
    {
        if(thisNode==null) return;
        
        if(thisNode.Equals(goalNode)){done = true;return;}//goal node has been found

        //Iterating throigh MapLocation objects in "directions" list 
        foreach (MapLocation dir in maze.directions)
        {
            //Calculating neighbour by adding location of thisNode
            MapLocation neighbour = dir+thisNode.location;
            if(maze.map[neighbour.x,neighbour.z]==1) continue; //Wall
            if(neighbour.x<1 || neighbour.x>=maze.width || neighbour.z<1 || neighbour.z>=maze.depth) continue; //Outside maze range
            if(IsClosed(neighbour))continue;//If already in closed list

            float G = Vector2.Distance(thisNode.location.ToVector(),neighbour.ToVector())+ thisNode.G;//From start Node
            float H = Vector2.Distance(neighbour.ToVector(),goalNode.location.ToVector());//Heuristic: to goal Node
            float F = G+H;

            GameObject pathBlock = Instantiate(pathP,new Vector3(neighbour.x * maze.scale,0,neighbour.z * maze.scale),Quaternion.identity);

            //Accessing the Text Mesh attached to the game object and storing in a list
            TextMesh[] values = pathBlock.GetComponentsInChildren<TextMesh>();
            values[0].text = "G" + G.ToString("0.00");
            values[1].text = "H" + H.ToString("0.00");
            values[2].text = "F" + F.ToString("0.00");

            if(!UpdateMarker(neighbour,G,H,F,thisNode))
                open.Add(new PathMarker(neighbour,G,H,F,pathBlock,thisNode));
        }

        /*
            Ordering the F values in ascending order so that we get lowest f values. Then ordering by H values
            if we have more than one lower f values
        */
        open = open.OrderBy(p => p.F).ThenBy(n =>n.H).ToList<PathMarker>();

        //Accessing the value with lowest f value
        PathMarker pm = (PathMarker)open.ElementAt(0);

        //Addding the above value to the closed list
        closed.Add(pm);

        //Removing the above element from the open list
        open.Remove(pm);    
        //Applying material
        pm.marker.GetComponent<Renderer>().material = closedMaterial;

        //Next node to search from 
        lastPos = pm;
    }

    //Checking if the node is present in the open/closed list.
    //If in open list, then accessing its G,H and F values for further calculation
    bool UpdateMarker(MapLocation pos,float g,float h,float f,PathMarker prt)
    {
        foreach(PathMarker p in open)
        {
            if(p.location.Equals(pos))
            {
                p.G = g;
                p.H = h;
                p.F = f;
                return true;
            }
        }
        return false;
    }

    bool IsClosed(MapLocation marker)
    {
        foreach(PathMarker p in closed)
        {
            if(p.location.Equals(marker)) return true;
        }
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void GetPath()
    {
    
        RemoveAllMarkers();
        PathMarker begin = lastPos;

        //Backtracking from lastPos to startNode
        while(!startNode.Equals(begin) && begin!=null)
        {
            Instantiate(pathP,new Vector3(begin.location.x * maze.scale,0,begin.location.z * maze.scale),
                Quaternion.identity);
            begin = begin.parent;
        }

        //Visual Representation of the startNode
        Instantiate(pathP,new Vector3(startNode.location.x * maze.scale,0,startNode.location.z * maze.scale),
            Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        BeginSearch();

        if(Input.GetKeyDown(KeyCode.C) && !done) Search(lastPos);

        if(Input.GetKeyDown(KeyCode.M)) GetPath();
    }
}
