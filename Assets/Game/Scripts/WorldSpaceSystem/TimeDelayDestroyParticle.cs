using UnityEngine;

namespace GameLab.WorldSpaceComponents
{
    public class TimeDelayDestroyParticle : MonoBehaviour
    {
        [SerializeField] ParticleSystem ps;
        float time = 0f;
        [SerializeField] float duration = 5f;

        private void OnEnable()
        {
            if (ps == null)
            {
                Destroy(this.gameObject, duration);
            }
            if (ps != null)
            {
                Destroy(this.gameObject, ps.main.duration);
            }
        }
    }
}

