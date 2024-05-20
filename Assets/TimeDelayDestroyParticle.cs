using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDelayDestroyParticle : MonoBehaviour
{
    [SerializeField] ParticleSystem ps;
    float time = 0f;

    private void LateUpdate()
    {
        time += Time.deltaTime;
        if(time > ps.main.duration)
        {
            Destroy(this.gameObject);
        }
    }
}
