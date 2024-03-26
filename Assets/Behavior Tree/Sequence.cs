using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    public Sequence(string n)
    {
        name = n;
    }

    // The below process is looping
    public override Status Process()
    {

        Status childstatus = children[currentChild].Process();

        //If the process is still running
        if(childstatus == Status.RUNNING) return Status.RUNNING;

        //if the process has failed
        if(childstatus == Status.FAILURE) return childstatus;

        //If the process has not failed or is not running
        currentChild++;

        //if the no. of children nodes has reached to the end
        if(currentChild>=children.Count)
        {
            currentChild = 0;
            return Status.SUCCESS;
        }
        return Status.RUNNING;
    }
}
