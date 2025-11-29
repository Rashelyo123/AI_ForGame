using System.Collections.Generic;

public class Selector : Node
{
    private List<Node> children;

    public Selector(List<Node> children)
    {
        this.children = children;
    }

    public override NodeState Evaluate()
    {
        foreach (Node node in children)
        {
            var result = node.Evaluate();
            if (result != NodeState.Failure)
                return result; // Success or Running
        }
        return NodeState.Failure;
    }
}
