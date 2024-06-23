using System.Collections;
using System.Collections.Generic;
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
        public UnitGender GetGender() => unitGender;
    }
}


