using UnityEngine;
using UnityEngine.UI;

public class UIHungerBar : MonoBehaviour
{
    public HungerModule percent;
    public Slider slider;

    void Update()
    {
        slider.value = percent.PercentHunger();
    }
}
