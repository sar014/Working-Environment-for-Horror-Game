using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyBehavior : MonoBehaviour
{
   
    BehaviorTree tree;
    int scale = 0;
    NavMeshAgent agent;
    List<Vector3> WayPointList;
    Node.Status treeStatus = Node.Status.RUNNING;

    GameObject go;
    GameObject Recur_obj;

    public Vector3 exitLocation;
    public Vector3 movDirection;
    public SpawnManager manager = new SpawnManager();

    public Recursive obj = new Recursive();

    
    
    void Start()
    {
        go = GameObject.Find("Maze");
        Recur_obj = GameObject.Find("Maze");
        WayPointList = go.GetComponent<SpawnManager>().waypointsList;
        int scale = Recur_obj.GetComponent<Recursive>().scale;

        agent = this.GetComponent<NavMeshAgent>();

        //UpdateDestination();

        tree = new BehaviorTree();
        // Node enemy = new Node("Enemy Behavior"); Should be sequence
        Leaf patrol = new Leaf("Patrol Behavior",Patrol);
        tree.AddChild(patrol);

    }

    public Node.Status Patrol()
    {
        int WayPointIndex = UnityEngine.Random.Range(0, WayPointList.Count);
        agent.SetDestination(WayPointList[WayPointIndex] * scale);

        StartCoroutine(RepeatPatrol(10f));
   
        return Node.Status.SUCCESS;
    }

   
    void Update()
    {
        if(treeStatus!=Node.Status.SUCCESS)
            treeStatus = tree.Process();
    }

    IEnumerator RepeatPatrol(float interval)
    {
        while (true)
        {
            int WayPointIndex = UnityEngine.Random.Range(0, WayPointList.Count);
            agent.SetDestination(WayPointList[WayPointIndex]);

            yield return new WaitForSeconds(interval);
        }
    }

}
