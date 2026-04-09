using UnityEngine;

public class UI_InsideScreen : UI_ScreenBase
{
    private void OnEnable()
    {
        InputManager.OnCancel -= ToggleMenu;
        InputManager.OnCancel += ToggleMenu;
    }
    private void OnDisable()
    {
        InputManager.OnCancel -= ToggleMenu;
    }

    void ToggleMenu(bool vlaue) => UIManager.ClaimToggleUI(UIType.Menu);
}
