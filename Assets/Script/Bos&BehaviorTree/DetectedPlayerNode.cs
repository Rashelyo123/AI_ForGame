public class DetectPlayerNode : Node
{
    private BossAI boss;

    public DetectPlayerNode(BossAI boss)
    {
        this.boss = boss;
    }

    public override NodeState Evaluate()
    {
        if (boss.CanSeePlayer())
            return NodeState.Success;
        return NodeState.Failure;
    }
}
