using UnityEngine;
using System.Collections.Generic;

public class BossAIController : MonoBehaviour
{
    private Node root;
    private BossAI boss;

    void Start()
    {
        boss = GetComponent<BossAI>();

        // Phase 2
        var phase2 = new Sequence(new List<Node>()
        {
            new CheckHPNode(boss, 50f),
            new DetectPlayerNode(boss),
            new SpecialAttackNode(boss)
        });

        // Phase 1
        var phase1 = new Sequence(new List<Node>()
        {
            new DetectPlayerNode(boss),
            new ChaseSafeNode(boss),
            new NormalAttackNode(boss)
        });

        // Root Selector
        root = new Selector(new List<Node>()
        {
            phase2,
            phase1
        });
    }

    void Update()
    {
        root.Evaluate();
    }
}
