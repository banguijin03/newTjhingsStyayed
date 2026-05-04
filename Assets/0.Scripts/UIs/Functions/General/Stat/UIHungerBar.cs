using UnityEngine;
using UnityEngine.UI;

public class UIHungerBar : UIBase
{
    public Stat percent;
    public Slider slider;

    void Update()
    {
        slider.value = percent.Percent;
    }
}
