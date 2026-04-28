using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UIElements;

public class HitPointModule : CharacterModule
{
    public FillValue HP;

    [SerializeField] int _hpCur = 100;
    [SerializeField] int _hpMax = 100;
    [SerializeField] int _hpMin = 0;

    void Awake()
    {
        HP = new FillValue(_hpCur, _hpMax, _hpMin);
    }

    //hp 감소하는 경우
    public bool attack;
    public bool isPoison, beHungry, beThirst;//서서히

    //hp증가하는 경우
    public bool rest;
    public bool eatHerb;//서서히

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

    //hp반복증가
    public void ReapeatedIncreaseHp(int value, int Time)
    {
       HP.RepeatedIncrease(value, Time);
    }

    //hp반복감소
    public void ReapeatedDecreaseHp(int value, int Time)
    {
       HP.RepeatedDecrease(value, Time);
    }

    //기절상태인가?
    public bool OutCheck => HP.IsUnderZero;
}
