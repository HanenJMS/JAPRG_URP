using System;
using Unity.VisualScripting;
using UnityEngine;

namespace GameLab.ResourceSystem
{
    public class HealthHandler : MonoBehaviour
    {
        [SerializeField] UnitResource resource;
        [SerializeField] bool isDead = false;
        public Action onDead;
        public Action onResourceChange;
        private void Start()
        {
            resource.SetCurrent(resource.GetMax());
            onResourceChange?.Invoke();
        }
        public void AddCurrent(int current)
        {

            resource.Add(current);
            onResourceChange?.Invoke();
        }
        public void RemoveFromCurrent(int value)
        {
            resource.Remove(value);
            onResourceChange?.Invoke();
            if (GetCurrent() <= 0)
            {
                isDead = true;
                onDead?.Invoke();
            }
        }
        public bool IsDead()
        {
            return isDead;
        }
        public int GetCurrent()
        {
            return resource.GetCurrent();
        }
        public float GetCurrentRatio()
        {
            return resource.GetCurrentRatio();
        }
        public void InitializeResource(int current, int max)
        {
            resource.SetCurrent(current);
            resource.SetMax(max);
        }
        public override string ToString()
        {
            return resource.ToString();
        }
    }
}
