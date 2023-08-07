using UnityEngine;
using UnityEngine.Events;

namespace GodHand.Controller.MyCharacterController
{
    public class CSCCMoveAndJump : AbsCCAction
    {
        /// <summary>
        /// ��ʼ�ƶ�ʱ
        /// </summary>
        public UnityEvent<float> OnMove;
        /// <summary>
        /// ֹͣ�ƶ�ʱ
        /// </summary>
        public UnityEvent<float> OnStopMove;
        /// <summary>
        /// ��Ծʱ
        /// </summary>
        public UnityEvent OnJump;
        /// <summary>
        /// ����ʱ
        /// </summary>
        public UnityEvent OnFalldown;
        public UnityEvent OnStartLand;
        /// <summary>
        /// ��ǰ�ٶ�
        /// </summary>
        private float _curSpeed;
        /// <summary>
        /// �ƶ��ٶ�
        /// </summary>
        private float Speed;
        /// <summary>
        /// �����ٶ�
        /// </summary>
        private float RunSpeed;
        /// <summary>
        /// ��Ծ�߶�
        /// </summary>
        private float JumpHeight;

        /// <summary>
        /// ��ɫ�ƶ�����Ծ
        /// </summary>
        /// <param name="characterController">��ɫ������</param>
        /// <param name="speed">�ƶ��ٶ�(default 2f)</param>
        /// <param name="runSpeed">�����ٶ�(default 5f))</param>
        /// <param name="jumpHeight">��Ծ�߶�(default 1.2f)</param>
        public CSCCMoveAndJump(CharacterController characterController, float speed = 2, float runSpeed = 5, float jumpHeight = 1.2f) : base(characterController)
        {
            Speed = speed;
            RunSpeed = runSpeed;
            JumpHeight = jumpHeight;
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
            OnMove = new UnityEvent<float>();
            OnStopMove = new UnityEvent<float>();
            OnJump = new UnityEvent();
            OnFalldown = new UnityEvent();
            OnStartLand = new UnityEvent();
        }

        public override void PlayAction()
        {
            Vector3 targetDirection = Move();
            Vector3 gravity = JumpAndGravity();
            _cController.Move(targetDirection * Time.deltaTime + gravity * Time.deltaTime);
        }
        /// <summary>
        /// �����ƶ��ٶ�
        /// </summary>
        /// <param name="newSpeed"></param>
        public void SetSpeed(float newSpeed)
        {
            Speed = newSpeed;
        }
        /// <summary>
        /// ���ñ����ٶ�
        /// </summary>
        /// <param name="runSpeed"></param>
        public void SetRunSpeed(float runSpeed)
        {
            RunSpeed = runSpeed;
        }
        /// <summary>
        /// ������Ծ�߶�
        /// </summary>
        /// <param name="jumpHeight"></param>
        public void SetJumpHeight(float jumpHeight)
        {
            JumpHeight = jumpHeight;
        }
        #region �ƶ�
        public Vector2 V2Move
        {
            get
            {
                _v2Move = Vector2.zero;
                if (Input.GetKey(KeyCode.W))
                {
                    _v2Move.y = 1;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    _v2Move.y = -1;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    _v2Move.x = -1;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    _v2Move.x = 1;
                }
                return _v2Move;
            }
        }
        private Vector2 _v2Move;

        public float TargetSpeed
        {
            get
            {
                if (V2Move == Vector2.zero)
                {
                    return 0;
                }
                return Input.GetKey(KeyCode.LeftShift) ? RunSpeed : Speed;
            }
        }

        private const float _speedOffset = 0.1f;
        private const float _rotationSmoothTime = 0.1f;
        private const float _speedChangeRate = 10f;
        private float _targetRotation;
        private float _rotationVelocity;

        public Vector3 Move()
        {
            if (V2Move == Vector2.zero)
            {
                if (_curSpeed != 0)
                {
                    _curSpeed = Mathf.Lerp(_curSpeed, 0, _speedChangeRate * Time.deltaTime);
                    _curSpeed = Mathf.Round(_curSpeed * 1000f) / 1000f;
                }
                OnStopMove?.Invoke(_curSpeed);
                return Vector3.zero;
            }
            //���㵱ǰ�ٶ�
            float currentHorizontalSpeed = new Vector3(_cController.velocity.x, 0f, _cController.velocity.z).magnitude;
            //��ǰ�ٶ���Ŀ���ٶ�������ʱƽ���仯
            if (currentHorizontalSpeed < TargetSpeed - _speedOffset ||
                currentHorizontalSpeed > TargetSpeed + _speedOffset)
            {
                _curSpeed = Mathf.Lerp(currentHorizontalSpeed, TargetSpeed * _v2Move.magnitude, Time.deltaTime * _speedChangeRate);
                _curSpeed = Mathf.Round(_curSpeed * 1000f) / 1000f;
            }
            else
            {
                _curSpeed = TargetSpeed;
            }
            OnMove.Invoke(_curSpeed);
            //���뷽��λ����
            Vector3 inputDirection = new Vector3(_v2Move.x, 0f, _v2Move.y).normalized;
            //����Ŀ��Ƕ�
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            //��תƽ������
            float rotation = Mathf.SmoothDampAngle(_cController.transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, _rotationSmoothTime);
            _cController.transform.rotation = Quaternion.Euler(0f, rotation, 0f);

            Vector3 targetDirection = Quaternion.Euler(0f, _targetRotation, 0f) * Vector3.forward;

            return targetDirection.normalized * _curSpeed;
        }
        #endregion

        #region ��Ծ
        public float Gravity = -15f;
        /// <summary>
        /// �����ԾCD
        /// </summary>
        public float JumpTimeout = 0.3f;
        /// <summary>
        /// ���ˤ���ж�CD
        /// </summary>
        public float FallTimeout = 0.15f;
        private float _verticalVelocity;
        private float _terminalVelocity = 53f;
        /// <summary>
        /// ʣ����ԾCD
        /// </summary>
        private float _jumpTimeoutDelta;
        /// <summary>
        /// ʣ��ˤ��CD
        /// </summary>
        private float _fallTimeoutDelta;

        private Vector3 _lastPos = default;
        public Vector3 JumpAndGravity()
        {
            _cController.Move(new Vector3(0, _verticalVelocity, 0) * Time.deltaTime);//�����ƶ�

            if (_cController.isGrounded)
            {
                _fallTimeoutDelta = FallTimeout;

                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                if (Input.GetKeyDown(KeyCode.Space) && _jumpTimeoutDelta <= 0f)
                {
                    _lastPos = _cController.transform.position;//��Ծǰ����
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);//������Ծ�߶ȼ�����ٶ�
                    OnJump?.Invoke();
                    //���Ŷ���
                }
                else if (_jumpTimeoutDelta > 0f)//��ԾCD
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                    OnStartLand?.Invoke();
                }
            }
            else//����״̬
            {
                _jumpTimeoutDelta = JumpTimeout;
                if (_fallTimeoutDelta >= 0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    if (_cController.transform.position.y < _lastPos.y)//������
                    {
                        OnFalldown?.Invoke();
                    }
                    _lastPos = _cController.transform.position;//������һ������                    
                }
            }
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
            return new Vector3(0, _verticalVelocity, 0);
        }
        #endregion
    }
}
