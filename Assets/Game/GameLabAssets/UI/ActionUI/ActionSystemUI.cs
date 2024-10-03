using GameLab.UnitSystem.ActionSystem;
using System.Collections.Generic;
using UnityEngine;


namespace GameLab.UISystem
{
    public class ActionSystemUI : MonoBehaviour
    {
        public static ActionSystemUI Instance;

        [SerializeField] GameObject actionButtonUIprefab;
        [SerializeField] GameObject actionbuttonUIcontainer;
        List<GameObject> buttonUIs = new();
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        }

        public void SpawnActionButtons(List<IAction> actions, object target)
        {
            Clear();
            actions.ForEach(action =>
            {
                GameObject actionUI = Instantiate(actionButtonUIprefab, actionbuttonUIcontainer.transform);
                actionUI.GetComponent<ActionButtonUI>().SetupButton(action, target);
                buttonUIs.Add(actionUI);
            });
            actionbuttonUIcontainer.transform.position = Input.mousePosition;
        }
        public void Clear()
        {
            foreach (var item in buttonUIs)
            {
                Destroy(item.gameObject);
            }
            buttonUIs.Clear();
        }
    }
}

