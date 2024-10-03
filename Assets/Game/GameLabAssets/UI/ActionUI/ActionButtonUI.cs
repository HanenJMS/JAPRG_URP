using GameLab.UnitSystem.ActionSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameLab.UISystem
{
    public class ActionButtonUI : MonoBehaviour
    {
        IAction action;
        Button button;
        TextMeshProUGUI text;
        Image image;
        private void Awake()
        {
            button = GetComponent<Button>();
            text = GetComponentInChildren<TextMeshProUGUI>();
            image = GetComponentInChildren<Image>();
        }
        public void SetupButton(IAction action, object target)
        {
            image.sprite = action.GetActionSprite();
            button?.onClick.AddListener(() =>
            {
                action.ExecuteOnTarget(target);
                Destroy(this.gameObject);
            });
        }
    }
}

