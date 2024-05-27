using System.Collections;
using System.Collections.Generic;
using UnityEngine.VFX;
using UnityEngine;
namespace GameLab.UnitSystem.AbilitySystem
{
    public class AbilityProjectileVFX : MonoBehaviour
    {
        private Vector3 projectileDir;
        public GameObject FX_Hit;

        VisualEffect FX_Projectile;
        VisualEffect FX_ProjectileTail;

        AudioSource SFX_Projectile;

        private void Start()
        {
            //FX_Projectile = gameObject.transform.GetChild(0).GetComponent<VisualEffect>();
            //FX_ProjectileTail = gameObject.transform.GetChild(1).GetComponent<VisualEffect>();
            SFX_Projectile = gameObject.GetComponent<AudioSource>();
        }

        public void SpawnImpactVFX(Collider target)
        {
            Instantiate(FX_Hit, target.transform.position, this.transform.rotation);

            //Destroy(FX_Projectile);
            //FX_ProjectileTail.Stop();
            if (SFX_Projectile == null) return;
            SFX_Projectile.Stop();
        }
    }
}

