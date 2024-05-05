using GameLab.InteractableSystem;
using System;
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
        transform.position = GetMousePosition();
    }
    public static Vector3 GetMousePosition()
    {
        RaycastHit hit = GetRaycastHit(instance.mousePlaneLayer);
        return hit.point;
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
    public static Interactable GetMouseRayCastInteractable()
    {
        return GetRaycastHit().collider.GetComponent<Interactable>();
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
}
