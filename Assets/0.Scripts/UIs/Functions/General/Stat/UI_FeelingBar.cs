using UnityEngine;
using UnityEngine.UI;

public class UI_FeelingBar : MonoBehaviour
{
    public FeelingModule percent;
    public Slider slider;

    void Update()
    {
        slider.value = percent.PercentFeeling();
    }
}
