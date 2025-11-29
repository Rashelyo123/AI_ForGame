public class NormalAttackNode : Node
{
    BossAI boss;

    public NormalAttackNode(BossAI boss)
    {
        this.boss = boss;
    }

    public override NodeState Evaluate()
    {
        if (boss.PlayerInAttackRange())
        {
            boss.NormalAttack();
            return NodeState.Success;
        }
        return NodeState.Failure;
    }
}
