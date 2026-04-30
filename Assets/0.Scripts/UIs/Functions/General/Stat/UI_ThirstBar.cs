using UnityEngine;
using UnityEngine.UI;

public class UI_ThirstBar : MonoBehaviour
{
    public ThirstModule percent;
    public Slider slider;

    void Update()
    {
        slider.value = percent.PercentThirst();
    }
}
