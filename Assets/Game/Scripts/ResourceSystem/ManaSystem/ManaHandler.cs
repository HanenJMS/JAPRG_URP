using System;
using UnityEngine;

namespace GameLab.ResourceSystem
{
    public class ManaHandler : MonoBehaviour
    {
        [SerializeField] UnitResource resource;
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
