using GameLab.PartySystem;
using GameLab.UnitSystem;
using GameLab.UnitSystem.ActionSystem;
using UnityEngine;

namespace GameLab.AI
{
    public class FollowerAI : MonoBehaviour, IAIState
    {
        [SerializeField] Unit Leader;
        MoveAction moveAction;
        Unit unit;
        ActionHandler actionHandler;
        AIHandler aiHandler;
        PartyHandler partyHandler;
        public void SetLeader(Unit Leader)
        {
            this.Leader = Leader;
        }
        private void Awake()
        {
            unit = GetComponent<Unit>();
            aiHandler = GetComponent<AIHandler>();
        }
        private void Start()
        {
            actionHandler = unit.GetActionHandler();
            partyHandler = unit.GetPartyHandler();
            moveAction = unit.GetActionHandler().GetActionType<MoveAction>();
            partyHandler.onLeaderChanged += OnPartyLeaderChanged;


        }
        void OnPartyLeaderChanged()
        {
            Leader = partyHandler.GetLeader();
            if (!isActive)
            {
                aiHandler.SetCurrentAIState(this);
            }
        }
        bool isActive = false;
        private void LateUpdate()
        {
            if (!isActive) return;
            if (unit.GetHealthHandler().IsDead())
            {
                CancelState();
                return;
            }
            if (Leader != null)
            {
                if (Vector3.Distance(this.transform.position, Leader.transform.position) > 1.5f)
                {
                    moveAction.ExecuteOnTarget(Leader);
                }
                else
                {
                    moveAction.Cancel();
                }
            }
        }

        public void ActivateState()
        {
            isActive = true;
        }

        public void CancelState()
        {
            isActive = false;
        }

        public bool IsRunning()
        {
            return isActive;
        }
    }
}

