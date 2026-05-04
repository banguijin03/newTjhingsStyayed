using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UIBase
{
    public Stat percent;
    public Slider slider;

    void Update()
    {
        slider.value = percent.Percent;
    }
}
