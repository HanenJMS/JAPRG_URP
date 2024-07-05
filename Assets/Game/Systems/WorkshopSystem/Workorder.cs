using UnityEngine;

public class Workorder
{
    int progress = 0;
    public bool Assigned { get; set; }
    public void Tick()
    {
        progress += 1;
    }
    public void Cancel()
    {
        Assigned = false;
        
    }
}
