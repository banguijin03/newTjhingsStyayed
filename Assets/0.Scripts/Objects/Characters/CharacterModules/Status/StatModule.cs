using System.Collections;
using UnityEngine;

public class StatModule : CharacterModule
{
    public FillValue HP      = new FillValue(100, 100);
    public FillValue Hunger  = new FillValue(100, 100);
    public FillValue Thirst  = new FillValue(100, 100);
    public FillValue Feeling = new FillValue(100, 100);

    public bool IsDead => HP.Current <= 0;
    public bool IsHungry => Hunger.Current <= 0;
    public bool IsThirst => Thirst.Current <= 0;

    public bool IsPoison=false;
    public bool IsRunning=false;
    public bool IsBurn=false;

    void Start()
    {
        StartCoroutine(StatusRoutine());
    }

    IEnumerator StatusRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (IsDead) yield break;

            Hunger.DecreaseCurrent(1);
            Thirst.DecreaseCurrent(1);

            if(IsHungry)
            {
                HP.DecreaseCurrent(1);
            }
            if(IsThirst)
            {
                HP.DecreaseCurrent(1);
            }
            if (IsPoison)
            {
                HP.DecreaseCurrent(1);
            }
            if (IsRunning)
            {
                Hunger.DecreaseCurrent(1);
                Thirst.DecreaseCurrent(1);
            }
            if (IsBurn)
            {
                HP.DecreaseCurrent(1);
            }
           //if(IsHallucination)
           //if(IsSlow)
        }
    }
}