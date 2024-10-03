using UnityEngine;

namespace GameLab.UnitSystem
{
    [System.Serializable]
    public enum UnitGender
    {
        Female, Male
    }
    public class UnitModel : MonoBehaviour
    {
        [SerializeField] UnitGender unitGender;
        public UnitGender GetGender()
        {
            return unitGender;
        }
    }
}


