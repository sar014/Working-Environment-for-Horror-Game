using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
   public Selector(string n)
   {
        name = n;
   }

    public override Status Process()
    {
        Status childStatus = children[currentChild].Process();

        //if the process is still running
        if(childStatus == Status.RUNNING) return Status.RUNNING;

        //if the process succeeds return success
        if(childStatus == Status.SUCCESS)
        {
            currentChild = 0;
            return Status.SUCCESS;
        }

        //otherwise the process has failed, move to the next child
        currentChild++;
        if(currentChild>=children.Count)
        {
            currentChild = 0;
            return Status.FAILURE;
        }

        return Status.RUNNING;
    }
}
