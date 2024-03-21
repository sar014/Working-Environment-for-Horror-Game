using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree : Node
{
    public BehaviorTree()
    {
        name = "Tree";
    }

    public BehaviorTree(string n)
    {
        name = n;
    }

    public override Status Process()
    {
        return children[currentChild].Process();
    }
}
