using UnityEngine;
namespace GameLab.ResourceSystem
{
    [System.Serializable]
    public class UnitResource
    {
        [SerializeField] int maxResource = 100;
        [SerializeField] int currentResource = 0;
        public void Add(int resource)
        {
            Mathf.Clamp(currentResource += resource, 0, maxResource);
            if (currentResource <= 0)
            {
                currentResource = 0;
            }
        }
        public void Remove(int resource)
        {
            Mathf.Clamp(currentResource -= resource, 0, maxResource);
            if (currentResource <= 0)  
            {
                currentResource = 0;
            }
        }

        public float GetCurrentRatio()
        {
            return Mathf.Clamp01((float)currentResource / maxResource);
        }
        public int GetCurrent()
        {
            return currentResource;
        }
        public int GetMax()
        {
            return maxResource;
        }
        public void SetCurrent(int resource)
        {
            currentResource = resource;
        }
        public void SetMax(int resource)
        {
            maxResource = resource;
        }
        public override string ToString()
        {
            return $"{currentResource}/{maxResource}";
        }
    }
}
