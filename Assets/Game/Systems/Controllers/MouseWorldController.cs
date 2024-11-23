using GameLab.InteractableSystem;
using GameLab.UISystem;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorldController : MonoBehaviour
{
    private static MouseWorldController instance { get; set; }


    Action onMouseClick;
    static Camera mainCameraReference;
    [SerializeField] LayerMask mousePlaneLayer;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }
    private void Start()
    {
        mainCameraReference = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        //transform.position = GetMousePosition();
    }
    public static Vector3 GetMousePosition()
    {
        RaycastHit hit = GetRaycastHit();
        return hit.point;
    }
    public static bool GetMousePosition(out RaycastHit hitInfo)
    {
        hitInfo = GetRaycastHit();
        if(hitInfo.collider != null)
            return true;
        return false;
    }
    public static RaycastHit GetRaycastHit(LayerMask layerMask)
    {
        Ray ray = GetMouseRay();
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask);
        return raycastHit;
    }
    public static RaycastHit GetRaycastHit()
    {
        Ray ray = GetMouseRay();
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue);
        return raycastHit;
    }
    public static RaycastHit[] GetRaycastHits()
    {
        Ray ray = GetMouseRay();
        return Physics.RaycastAll(ray, float.MaxValue);
    }
    public static Interactable GetMouseRayCastInteractable()
    {
        if (GetRaycastHit().collider == null) return null;
        return GetRaycastHit().collider.GetComponent<Interactable>();
    }
    public static List<Interactable> GetMouseRayCastInteractables()
    {
        List<Interactable> interactables = new();
        foreach (RaycastHit hits in GetRaycastHits())
        {
            if (hits.collider.TryGetComponent<Interactable>(out Interactable interactable))
            {
                interactables.Add(interactable);
            }
        }
        return interactables;
    }
    public static T GetMouseRayInteractionType<T>() where T : MonoBehaviour
    {
        return GetRaycastHit().collider.GetComponent<T>();
    }
    static Ray GetMouseRay()
    {
        return mainCameraReference.ScreenPointToRay(Input.mousePosition);
    }
    public static T GetInteractionType<T>(RaycastHit hit) where T : MonoBehaviour
    {
        return hit.transform.GetComponent<T>();
    }

    public static void SetMouseCursor(MouseCursorData cursor)
    {

        Cursor.SetCursor(cursor.texture, cursor.hotspot, CursorMode.Auto);
    }
}
