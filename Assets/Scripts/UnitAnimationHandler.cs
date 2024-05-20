using System;
using UnityEngine;
using UnityEngine.AI;
namespace GameLab.Animation
{
    public class UnitAnimationHandler : MonoBehaviour
    {
        Animator animator;
        RuntimeAnimatorController defaultAnimationController;
        AnimatorOverrideController currentOverrider;
        NavMeshAgent agent;
        float speed = 0f;
        private void Awake()
        {
            animator = GetComponent<Animator>();
            agent = GetComponentInParent<NavMeshAgent>();
            defaultAnimationController = animator.runtimeAnimatorController;
        }

        private void Update()
        {
            HandleMovementAnimation();
        }

        private void HandleMovementAnimation()
        {
            Vector3 velocity = agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            speed = localVelocity.z;
            animator.SetFloat("forwardSpeed", speed);
        }
        public void SetAbilityAnimationOverrider(AnimatorOverrideController newOverride)
        {
            animator.runtimeAnimatorController = newOverride;
        }
        public void SetAnimationOverrideController(AnimatorOverrideController newOverride)
        {
            if (newOverride == null) return;
            currentOverrider = newOverride;
            animator.runtimeAnimatorController = currentOverrider;
        }
        public void ReturnToOverrider()
        {
            if(currentOverrider != null)
            {
                animator.runtimeAnimatorController = currentOverrider;
            }
            else
            {
                SetDefaultAnimationController();
            }
        }
        public void SetDefaultAnimationController()
        {
            currentOverrider = null;
            animator.runtimeAnimatorController = defaultAnimationController;
        }
        public void SetTrigger(string trigger)
        {
            animator.SetTrigger(trigger);
        }

        public void ResetTrigger(string trigger)
        {
            animator.ResetTrigger(trigger);
        }
    }
}

