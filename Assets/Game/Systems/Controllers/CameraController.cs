using Cinemachine;
using GameLab.UnitSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.ScrollRect;

namespace GameLab.Controller
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera cinemachineCamera;
        float moveSpeed = 3f;
        float rotationSpeed = 100f;
        float zoomAmount = 1f;
        const float MIN_FOLLOW_Y = 5f;
        const float MAX_FOLLOW_Y = 35;

        Vector3 targetFollowOffset;
        CinemachineTransposer cineMachineTransposer;
        [SerializeField] GameObject target;
        private void Start()
        {
            cineMachineTransposer = cinemachineCamera.GetCinemachineComponent<CinemachineTransposer>();
            targetFollowOffset = cineMachineTransposer.m_FollowOffset;
            //UnitSelectionSystem.Instance.onPlayerSelected += OnPlayerSelected;
            //UnitSelectionSystem.Instance.onDeselectedUnit += OnDeselectUnit;
        }

        private void LateUpdate()
        {

            if (target != null)
            {
                HandleCameraFollowTarget();
            }
            else
            {
                HandleCameraMovement();
            }
            HandleCameraRotation();
            if (MouseWorldController.GetMouseRayCastInteractable() != null) return;
            if (EventSystem.current.IsPointerOverGameObject()) return;
            HandleCameraZoom();
        }
        void OnPlayerSelected()
        {
            target = UnitSelectionSystem.Instance.GetPlayerUnit().gameObject;
        }
        void OnDeselectUnit()
        {
            target = null;
        }
        void HandleCameraFollowTarget()
        {
            transform.position = target.transform.position;
        }

        Vector3 moveVector;
        Vector3 newPosition;
        Vector3 dragStartPosition, dragCurrentPosition;
        private void HandleCameraMovement()
        {
            Vector3 moveDirection = new(0, 0, 0);
            moveDirection.x = Input.GetAxis("Horizontal");
            moveDirection.z = Input.GetAxis("Vertical");
            moveVector += transform.forward * moveDirection.z + transform.right * moveDirection.x;

            transform.position = Vector3.Lerp(transform.position, moveVector, moveSpeed * Time.deltaTime);
            
        }

        private void HandleMovementKeyControl(Vector3 moveDirection)
        {
            
            
        }

        private void HandleMovementMouseControl()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Plane plane = new Plane(Vector3.up, Vector3.zero);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                float entry;
                if (plane.Raycast(ray, out entry))
                {
                    dragStartPosition = ray.GetPoint(entry);
                }
            }

            if (Input.GetMouseButton(0))
            {
                Plane plane = new Plane(Vector3.up, Vector3.zero);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                float entry;
                if (plane.Raycast(ray, out entry))
                {
                    dragCurrentPosition = ray.GetPoint(entry);

                    moveVector = transform.position + dragStartPosition - dragCurrentPosition;
                }
            }
        }
        Vector3 rotationVector;
        private void HandleCameraRotation()
        {
            rotationVector = new Vector3(0, 0, 0);
            if (Input.GetKey(KeyCode.Q))
            {
                rotationVector.y = -1f;
            }
            if (Input.GetKey(KeyCode.E))
            {
                rotationVector.y = +1f;
            }
            transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
            

            //HandleCameraMouseRotation();


        }

        Vector3 rotateStartPosition, rotateCurrentPosition;
        Quaternion newRotation;
        public void HandleCameraMouseRotation()
        {
            if (Input.GetMouseButtonDown(2))
            {
                rotateStartPosition = Input.mousePosition;
            }
            if (Input.GetMouseButton(2))
            {
                rotateCurrentPosition = Input.mousePosition;

                Vector3 difference = rotateStartPosition - rotateCurrentPosition;
                rotateStartPosition = rotateCurrentPosition;

                newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        }
        private void HandleCameraZoom()
        {
            if (Input.mouseScrollDelta.y < 0)
            {
                targetFollowOffset.y += zoomAmount;
            }
            if (Input.mouseScrollDelta.y > 0)
            {
                targetFollowOffset.y -= zoomAmount;
            }
            float zoomSpeed = 3f;
            targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y, MAX_FOLLOW_Y);
            targetFollowOffset.z = -Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y, MAX_FOLLOW_Y);
            cineMachineTransposer.m_FollowOffset = Vector3.Lerp(cineMachineTransposer.m_FollowOffset, targetFollowOffset, Time.unscaledDeltaTime * zoomSpeed);
            cinemachineCamera.m_Lens.FieldOfView = Mathf.Clamp(cinemachineCamera.m_Lens.FieldOfView += -Input.mouseScrollDelta.y * zoomSpeed, 25f, 50f);
        }
    }

}
