
using UnityEngine;
public class ChaseSafeNode : Node
{
    BossAI boss;

    public ChaseSafeNode(BossAI boss)
    {
        this.boss = boss;
    }

    public override NodeState Evaluate()
    {
        float dist = Vector2.Distance(boss.transform.position, boss.player.position);

        if (dist > boss.safeDistance)
        {
            // Masih mengejar → Running
            boss.MoveTowardsPlayerSafe();
            return NodeState.Running;
        }

        // Sudah dekat → Berhenti mengejar dan izinkan Sequence lanjut
        return NodeState.Success;
    }
}
