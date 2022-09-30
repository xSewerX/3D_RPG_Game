using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]

    [RequireComponent(typeof(PlayerInput))]

    public class PlayerControls : MonoBehaviour
    {

        #region ThirdPersonController
        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;
        
        [SerializeField]
        private AudioSource audioSource;

        [Range(0f, 1f)]
        public float volume = 1;

        public AudioClip[] FootstepAudioClips;
        public AudioClip[] SwordSwing;
        public AudioClip[] StrongSwordSwing;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        private PlayerInput _playerInput;
#endif
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

        private bool IsCurrentDeviceMouse
        {
            get
            {
                return _playerInput.currentControlScheme == "KeyboardMouse";

            }
        }
        #endregion

        [Header("Player Controls")]
        
        //public Item item1;
       // public Item item2;
        //public Item item3;
        //public Item item4;
        //public Item item5;
        //public Item item6;
        //public Item item7;

        Animator anim;
        public @StarterAsset playerControls;
        private MeleeWeapon Weapon;
        //public GameObject inventoryUI, characterUI, QuestLog, Map, MenuUI;
        private CharacterStats characterStats;
        public MenuUI menuUI;

        private InputAction attack;
        private InputAction opencharpanel;
        private InputAction interaction;
        private InputAction openinventory;
        private InputAction RightClick;
        private InputAction openQuestLog;
        private InputAction openMap;
        private InputAction openMenu;

        private float nextAttackTime = 0f;
        private float nextStrongAttackTime = 0f;
        public float attackRate = 2f;
        private bool isAttacking;
        private bool isAttackingStrong;
        private bool inventoryOpen = false;
        bool isoverui;
        [HideInInspector]
        public float targetSpeed;
        
        [HideInInspector]
        public bool isGathering;

        private void Awake()
        {
            playerControls = new @StarterAsset();
            anim = GetComponent<Animator>();
            characterStats = GetComponent<CharacterStats>();
            Weapon = GetComponentInChildren<MeleeWeapon>();

            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }

        }

        private void Start()
        {
            EquipmentManager.instance.weaponobject.SetActive(false);
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

            _hasAnimator = TryGetComponent(out anim);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
            _playerInput = GetComponent<PlayerInput>();
            
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
            AssignAnimationIDs();
            
        }

        private void OnEnable()
        {
            RightClick = playerControls.Player.MouseRightClick;
            RightClick.Enable();
            RightClick.performed += StrongAttack;

            opencharpanel = playerControls.Player.OpenCharacterPanel;
            opencharpanel.Enable();
            opencharpanel.performed += OpenCharacterPanel;

            openinventory = playerControls.Player.OpenInventory;
            openinventory.Enable();
            openinventory.performed += OpenInventory;

            interaction = playerControls.Player.Interaction;
            interaction.Enable();
            interaction.performed += Interaction;

            attack = playerControls.Player.Attack;
            attack.Enable();
            attack.performed += Attack;

            openMap = playerControls.Player.OpenMap;
            openMap.Enable();
            openMap.performed += OpenMap;

            openMenu = playerControls.Player.OpenMenu;
            openMenu.Enable();
            openMenu.performed += OpenMenu;

            openQuestLog = playerControls.Player.OpenQuestLog;
            openQuestLog.Enable();
            openQuestLog.performed += OpenQuestLog;
        }

        private void OnDisable()
        {
            opencharpanel.Disable();
            attack.Disable();
            interaction.Disable();
            openinventory.Disable();
            RightClick.Disable();
            openMap.Disable();
            openMenu.Disable();
            openQuestLog.Disable();
        }

        private void Update()
        {
            _hasAnimator = TryGetComponent(out anim);

            JumpAndGravity();
            GroundedCheck();
            Move();

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                isoverui = false;

                if (isAttacking == true)
                {
                    Weapon.Attack();
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                if (isAttackingStrong == true)
                {
                    Weapon.StrongAttack();
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                
            }
            else
            {
                isoverui = true;
            }

            if (inventoryOpen == true)
            {
                LockCameraPosition = true;
            }
            else
            {
                LockCameraPosition = false;
            }
        }
        private void LateUpdate()
        {
            CameraRotation();
        }

        private void Attack(InputAction.CallbackContext context)
        {

            if (Time.time >= nextAttackTime && isoverui == false && EquipmentManager.instance.isWeaponEquipped == true && isAttackingStrong == false && isGathering==false)
            {
                anim.SetTrigger("Attack");
                isAttacking = true;
                Invoke("IsAttacking", 0.8f);
                nextAttackTime = Time.time + 1f / attackRate;
            }


        }
        private void AttackSound()
        {
                if (SwordSwing.Length > 0)
                {
                    var index = Random.Range(0, SwordSwing.Length);
                audioSource.PlayOneShot(SwordSwing[index], volume);

            }
        }

        private void StrongAttack(InputAction.CallbackContext context)
        {

             //Inventory.instance.Add(item1, 1);
            // Inventory.instance.Add(item2, 1);
             //Inventory.instance.Add(item3, 1);
            // Inventory.instance.Add(item4, 1);
            // Inventory.instance.Add(item5, 1);
            // Inventory.instance.Add(item6, 1);
            // Inventory.instance.Add(item7, 1);
            //CharacterStats.instance.AddExp(700);

            if (Time.time >= nextStrongAttackTime && isoverui == false && EquipmentManager.instance.isWeaponEquipped == true && isAttacking == false && Grounded && isGathering==false)
            {
                anim.SetTrigger("StrongAttack");
                isAttackingStrong = true;
                Invoke("IsAttackingStrong", 1.8f);
                nextStrongAttackTime = Time.time + 1f / attackRate * 2.5f;
            }
        }
        private void StrongAttackSound()
        {
            if (StrongSwordSwing.Length > 0)
            {
                var index = Random.Range(0, StrongSwordSwing.Length);
                audioSource.PlayOneShot(StrongSwordSwing[index], volume);
            }
        }
        private void IsAttackingStrong()
        {
            isAttackingStrong = false;
        }
        private void IsAttacking()
        {
            isAttacking = false;
        }
        

        private void OpenInventory(InputAction.CallbackContext context)
        {
            menuUI.InventoryExit();
        }
        private void OpenCharacterPanel(InputAction.CallbackContext context)
        {
            menuUI.CharacterProfileMenu();
        }

        private void Interaction(InputAction.CallbackContext context)
        {
            
        }

        private void OpenMenu(InputAction.CallbackContext context)
        {
            menuUI.OpenMenu();
        }

        private void OpenMap(InputAction.CallbackContext context)
        {
            menuUI.MapButton();
        }

        private void OpenQuestLog(InputAction.CallbackContext context)
        {
            menuUI.Questlog();
        }


        #region ThirdPersonController
        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                anim.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        private void Move()
        {
            if (isGathering == false)
            {
                if (isAttackingStrong == false)
                {
                    // set target speed based on move speed, sprint speed and if sprint is pressed
                    targetSpeed = CharacterStats.instance.MovementSpeed.GetValue();

                    // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

                    // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
                    // if there is no input, set the target speed to 0
                    if (_input.move == Vector2.zero) targetSpeed = 0.0f;

                    // a reference to the players current horizontal velocity
                    float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

                    float speedOffset = 0.1f;
                    float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

                    // accelerate or decelerate to target speed
                    if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                        currentHorizontalSpeed > targetSpeed + speedOffset)
                    {
                        // creates curved result rather than a linear one giving a more organic speed change
                        // note T in Lerp is clamped, so we don't need to clamp our speed
                        _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                            Time.deltaTime * SpeedChangeRate);

                        // round speed to 3 decimal places
                        _speed = Mathf.Round(_speed * 1000f) / 1000f;
                    }
                    else
                    {
                        _speed = targetSpeed;
                    }

                    _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
                    if (_animationBlend < 0.01f) _animationBlend = 0f;

                    // normalise input direction
                    Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

                    // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
                    // if there is a move input rotate player when the player is moving
                    if (_input.move != Vector2.zero)
                    {
                        _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                          _mainCamera.transform.eulerAngles.y;
                        float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                            RotationSmoothTime);

                        // rotate to face input direction relative to camera position
                        transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                    }


                    Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

                    // move the player
                    _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                                     new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        anim.SetFloat(_animIDSpeed, _animationBlend);
                        anim.SetFloat(_animIDMotionSpeed, inputMagnitude);

                    }
                }
            }
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    anim.SetBool(_animIDJump, false);
                    anim.SetBool(_animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                
                if (_input.jump && _jumpTimeoutDelta <= 0.0f && isAttackingStrong ==false)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        anim.SetBool(_animIDJump, true);
                    }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        anim.SetBool(_animIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                _input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
             if (animationEvent.animatorClipInfo.weight > 0.5f)
             {
                 if (FootstepAudioClips.Length > 0)
                 {
                     var index = Random.Range(0, FootstepAudioClips.Length);
                    audioSource.PlayOneShot(FootstepAudioClips[index], volume);
                 }
             }
            
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            AudioManager.instance.Play("Landing");
        }
        #endregion

    }
}