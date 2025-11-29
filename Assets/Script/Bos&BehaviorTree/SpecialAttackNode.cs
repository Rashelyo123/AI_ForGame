public class SpecialAttackNode : Node
{
    BossAI boss;

    public SpecialAttackNode(BossAI boss)
    {
        this.boss = boss;
    }

    public override NodeState Evaluate()
    {
        boss.SpecialAttack();
        return NodeState.Success;
    }
}
