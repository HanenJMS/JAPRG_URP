using UnityEngine;

namespace GameLab.InteractableSystem
{
    public class Interactable : MonoBehaviour
    {
        public virtual void Interact(object interaction)
        {

        }
        public virtual Transform GetCurrentWorldTransform()
        {
            return this.transform;
        }
    }

}
