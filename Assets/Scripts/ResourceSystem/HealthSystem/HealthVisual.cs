using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameLab.ResourceSystem
{
    public class HealthVisual : MonoBehaviour
    {
        HealthHandler handler;
        TextMeshProUGUI text;
        [SerializeField] Image barImage;
        private void Awake()
        {
            text = GetComponentInChildren<TextMeshProUGUI>();
        }
        private void Start()
        {
            handler = GetComponentInParent<HealthHandler>();
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

