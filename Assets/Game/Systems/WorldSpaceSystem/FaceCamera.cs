using UnityEngine;

namespace GameLab.WorldSpaceComponents
{
    public class FaceCamera : MonoBehaviour
    {
        private void LateUpdate()
        {
            this.transform.forward = Camera.main.transform.forward;
        }
    }
}

