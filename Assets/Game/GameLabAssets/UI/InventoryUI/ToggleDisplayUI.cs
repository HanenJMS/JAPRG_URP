using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLab.UISystem
{
    public class ToggleDisplayUI : MonoBehaviour
    {
        [SerializeField] KeyCode key;
        [SerializeField] bool isActive = false;
        [SerializeField] GameObject displayObject;
        private void LateUpdate()
        {
            if (displayObject == null) return;
            if(Input.GetKeyDown(key))
            {
                isActive = !isActive;
                this.displayObject.SetActive(isActive);
            }
        }
    }
}

