using Cinemachine;
using GameLab.UnitSystem;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace GameLab.Controller
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera cinemachineCamera;
        float moveSpeed = 10f;
        float rotationSpeed = 100f;
        float zoomAmount = 1f;
        const float MIN_FOLLOW_Y = 5f;
        const float MAX_FOLLOW_Y = 13f;

        Vector3 targetFollowOffset;
        CinemachineTransposer cineMachineTransposer;
        [SerializeField] GameObject target;
        private void Start()
        {
            cineMachineTransposer = cinemachineCamera.GetCinemachineComponent<CinemachineTransposer>();
            targetFollowOffset = cineMachineTransposer.m_FollowOffset;
            UnitSelectionSystem.Instance.onPlayerSelected += OnPlayerSelected;
            UnitSelectionSystem.Instance.onDeselectedUnit += OnDeselectUnit;
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
        private void HandleCameraMovement()
        {
            Vector3 moveDirection = new(0, 0, 0);
            moveDirection.x = Input.GetAxis("Horizontal");
            moveDirection.z = Input.GetAxis("Vertical");

            moveSpeed = 5f;
            Vector3 moveVector = transform.forward * moveDirection.z + transform.right * moveDirection.x;
            transform.position += moveVector * moveSpeed * Time.deltaTime;
        }

        private void HandleCameraRotation()
        {
            Vector3 rotationVector = new Vector3(0, 0, 0);
            if (Input.GetKey(KeyCode.Q))
            {
                rotationVector.y = -1f;
            }
            if (Input.GetKey(KeyCode.E))
            {
                rotationVector.y = +1f;
            }
            transform.eulerAngles += rotationVector * Time.unscaledDeltaTime * rotationSpeed;
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
            cineMachineTransposer.m_FollowOffset = Vector3.Lerp(cineMachineTransposer.m_FollowOffset, targetFollowOffset, Time.unscaledDeltaTime * zoomSpeed);
            cinemachineCamera.m_Lens.FieldOfView = Mathf.Clamp(cinemachineCamera.m_Lens.FieldOfView += -Input.mouseScrollDelta.y * zoomSpeed, 25f, 50f);
        }
    }

}
