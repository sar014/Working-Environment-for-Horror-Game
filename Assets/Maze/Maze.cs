using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//For NavMesh
using Unity.AI.Navigation;
using UnityEngine.UIElements;

public class MapLocation       
{
    public int x;
    public int z;

    public MapLocation(int _x, int _z)
    {
        x = _x;
        z = _z;
    }

    public Vector2 ToVector()
    {
        return new Vector2(x, z);
    }

    public static MapLocation operator +(MapLocation a, MapLocation b)
       => new MapLocation(a.x + b.x, a.z + b.z);

    public override bool Equals(object obj)
    {
        if((obj==null)|| !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            return x == ((MapLocation)obj).x && z==((MapLocation)obj).z;
        }
    }

    public override int GetHashCode()
    {
        return 0;
    }

}

public class Maze : MonoBehaviour
{
    public List<MapLocation> directions = new List<MapLocation>() {
                                            new MapLocation(1,0),
                                            new MapLocation(0,1),
                                            new MapLocation(-1,0),
                                            new MapLocation(0,-1) };
    public int width = 30; //x length
    public int depth = 30; //z length
    public bool hasEntered = false;
    public byte[,] map;
    public int scale = 6;
    GameObject wall;
    public SpawnManager manager;

    // Start is called before the first frame update
    void Awake()
    {
        InitialiseMap();
        Generate();
        DrawMap();
        GetComponent<NavMeshSurface>().BuildNavMesh();
        StartCoroutine(manager.InstantiatingEnemy(5));
        // StartCoroutine(DelayedNavMeshBuild(1.0f));
    }

    void InitialiseMap()
    {
        map = new byte[width,depth];
        for (int z = 0; z < depth; z++)
            for (int x = 0; x < width; x++)
            {
                    map[x, z] = 1;     //1 = wall  0 = corridor
            }
    }

    public virtual void Generate()
    {
        for (int z = 0; z < depth; z++)
            for (int x = 0; x < width; x++)
            {
               if(Random.Range(0,100) < 50)
                 map[x, z] = 0;     //1 = wall  0 = corridor
            }
    }

    void DrawMap()
    {
        int x,z;
        GameObject mazeParent = GameObject.Find("Maze");
        for (z = 0; z < depth; z++)
        {
            for (x = 0; x < width; x++)
            {
                
                if (map[x, z] == 1)
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    
                    if (pos.x == 48 && pos.z == 144 || pos.x == 48 && pos.z == 138)
                    {
                        //Actual position : x=50.8,y=0,z=44
                        // If the position matches, skip creating a cube at this position
                        continue;
                    }

                    if ((x == 15 && z == 0 || x==15 && z==1 || x==14 && z==0))
                    {
                        //Actual Position :89.6,0,-100
                        // If the position matches, skip creating a cube at this position
                        continue;
                    }
                    
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall.transform.localScale = new Vector3(scale, scale, scale);
                    wall.transform.position = pos;
                    wall.layer = 7;
                    
                    wall.transform.SetParent(mazeParent.transform);
                }
                else if(map[x,z]==0)
                {
                    if(Random.Range(0,100)<15)
                    {
                        manager.InstantiatingWaypoints(x,z);
                    }
                    else if(Random.Range(0,100)<10)
                    {
                        manager.InstantiateWeapons(x,z);
                    }
   
                }
                
            }
        }
        mazeParent.transform.localPosition = new Vector3(3f,0f,-100f);
    }

    // IEnumerator DelayedNavMeshBuild(float delay)
    // {
    //     // Wait for the specified delay
    //     yield return new WaitForSeconds(delay);

    //     // After the delay, build the NavMesh
    //     GetComponent<NavMeshSurface>().BuildNavMesh();
    // }

    public int CountSquareNeighbours(int x, int z)
    {
        int count = 0;
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
        if (map[x - 1, z] == 0) count++;
        if (map[x + 1, z] == 0) count++;
        if (map[x, z + 1] == 0) count++;
        if (map[x, z - 1] == 0) count++;
        return count;
    }

    public int CountDiagonalNeighbours(int x, int z)
    {
        int count = 0;
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
        if (map[x - 1, z - 1] == 0) count++;
        if (map[x + 1, z + 1] == 0) count++;
        if (map[x - 1, z + 1] == 0) count++;
        if (map[x + 1, z - 1] == 0) count++;
        return count;
    }

    public int CountAllNeighbours(int x, int z)
    {
        return CountSquareNeighbours(x,z) + CountDiagonalNeighbours(x,z);
    }
}
