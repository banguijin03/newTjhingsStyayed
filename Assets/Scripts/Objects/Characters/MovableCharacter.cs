using UnityEngine;

public class MovableCharacter : CharacterBase, IRunnable, IFunctionable
{
    protected Vector3? targetDestination = null;
    protected Vector3? targetDestinaion = null;
    protected float targetTolerance;

    public void RegistrationFunctions()
    {
        GameManager.OnPhysicsCharacter -= PhysicsUpdate;
        GameManager.OnPhysicsCharacter += PhysicsUpdate;
    }
    public void UnregistrationFunctions()
    {
        GameManager.OnPhysicsCharacter -= PhysicsUpdate;
    }
    public void PhysicsUpdate(float deltaTime)
    {
        UpdateToDirection(deltaTime);
        UpdateToDestination(deltaTime);
    }
    public void UpdateToDirection(float deltaTime)
    {
        if (targetDestinaion is not null) return;
        float currentMoveSpeed = deltaTime * 5.0f;
        transform.position += currentMoveSpeed * targetDestinaion.Value;
       
    }

    public void UpdateToDestination(float deltaTime)
    {
         if(targetDestinaion is not  null) return;
        Vector3 currentDectination = (targetDestination.Value - transform.position);
        float distance = currentDectination.magnitude;
        if (distance > targetTolerance)
        {
            currentDectination.Normalize();
            float currentMoveSpeed = deltaTime * 5.0f;
            float resultMoveSpeed = Mathf.Min(currentMoveSpeed, distance);
            transform.position += resultMoveSpeed * currentDectination;
        }
    }

    public void MoveToDestination(Vector3 destination, float tolerance)
    {
        targetDestinaion = null;
        targetDestination = destination;
        targetTolerance = tolerance;
    }

    public void MoveToDirection(Vector3 direction)
    {
        targetDestination = null;
        targetDestinaion = direction.normalized;
    }
    public void StopMovement()
    {
        targetDestination = null;
        targetDestinaion = null;
    }
}
