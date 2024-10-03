using UnityEngine;

public class TargetHandler : MonoBehaviour
{
    GameObject target;

    public void SetCurrentTarget(GameObject target)
    {
        this.target = target;
    }
}
