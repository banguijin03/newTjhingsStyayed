using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : MonoBehaviour
{
    public HitPointModule percent;
    public Slider slider;

    void Update()
    {
        slider.value = percent.PercentHP();
    }
}
