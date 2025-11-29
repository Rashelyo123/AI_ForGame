public class CheckHPNode : Node
{
    private BossAI boss;
    private float threshold;

    public CheckHPNode(BossAI boss, float threshold)
    {
        this.boss = boss;
        this.threshold = threshold;
    }

    public override NodeState Evaluate()
    {
        return boss.health <= threshold ? NodeState.Success : NodeState.Failure;
    }
}
