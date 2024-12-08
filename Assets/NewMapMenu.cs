using GameLab.GridSystem;
using UnityEngine;

public class NewMapMenu : MonoBehaviour
{
    public void CreateMap(int x, int y)
    {
        HexGridVisualSystem.Instance.CreateMap(x, y);
        Close();
    }

    public void CreateSmallMap()
    {
        CreateMap(20, 15);
    }

    public void CreateMediumMap()
    {
        CreateMap(40, 30);
    }

    public void CreateLargeMap()
    {
        CreateMap(80, 60);
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

}
