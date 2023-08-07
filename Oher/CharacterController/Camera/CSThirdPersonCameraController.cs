//Ïà»úÐý×ª
using UnityEngine;
namespace GodHand.Controller.MyCharacterController
{
    public class CSThirdPersonCameraController
    {
        public Vector2 MouseXY
        {
            get
            {
                return new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
            }
        }
        public float RotateSpeed;
        private Transform _cameraPoint;

        public CSThirdPersonCameraController(Transform cameraPoint, float rotateSpeed = 0)
        {
            _cameraPoint = cameraPoint;
            RotateSpeed = rotateSpeed;
        }

        float _cinemachineTargetYaw;
        float _cinemachineTargetPitch;
        float _bottomClamp = -30f;
        float _topClamp = 70f;
        public void CameraRotation()
        {
            //if (MouseXY == Vector2.zero) return;
            _cinemachineTargetYaw += MouseXY.x;
            _cinemachineTargetPitch += MouseXY.y;

            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, _bottomClamp, _topClamp);
            _cameraPoint.rotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0f);
        }

        private float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
    }
}