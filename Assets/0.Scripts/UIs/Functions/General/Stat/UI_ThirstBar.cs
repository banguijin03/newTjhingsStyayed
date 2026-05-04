using UnityEngine;
using UnityEngine.UI;

public class UI_ThirstBar : UIBase
{
    public Stat percent;
    public Slider slider;

    void Update()
    {
        slider.value = percent.Percent;
    }
}
