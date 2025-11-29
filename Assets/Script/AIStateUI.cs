using UnityEngine;
using TMPro;

public class AIStateUI : MonoBehaviour
{
    public GuardAI ai;
    public TMP_Text stateText;

    void Update()
    {
        if (ai == null) return;

        switch (ai.currentState)
        {
            case GuardAI.State.Patrol:
                stateText.text = "<color=green>PATROL</color>";
                break;

            case GuardAI.State.Chase:
                stateText.text = "<color=red>CHASE</color>";
                break;

            case GuardAI.State.Search:
                stateText.text = "<color=yellow>SEARCH</color>";
                break;
        }
    }
}
