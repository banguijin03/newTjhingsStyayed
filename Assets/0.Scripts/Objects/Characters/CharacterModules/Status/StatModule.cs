using System.Collections;
using UnityEngine;

public class StatModule : CharacterModule
{
    public Stat HP;
    public Stat Hunger;
    public Stat Thirst;
    public Stat Mood;

    public override void OnRegistration(CharacterBase newOwner)
    {
        base.OnRegistration(newOwner);

        HP = new Stat("HP", 100, 100);
        Hunger = new Stat("Hunger", 100, 100);
        Thirst = new Stat("Thirst", 100, 100);
        Mood = new Stat("Mood", 100, 100);
    }

    //너 지금 이 행동하고 있는지에 대한 질문

    //달리고 있니
    public bool isRunning;
    //독상태니
    public bool isPoison;
    //화상 
    public bool isBurn;
    //둔화
    public bool isSlow;

    //배고프니
    public bool isStarving => Hunger.IsEmpty;
    //목마르니
    public bool isThirst => Thirst.IsEmpty;

    IEnumerator DecreaseNeedsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (isStarving)
            {
                HP.Decrease(1);
            }
            else if (isThirst)
            {
                HP.Decrease(1);
            }
            else
            {
                Hunger.Decrease(1);
                Thirst.Decrease(1);
            }
        }
    }
}
