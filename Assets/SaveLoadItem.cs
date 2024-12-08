using GameLab.UISystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadItem : MonoBehaviour
{

    public SaveLoadMenu menu;

    public string MapName
    {
        get => mapName;
        set
        {
            mapName = value;
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = value;
        }
    }

    string mapName;

    public void Select()
    {
        menu.SelectItem(mapName);
    }
}
