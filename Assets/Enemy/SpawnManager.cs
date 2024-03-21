using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : MonoBehaviour
{

    int mazeAreaMask;
    public Recursive obj;
    public GameObject enemyPrefab;
    public GameObject WayPointPrefab;
    public Transform mazeParent;
    public Transform enemyCollection;
    public List<Vector3> waypointsList;



    // void InstantiatingEnemy()
    // {
    //     for (int i = 0; i < obj.width; i++)
    //     {
    //         for (int j = 0; j < obj.depth; j++)
    //         {
    //             //less than 5% chance that the condition will be true. Meaning very less no. of enemies 
    //             if (Random.Range(0, 100) < 7)
    //             {
    //                 //A 0 means open space in the maze
    //                 if (obj.map[i, j] == 0)
    //                 {
    //                     // Calculate the position for the enemy based on coordinates and obj.scale
    //                     Vector3 enemyPosition = new Vector3(i * obj.scale, 0, j * obj.scale);

    //                     //Checking if the enemy is placed on the nav mesh correctly
    //                     /*
    //                         Hit contains the info about the position on the nav mesh
    //                         2f refers to the radius within which it will search
    //                     */
    //                     NavMeshHit hit;
    //                     if (NavMesh.SamplePosition(enemyPosition, out hit, 2f, NavMesh.AllAreas))
    //                     {
    //                         Instantiate(enemyPrefab, enemyPosition, Quaternion.identity, mazeParent.transform);
    //                         enemyPrefab.transform.position = hit.position;
    //                     }
    //                 }
    //             }
    //         }
    //     }
    //     //Forcing the spawnParent to move to this new pos, so that the children are positioned in the correct pos
    //     mazeParent.transform.localPosition = new Vector3(50.7f, 0f, -130.3f);
    // }

    public IEnumerator InstantiatingEnemy(int numberOfEnemiesToSpawn)
    {
        int enemiesSpawned = 0;

        while (enemiesSpawned < numberOfEnemiesToSpawn)
        {
            yield return new WaitForSeconds(2f); // Wait for 2 seconds before spawning the next enemy

            Vector3 enemyPosition = new Vector3(89.6f, 0, -100f);

            Instantiate(enemyPrefab, enemyPosition, Quaternion.identity, mazeParent.transform);

            enemiesSpawned++;
        }
    }    

    public void InstantiatingWaypoints(int i,int j)
    {
        // less than 5% chance that the condition will be true. Meaning very less no. of enemies 
        if (obj.map[i, j] == 0)
        {
            {
                
                // Calculate the position for the enemy based on coordinates and obj.scale
                Vector3 WayPointPosition = new Vector3(i * obj.scale, 0, j * obj.scale);

                //Checking if the enemy is placed on the nav mesh correctly
                /*
                    Hit contains the info about the position on the nav mesh
                    2f refers to the radius within which it will search
                */
                NavMeshHit hit;
                if (NavMesh.SamplePosition(WayPointPosition, out hit, 2f, NavMesh.AllAreas))
                {
                    Instantiate(WayPointPrefab, WayPointPosition, Quaternion.identity, mazeParent.transform);
                    WayPointPrefab.transform.position = hit.position;
                    waypointsList.Add(WayPointPosition);
                }

                
            }
        }
    }
}




