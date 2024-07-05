using UnityEngine;

namespace GameLab.InteractableSystem
{
    public abstract class Interactable : MonoBehaviour
    {
        public abstract void Interact(object interaction);
        public virtual Transform GetCurrentWorldTransform()
        {
            return this.transform;
        }
    }

}
