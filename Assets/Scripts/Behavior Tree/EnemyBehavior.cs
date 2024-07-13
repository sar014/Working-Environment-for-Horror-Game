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
    bool canSeePlayer;
    BehaviorTree tree;
    int scale = 0;
    NavMeshAgent agent;
    List<Vector3> WayPointList;
    Node.Status treeStatus = Node.Status.RUNNING;

    GameObject go;
    GameObject Recur_obj;

    public Vector3 exitLocation;
    public Vector3 movDirection;

    //Field of View 
    public float radius;
    [Range(0,360)]
    public float angle;
    public GameObject playerRef;
    public LayerMask targetMask;
    public LayerMask obstructionMask;

    
    
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
        
        go = GameObject.Find("Maze");
        Recur_obj = GameObject.Find("Maze");
        WayPointList = go.GetComponent<SpawnManager>().waypointsList;
        int scale = Recur_obj.GetComponent<Recursive>().scale;

        agent = this.GetComponent<NavMeshAgent>();


        tree = new BehaviorTree();
        //Node enemy = new Node("Enemy Behavior"); //Should be sequence
        // Leaf patrol = new Leaf("Patrol Behavior",Patrol);
        Leaf InRange = new Leaf("If Target is in Range",CanSeeTarget);
        Leaf Attack  = new Leaf("Attack player",Seek);

        
        Sequence target = new Sequence("Targeting Players");
        target.AddChild(InRange);
        target.AddChild(Attack);

        // enemy.AddChild(patrol);
        // enemy.AddChild(target);
        tree.AddChild(target);

        tree.PrintTree();
        Patrol();

    }

    public void Patrol()
    {
        int WayPointIndex = UnityEngine.Random.Range(0, WayPointList.Count);
        agent.SetDestination(WayPointList[WayPointIndex] * scale);

        StartCoroutine(RepeatPatrol(10f));
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

    public Node.Status Seek()
    {
        if(canSeePlayer)
        {
            agent.SetDestination(playerRef.transform.position);
            return Node.Status.RUNNING;
        }
        this.GetComponent<Renderer>().material.color = Color.gray;
        return Node.Status.FAILURE;
    }

    public Node.Status CanSeeTarget()
    {
        if(canSeePlayer){ 
            this.GetComponent<Renderer>().material.color = Color.red;
            return Node.Status.SUCCESS;
        }
        
        return Node.Status.FAILURE;
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();

            if (canSeePlayer)
            CanSeeTarget();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }

    void Update()
    {
        if(treeStatus!=Node.Status.SUCCESS)
            treeStatus = tree.Process();
    }


}
