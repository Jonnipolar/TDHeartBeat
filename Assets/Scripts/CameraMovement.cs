using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    class CameraMovement : MonoBehaviour
    {
        public float cameraMoveSpeed = 10f;

        private Camera mainCamera;
        private Vector3 cameraPosition;
        private float edgeSize = 10f;

        private void Start()
        {
            mainCamera = Camera.main;
            cameraPosition = mainCamera.transform.position;
        }

        private void Update()
        {
            if (Input.mousePosition.x > Screen.width - edgeSize)
            {
                cameraPosition.x += cameraMoveSpeed * Time.deltaTime;
            }
            else if (Input.mousePosition.x < edgeSize)
            {
                cameraPosition.x -= cameraMoveSpeed * Time.deltaTime;
            }

            if (Input.mousePosition.y > Screen.height - edgeSize)
            {
                cameraPosition.y += cameraMoveSpeed * Time.deltaTime;
            }
            else if (Input.mousePosition.y < edgeSize)
            {
                cameraPosition.y -= cameraMoveSpeed * Time.deltaTime;
            }

            mainCamera.transform.position = cameraPosition;
        }
    }
}
