using UnityEngine;

namespace GameLab.UISystem
{
    public class WorldSpaceUIHandler : MonoBehaviour
    {
        [SerializeField] Transform TextWorldContainer;
        [SerializeField] GameObject worldTextUIObject;
        [SerializeField] Transform worldSpaceHolder;
        [SerializeField] int textSum = 0;
        [SerializeField] float countDownTimer = 0f;
        [SerializeField] bool isActive = false;
        [SerializeField] float subCountTimer = 0f;
        public void SpawnText(string text, GameObject worldTextUIObject)
        {
            var go = Instantiate(worldTextUIObject, TextWorldContainer);
            go.GetComponent<WorldSpaceTextUI>().SetText(text);
            if (this.worldTextUIObject == null)
            {
                this.worldTextUIObject = worldTextUIObject;
            }

            if (int.TryParse(text, out int y))
            {
                textSum += y;
                subCountTimer = Mathf.Clamp(subCountTimer += 1f, 0, 3f);
            }

            if (subCountTimer >= 2f)
            {
                isActive = true;
                countDownTimer = 1f;
            }
        }
        private void LateUpdate()
        {
            if (subCountTimer > 0)
            {
                subCountTimer = Mathf.Clamp(subCountTimer -= Time.deltaTime, 0, 4f);
            }
            if (!isActive) return;
            countDownTimer -= Time.deltaTime;
            if (countDownTimer > 0) return;
            SpawnText($"{textSum}", worldTextUIObject);
            worldTextUIObject = null;
            isActive = false;
            textSum = 0;
            countDownTimer = 1f;
            subCountTimer = 0f;
        }
    }
}

