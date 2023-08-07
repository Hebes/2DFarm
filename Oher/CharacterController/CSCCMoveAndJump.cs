using UnityEngine;
using UnityEngine.Events;

namespace GodHand.Controller.MyCharacterController
{
    public class CSCCMoveAndJump : AbsCCAction
    {
        /// <summary>
        /// 开始移动时
        /// </summary>
        public UnityEvent<float> OnMove;
        /// <summary>
        /// 停止移动时
        /// </summary>
        public UnityEvent<float> OnStopMove;
        /// <summary>
        /// 跳跃时
        /// </summary>
        public UnityEvent OnJump;
        /// <summary>
        /// 下落时
        /// </summary>
        public UnityEvent OnFalldown;
        public UnityEvent OnStartLand;
        /// <summary>
        /// 当前速度
        /// </summary>
        private float _curSpeed;
        /// <summary>
        /// 移动速度
        /// </summary>
        private float Speed;
        /// <summary>
        /// 奔跑速度
        /// </summary>
        private float RunSpeed;
        /// <summary>
        /// 跳跃高度
        /// </summary>
        private float JumpHeight;

        /// <summary>
        /// 角色移动和跳跃
        /// </summary>
        /// <param name="characterController">角色控制器</param>
        /// <param name="speed">移动速度(default 2f)</param>
        /// <param name="runSpeed">奔跑速度(default 5f))</param>
        /// <param name="jumpHeight">跳跃高度(default 1.2f)</param>
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
        /// 设置移动速度
        /// </summary>
        /// <param name="newSpeed"></param>
        public void SetSpeed(float newSpeed)
        {
            Speed = newSpeed;
        }
        /// <summary>
        /// 设置奔跑速度
        /// </summary>
        /// <param name="runSpeed"></param>
        public void SetRunSpeed(float runSpeed)
        {
            RunSpeed = runSpeed;
        }
        /// <summary>
        /// 设置跳跃高度
        /// </summary>
        /// <param name="jumpHeight"></param>
        public void SetJumpHeight(float jumpHeight)
        {
            JumpHeight = jumpHeight;
        }
        #region 移动
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
            //计算当前速度
            float currentHorizontalSpeed = new Vector3(_cController.velocity.x, 0f, _cController.velocity.z).magnitude;
            //当前速度与目标速度相差过大时平滑变化
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
            //输入方向单位长度
            Vector3 inputDirection = new Vector3(_v2Move.x, 0f, _v2Move.y).normalized;
            //计算目标角度
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            //旋转平滑过渡
            float rotation = Mathf.SmoothDampAngle(_cController.transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, _rotationSmoothTime);
            _cController.transform.rotation = Quaternion.Euler(0f, rotation, 0f);

            Vector3 targetDirection = Quaternion.Euler(0f, _targetRotation, 0f) * Vector3.forward;

            return targetDirection.normalized * _curSpeed;
        }
        #endregion

        #region 跳跃
        public float Gravity = -15f;
        /// <summary>
        /// 最大跳跃CD
        /// </summary>
        public float JumpTimeout = 0.3f;
        /// <summary>
        /// 最大摔落判断CD
        /// </summary>
        public float FallTimeout = 0.15f;
        private float _verticalVelocity;
        private float _terminalVelocity = 53f;
        /// <summary>
        /// 剩余跳跃CD
        /// </summary>
        private float _jumpTimeoutDelta;
        /// <summary>
        /// 剩余摔落CD
        /// </summary>
        private float _fallTimeoutDelta;

        private Vector3 _lastPos = default;
        public Vector3 JumpAndGravity()
        {
            _cController.Move(new Vector3(0, _verticalVelocity, 0) * Time.deltaTime);//重力移动

            if (_cController.isGrounded)
            {
                _fallTimeoutDelta = FallTimeout;

                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                if (Input.GetKeyDown(KeyCode.Space) && _jumpTimeoutDelta <= 0f)
                {
                    _lastPos = _cController.transform.position;//跳跃前坐标
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);//根据跳跃高度计算初速度
                    OnJump?.Invoke();
                    //播放动画
                }
                else if (_jumpTimeoutDelta > 0f)//跳跃CD
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                    OnStartLand?.Invoke();
                }
            }
            else//空中状态
            {
                _jumpTimeoutDelta = JumpTimeout;
                if (_fallTimeoutDelta >= 0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    if (_cController.transform.position.y < _lastPos.y)//下落中
                    {
                        OnFalldown?.Invoke();
                    }
                    _lastPos = _cController.transform.position;//重置上一次坐标                    
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
