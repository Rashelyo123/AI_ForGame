using System.Collections.Generic;

public class Sequence : Node
{
    private List<Node> children;

    public Sequence(List<Node> children)
    {
        this.children = children;
    }

    public override NodeState Evaluate()
    {
        foreach (Node node in children)
        {
            var result = node.Evaluate();
            if (result != NodeState.Success)
                return result; // Failure or Running
        }
        return NodeState.Success;
    }
}
