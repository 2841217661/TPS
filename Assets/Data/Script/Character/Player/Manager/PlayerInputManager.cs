using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public Vector2 movementInput;
    public Vector2 cameraInput;

    public PlayerInputAction inputActions;


    public void ApplyActionMap(bool _useNormalInput, bool _useUIInput)
    {
        if (_useNormalInput)
            inputActions.Normal.Enable();
        else
            inputActions.Normal.Disable();

        if (_useUIInput)
            inputActions.UI.Enable();
        else
            inputActions.UI.Disable();
    }

    private void Awake()
    {
        inputActions = new PlayerInputAction();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void GetAllInput()
    {
        GetMovementInput();
        GetCameraInput();
    }

    private void GetMovementInput()
    {
        movementInput = inputActions.Normal.Movement.ReadValue<Vector2>();
    }

    public bool GetRunInput()
    {
        return inputActions.Normal.Run.WasPressedThisFrame();
    }

    public void GetCameraInput()
    {
        cameraInput = inputActions.Normal.Look.ReadValue<Vector2>();
    }

    public bool GetFireInput()
    {
        return inputActions.Normal.Fire.IsPressed();
    }

    public bool GetAimInput()
    {
        return inputActions.Normal.Aim.IsPressed();
    }

    public bool GetJumpInput()
    {
        return inputActions.Normal.Jump.WasPressedThisFrame();
    }

    public bool GetThrowInput()
    {
        return inputActions.Normal.Throw.WasPressedThisFrame();
    }

    public bool GetAttackInput()
    {
        return inputActions.Normal.ElbowStrike.WasPressedThisFrame();
    }
}
