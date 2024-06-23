using GameLab.ResourceSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameLab.ManaSystem
{
    public class ManaVisual : MonoBehaviour
    {
        ManaHandler handler;
        TextMeshProUGUI text;
        [SerializeField] Image barImage;
        private void Awake()
        {
            text = GetComponentInChildren<TextMeshProUGUI>();
        }
        private void Start()
        {
            handler = GetComponentInParent<ManaHandler>();
            handler.onResourceChange += UpdateVisual;
            UpdateVisual();
        }
        private void LateUpdate()
        {
            transform.forward = Camera.main.transform.forward;
        }
        void UpdateVisual()
        {
            text.text = handler.ToString();
            barImage.fillAmount = handler.GetCurrentRatio();
            Debug.Log("health percentage = " + handler.GetCurrentRatio());
        }
    }
}

