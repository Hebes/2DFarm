using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodHand.Controller.MyCharacterController
{
    public class CSPlayerController : MonoBehaviour
    {
        public Transform CameraPoint;
        CSCCMoveAndJump _ccMoveAndJump;
        CharacterController _cController;
        CSThirdPersonCameraController _cameraController;
        Animator _animator;

        private void Awake()
        {
            _cController = transform.GetComponent<CharacterController>();
            _ccMoveAndJump = new CSCCMoveAndJump(_cController);
            _cameraController = new CSThirdPersonCameraController(CameraPoint);
            _animator = transform.GetComponent<Animator>();
        }
        private void OnEnable()
        {
            _ccMoveAndJump.OnMove.AddListener((speed) =>
            {
                _animator.SetFloat("Speed", speed);
            });
            _ccMoveAndJump.OnStopMove.AddListener((speed) =>
            {
                _animator.SetFloat("Speed", speed);
            });
            _ccMoveAndJump.OnJump.AddListener(() =>
            {                
                if (_coJump != null)
                    StopCoroutine(_coJump);
                _coJump = StartCoroutine(ValueLerp(0, 2, (curValue) =>
                {
                    _animator.SetFloat("Jump", curValue);
                }));
            });
        }
        private void OnDisable()
        {
            _ccMoveAndJump.OnMove.RemoveAllListeners();
            _ccMoveAndJump.OnStopMove.RemoveAllListeners();
            _ccMoveAndJump.OnJump.RemoveAllListeners();
            //_ccMoveAndJump.OnFalldown.RemoveAllListeners();
            _ccMoveAndJump.OnStartLand.RemoveAllListeners();
        }
        private void Update()
        {
            _ccMoveAndJump.PlayAction();
            if (_cController.isGrounded)
            {
                if (_coJump != null)
                    StopCoroutine(_coJump);
                float tempJump = _animator.GetFloat("Jump");
                if(tempJump != 0)
                    _animator.SetFloat("Jump", Mathf.Lerp(tempJump, 0, Time.deltaTime * 10f));
            }
            //_animator.SetFloat("Grounded", _cController.isGrounded);
        }
        private void LateUpdate()
        {
            _cameraController.CameraRotation();
        }

        private void OnFootstep()
        {

        }
        private void OnLand()
        {

        }

        #region Ð­³Ì
        Coroutine _coJump;       
        private IEnumerator ValueLerp(float para1, float para2, Action<float> action, float changeRate = 10f)
        {
            float temp = para1;
            while (temp != para2)
            {
                temp = Mathf.Lerp(temp, para2, Time.fixedDeltaTime * changeRate);
                action?.Invoke(temp);
                yield return new WaitForFixedUpdate();
            }
        }
        #endregion
    }
}