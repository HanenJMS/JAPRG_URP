using TMPro;
using UnityEngine;

namespace GameLab.GridSystem
{
    public class PathFindingGridDebugObject : GridDebugObject
    {
        private PathNode pathNodeObject;
        [SerializeField] TextMeshPro gCostText;
        [SerializeField] TextMeshPro hCostText;
        [SerializeField] TextMeshPro fCostText;
        public override void SetGridObject(object gridObject)
        {
            base.SetGridObject(gridObject);
            pathNodeObject = gridObject as PathNode;
        }
        protected override void Update()
        {
            base.Update();
            gCostText.text = pathNodeObject.GetgCost().ToString();
            hCostText.text = pathNodeObject.GethCost().ToString();
            fCostText.text = pathNodeObject.GetfCost().ToString();
        }
    }

}
