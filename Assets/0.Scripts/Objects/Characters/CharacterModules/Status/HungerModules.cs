using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;

public class HungerModule : CharacterModule
{
    public FillValue Hunger;
    float Timer = 0f;

    void Awake()
    {
        Hunger = new FillValue(100, 100, 0);
    }



    public void DecreaseHunger(int value)
    {
        Hunger.DecreaseCurrent(value);
    }

    //hp증가
    public void IncreaseHunger(int value)
    {
        Hunger.IncreaseCurrent(value);
    }

    public float PercentHunger()
    {
        return Hunger.Percent;
    }

    //먹기
    public void Eat(int value) => Hunger.IncreaseCurrent(value);


    public bool isHungry=false;

    void Update()
    {
            float percent = Hunger.Percent;

            Timer += Time.deltaTime; 

            if (Timer >= 1f)
            {
                Timer -= 1f; 

                Hunger.DecreaseCurrent(5);
            }

            if (Hunger.IsEmpty)
            {
                isHungry = true;
            }
            else
            {
                isHungry = false;
            }
        }
    }