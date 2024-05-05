using UnityEngine;
using UnityEngine.AI;
namespace GameLab.Animation
{
    public class UnitAnimationHandler : MonoBehaviour
    {
        Animator animator;
        NavMeshAgent agent;
        float speed = 0f;
        private void Awake()
        {
            animator = GetComponent<Animator>();
            agent = GetComponentInParent<NavMeshAgent>();
        }

        private void Update()
        {
            Vector3 velocity = agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            speed = localVelocity.z;
            animator.SetFloat("forwardSpeed", speed);
        }
    }
}

