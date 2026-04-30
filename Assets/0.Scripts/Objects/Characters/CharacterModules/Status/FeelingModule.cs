using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;

public class FeelingModule : CharacterModule
{
    public FillValue Feeling;
    public ImpactStatus Check;
    float Timer=0f;

    void Awake()
    {
        Feeling = new FillValue(100, 100, 0);
    }

    public void DecreaseFeeling(int value)
    {
        Feeling.DecreaseCurrent(value);
    }

    //hp증가
    public void IncreaseFeeling(int value)
    {
        Feeling.IncreaseCurrent(value);
    }

    public float PercentFeeling()
    {
        return Feeling.Percent;
    }

    //환각풀 섭취
    public void hallucinogenicGrass(int value) => Feeling.IncreaseCurrent(value);

    void Update()
    {
        Timer += Time.deltaTime;

        if (Timer >= 1f)
        {
            Timer -= 1f; 

            Feeling.DecreaseCurrent(5);
        }

        if (Feeling.IsEmpty)
        {
            if (Check != null) 
                Check.OutCheck = true;
        }
    }
}
