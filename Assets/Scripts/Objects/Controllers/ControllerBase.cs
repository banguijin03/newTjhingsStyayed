using UnityEngine;

public class ControllerBase : MonoBehaviour, IFunctionable
{
    CharacterBase _character;
    public CharacterBase Character => _character;

    public void Start()
    {
        //게임매니져에 초기화 신청(함수 등록 대신 신청)
        GameManager.OnInitializeController += RegistrationFunctions;
    }

    public virtual void RegistrationFunctions()
    {
        Possess(GetComponent<CharacterBase>());
    }
    public void UnregistrationFunctions()
    {
        UnPossess();
    }


    public virtual void OnPossess(CharacterBase newCharacter) { }
    public void Possess(CharacterBase target)
    {
        if(!target) return; //대상이 없다
        ControllerBase result = target.Possessed(this);
        if(result == this) OnPossess(target);
    }


    public virtual void OnUnpossess(CharacterBase oldCharacter) { }
    public void UnPossess()
    {
        if (Character.Unpossessed(this))
        {
            OnUnpossess(Character);
        }
        _character = null;
    }

    //캐릭터한테 명령
    public void CommandMoveToDirection(Vector3 direction) 
    {
        if(Character is IRunnable target) target.MoveToDirection(direction); 
    }
    public void CommandMoveToDestination(Vector3 destination, float tolerance)
    {
        if (Character is IRunnable target) target.MoveToDestination(destination, tolerance); 
    }
    public void CommandStop()
    {
        if (Character is IRunnable target) target.StopMovement(); 
    }
}
