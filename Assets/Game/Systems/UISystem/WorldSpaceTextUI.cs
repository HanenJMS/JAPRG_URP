using TMPro;
using UnityEngine;

namespace GameLab.UISystem
{
    public class WorldSpaceTextUI : MonoBehaviour
    {
        [SerializeField] TextMeshPro text;
        [SerializeField] Material materials;
        [SerializeField] float timer = 5f;
        private void Awake()
        {
            text = GetComponent<TextMeshPro>();
        }

        public void SetText(string text)
        {
            this.text.text = text;
        }

        public void SetColor(Color color)
        {
            text.color = color;

        }
        private void LateUpdate()
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}

