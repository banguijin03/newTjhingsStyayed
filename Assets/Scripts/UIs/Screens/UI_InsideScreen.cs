using UnityEngine;

public class UI_InsideScreen : UI_ScreenBase
{
    private void OnEnable()
    {
        InputManager.OnCancel -= ToggleOption;
        InputManager.OnCancel += ToggleOption;
    }

    private void OnDisable()
    {
        InputManager.OnCancel -= ToggleOption;
    }

    void ToggleOption(bool value)
    {

        UIManager.ClaimToggleUI(UIType.InsideOption);
    }
}
