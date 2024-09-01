using TMPro;
using UnityEngine;
namespace GameLab.GridSystem
{
    public class GridDebugObject : MonoBehaviour
    {
        object gridObject;
        [SerializeField] TextMeshPro GridPosition;
        public virtual void SetGridObject(object gridObject)
        {
            this.gridObject = gridObject;
            
        }
        protected virtual void Update()
        {
            GridPosition.text = gridObject.ToString();
        }
    }
}

