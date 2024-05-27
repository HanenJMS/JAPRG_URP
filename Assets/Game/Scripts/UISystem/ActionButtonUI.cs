using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameLab.UISystem
{
    public class ActionButtonUI : MonoBehaviour
    {
        Action action;
        Button button;
        TextMeshProUGUI text;
        private void Awake()
        {
            button = GetComponent<Button>();
            text = GetComponentInChildren<TextMeshProUGUI>();   
        }
        public void SetupButton(Action action)
        {
            this.action = action;
            button.onClick.AddListener(() =>
            {
                action?.Invoke();
            });
        }
        void Clear()
        {
            Destroy(this.gameObject);
        }
    }
}

