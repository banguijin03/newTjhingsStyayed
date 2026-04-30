using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UIElements;

public class HitPointModule : CharacterModule
{
    public FillValue HP;
    public ImpactStatus Check;
    public HungerModule Hunger;
    public ThirstModule Thirst;
    float hungerTimer=0f;//시간재기용
    float thirstTimer=0f;//시간재기용

    void Awake()
    {
        HP = new FillValue(100, 100, 0);
        Hunger = GetComponent<HungerModule>();
        Thirst = GetComponent<ThirstModule>();
        Check = new ImpactStatus();
    }
     
    //hp감소
    public void DecreaseHp(int value)
    {
        HP.DecreaseCurrent(value);
    }

    //hp증가
    public void IncreaseHp(int value)
    {
        HP.IncreaseCurrent(value);
    }

    public float PercentHP()
    {
        return HP.Percent;
    }

    //hp 감소하는 경우
    public void attack(int value)//공격당함!
    {
        DecreaseHp(value);
    }

    //hp증가하는 경우
    public bool rest, eatHerb;//서서히
    void Update()
    {
        //hp가 0이하면 기절
        if (HP.IsEmpty)
        {
            Check.OutCheck = true;
        }

        //배고픔!!
        if (Hunger.isHungry==true)
        {
            hungerTimer += Time.deltaTime;

            if (hungerTimer >= 1f)
            {
                hungerTimer -= 1f;
                DecreaseHp(3);
            }
        }
        else
        {
            hungerTimer = 0f;
        }

        //  목마름
        if (Thirst.isThirst==true)
        {
            thirstTimer += Time.deltaTime;

            if (thirstTimer >= 1f)
            {
                thirstTimer -= 1f;
                DecreaseHp(3);
            }
        }
        else
        {
            thirstTimer = 0f;
        }
    }
}