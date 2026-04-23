using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    ControllerBase _controller;
    public ControllerBase Controller => _controller;

    protected Vector3 _lookRotation;
    public Vector3 LookRotation => _lookRotation;

    public virtual string DisplayName => "character";

    public virtual void OnPossessed(ControllerBase newController){}
    public ControllerBase Possessed(ControllerBase from)
    {
        if(_controller) UnPossessed();
        _controller = from;
        OnPossessed(Controller);
        return _controller;
    }

    public void OnUnpossessed(ControllerBase oldController) {}
    public void UnPossessed()
    {
        if(Controller) OnUnpossessed(_controller);
        _controller= null;
    }
    public bool Unpossessed(ControllerBase oldController)
    {
        if (Controller != oldController) return false;
        UnPossessed();
        return true;
    }
}