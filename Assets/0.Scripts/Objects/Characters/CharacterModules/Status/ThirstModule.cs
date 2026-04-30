using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.EnhancedTouch;

public class ThirstModule : CharacterModule
{
    public FillValue Thirst;
    float Timer = 0f;

    void Awake()
    {
        Thirst = new FillValue(100, 100, 0);
    }

    public void DecreaseThirst(int value)
    {
        Thirst.DecreaseCurrent(value);
    }

    //hp증가
    public void IncreaseThirst(int value)
    {
        Thirst.IncreaseCurrent(value);
    }

    public float PercentThirst()
    {
        return Thirst.Percent;
    }

    //물마시기
    public void Drink(int value) => Thirst.IncreaseCurrent(value);

    public bool isThirst=false;

    void Update()
    {
        float percent = Thirst.Percent;

        Timer += Time.deltaTime;

        if (Timer >= 1f)
        {
            Timer -= 1f;

            Thirst.DecreaseCurrent(5);
        }

        if (Thirst.IsEmpty)
        {
            isThirst = true;
        }
        else
        {
            isThirst = false;
        }
    }
}
