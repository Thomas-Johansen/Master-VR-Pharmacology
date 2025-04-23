using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public SharedTimingData sharedTimingData;
    private float storedSpeed = 1;

    public GameObject calendar;
    private bool hasActivated;
    
    public GameObject playerCamera;
    private XROrigin xOrigin;
    
    //Controller inputs
    [SerializeField] private InputActionAsset inputActions;
    private InputAction _leftMoveAction;
    private Vector2 _leftMoveVector;
    private InputAction _leftPrimaryAction;
    private bool _leftPrimary;
    private InputAction _leftSecondaryAction;
    private bool _leftSecondary;
    private InputAction _leftGrabAction;
    private bool _leftGrab;
    private InputAction _leftTriggerAction;
    private bool _leftTrigger;
    private InputAction _rightMoveAction;
    private Vector2 _rightMoveVector;
    private InputAction _rightPrimaryAction;
    private bool _rightPrimary;
    private InputAction _rightSecondaryAction;
    private bool _rightSecondary;
    private InputAction _rightGrabAction;
    private bool _rightGrab;
    private InputAction _rightTriggerAction;
    private bool _rightTrigger;
    
    private void Awake()
    {
        // Bindings
        // Headset
        
        // Right Hand
        InputActionMap rightHandLocomotion = inputActions.FindActionMap("XRI Right Locomotion");
        _rightMoveAction = rightHandLocomotion.FindAction("Move");
        _rightMoveAction.Enable();
        _rightMoveAction.performed += OnRightMovementPerformed;
        _rightMoveAction.canceled += OnRightMovementCanceled;
        _rightPrimaryAction = rightHandLocomotion.FindAction("A Button");
        _rightPrimaryAction.Enable();
        _rightPrimaryAction.performed += OnRightPrimaryPerformed;
        _rightPrimaryAction.canceled += OnRightPrimaryCanceled;
        _rightSecondaryAction = rightHandLocomotion.FindAction("B Button");
        _rightSecondaryAction.Enable();
        _rightSecondaryAction.performed += OnRightSecondaryPerformed;
        _rightSecondaryAction.canceled += OnRightSecondaryCanceled;
        _rightGrabAction = rightHandLocomotion.FindAction("Grab Move");
        _rightGrabAction.Enable();
        _rightGrabAction.performed += OnRightGrabPerformed;
        _rightGrabAction.canceled += OnRightGrabCanceled;
        _rightTriggerAction = rightHandLocomotion.FindAction("Trigger");
        _rightTriggerAction.Enable();
        _rightTriggerAction.performed += OnRightTriggerPerformed;
        _rightTriggerAction.canceled += OnRightTriggerCanceled;
        
        // Left Hand
        InputActionMap leftHandLocomotion = inputActions.FindActionMap("XRI Left Locomotion");
        _leftMoveAction = leftHandLocomotion.FindAction("Move");
        _leftMoveAction.Enable();
        _leftMoveAction.performed += OnLeftMovementPerformed;
        _leftMoveAction.canceled += OnLeftMovementCanceled;
        _leftPrimaryAction = leftHandLocomotion.FindAction("X Button");
        _leftPrimaryAction.Enable();
        _leftPrimaryAction.performed += OnLeftPrimaryPerformed;
        _leftPrimaryAction.canceled += OnLeftPrimaryCanceled;
        _leftSecondaryAction = leftHandLocomotion.FindAction("Y Button");
        _leftSecondaryAction.Enable();
        _leftSecondaryAction.performed += OnLeftSecondaryPerformed;
        _leftSecondaryAction.canceled += OnLeftSecondaryCanceled;
        _leftGrabAction = leftHandLocomotion.FindAction("Grab Move");
        _leftGrabAction.Enable();
        _leftGrabAction.performed += OnLeftGrabPerformed;
        _leftGrabAction.canceled += OnLeftGrabCanceled;
        _leftTriggerAction = leftHandLocomotion.FindAction("Trigger");
        _leftTriggerAction.Enable();
        _leftTriggerAction.performed += OnLeftTriggerPerformed;
        _leftTriggerAction.canceled += OnLeftTriggerCanceled;

    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        calendar.SetActive(false);
        //playerCamera.transform.position = (transform.position + Vector3.up * 0.8f);
        xOrigin = playerCamera.GetComponent<XROrigin>();
        xOrigin.MoveCameraToWorldLocation(transform.position + Vector3.up * 0.8f);

    }

    // Update is called once per frame
    void Update()
    {
        if (sharedTimingData.Stage == 7 && !hasActivated)
        {
            hasActivated = true;
            calendar.SetActive(true);
        }

        if (sharedTimingData.Stage > 7)
        {
            calendar.SetActive(false);
        }
    }
    
        //Right Hand
    private void OnRightMovementPerformed(InputAction.CallbackContext context)
    {
        _rightMoveVector = context.ReadValue<Vector2>();
    }
    
    private void OnRightMovementCanceled(InputAction.CallbackContext context)
    {
        _rightMoveVector = Vector2.zero;
    }
    
    private void OnRightPrimaryPerformed(InputAction.CallbackContext context)
    {
        _rightPrimary = true;
        xOrigin.MoveCameraToWorldLocation(transform.position + Vector3.up * 0.8f);
        
    }

    private void OnRightPrimaryCanceled(InputAction.CallbackContext context)
    {
        _rightPrimary = false;
    }

    private void OnRightSecondaryPerformed(InputAction.CallbackContext context)
    {
        _rightSecondary = true;
        xOrigin.MoveCameraToWorldLocation(transform.position + Vector3.up * 0.8f);
    }

    private void OnRightSecondaryCanceled(InputAction.CallbackContext context)
    {
        _rightSecondary = false;
    }

    private void OnRightGrabPerformed(InputAction.CallbackContext context)
    {
        _rightGrab = true;
        sharedTimingData.IsGrabingRight = true;
    }
    
    private void OnRightGrabCanceled(InputAction.CallbackContext context)
    {
        _rightGrab = false;
        sharedTimingData.IsGrabingRight = false;
    }
    
    private void OnRightTriggerPerformed(InputAction.CallbackContext context)
    {
        _rightTrigger = true;
    }
    
    private void OnRightTriggerCanceled(InputAction.CallbackContext context)
    {
        _rightTrigger = false;
    }
    
    //LeftHand
    private void OnLeftMovementPerformed(InputAction.CallbackContext context)
    {
        _leftMoveVector = context.ReadValue<Vector2>();
    }
    
    private void OnLeftMovementCanceled(InputAction.CallbackContext context)
    {
        _leftMoveVector = Vector2.zero;
    }
    
    private void OnLeftPrimaryPerformed(InputAction.CallbackContext context)
    {
        _leftPrimary = true;
        //TODO: Remember this is placed here
        if (sharedTimingData.Speed >= 1)
        {
            sharedTimingData.Speed = 0;
            storedSpeed = sharedTimingData.Speed;
        }
        else
        {
            sharedTimingData.Speed = storedSpeed;
        }
    }
    
    private void OnLeftPrimaryCanceled(InputAction.CallbackContext context)
    {
        _leftPrimary = false;
    }
    
    private void OnLeftSecondaryPerformed(InputAction.CallbackContext context)
    {
        _leftSecondary = true;

    }
    
    private void OnLeftSecondaryCanceled(InputAction.CallbackContext context)
    {
        _leftSecondary = false;
    }
    
    private void OnLeftGrabPerformed(InputAction.CallbackContext context)
    {
        _leftGrab = true;
        sharedTimingData.IsGrabingLeft = true;
    }
    
    private void OnLeftGrabCanceled(InputAction.CallbackContext context)
    {
        _leftGrab = false;
        sharedTimingData.IsGrabingLeft = false;
    }
    
    private void OnLeftTriggerPerformed(InputAction.CallbackContext context)
    {
        _leftTrigger = true;
        sharedTimingData.Speed = 5;
    }
    
    private void OnLeftTriggerCanceled(InputAction.CallbackContext context)
    {
        _leftTrigger = false;
        sharedTimingData.Speed = 1;
    }
}
