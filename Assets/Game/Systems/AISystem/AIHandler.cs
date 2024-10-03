using GameLab.UnitSystem;
using UnityEngine;
namespace GameLab.AI
{
    public class AIHandler : MonoBehaviour
    {
        IAIState combatAI;
        IAIState followerAI;
        IAIState currentState;
        Unit unit;
        private void Awake()
        {
            unit = GetComponent<Unit>();
            combatAI = GetComponent<CombatAI>();
            followerAI = GetComponent<FollowerAI>();
        }

        private void Start()
        {
            //followerAI.ActivateState();
        }
        public void SetCurrentAIState(IAIState state)
        {
            if (currentState == state) return;
            if (currentState != null)
            {
                currentState.CancelState();
            }
            currentState = state;
            currentState.ActivateState();
        }
        private void LateUpdate()
        {
            if (unit.GetHealthHandler().IsDead())
            {
                return;
            }
            if (currentState == null) return;
            if (currentState.IsRunning()) return;
            SetCurrentAIState(followerAI);
        }
    }
}

