using UnityEngine;
namespace GameLab.FactionSystem
{
    public class FactionHandler : MonoBehaviour
    {
        [SerializeField] string currentFaction = "";

        public void SetFaction(string faction)
        {
            currentFaction = faction;
        }
        public string GetFaction()
        {
            return currentFaction;
        }
    }
}

