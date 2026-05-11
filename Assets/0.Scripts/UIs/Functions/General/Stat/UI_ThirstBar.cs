using UnityEngine;
using UnityEngine.UI;

public class UI_ThirstBar : UIBase
{
    [SerializeField] Slider thirstBar;

    [SerializeField] CharacterBase targetCharacter;

    StatModule statModule;

    public override void Registration(UIManager manager)
    {
        base.Registration(manager);

        statModule = targetCharacter.GetModule<StatModule>();

        statModule.Thirst.OnValueChanged -= RefreshThirstBar;
        statModule.Thirst.OnValueChanged += RefreshThirstBar;

        RefreshThirstBar(statModule.Thirst.Current, statModule.Thirst.Max);
    }

    public override void Unregistration(UIManager manager)
    {
        base.Unregistration(manager);

        if (statModule != null)
        {
            statModule.Thirst.OnValueChanged -= RefreshThirstBar;
        }
    }

    void RefreshThirstBar(int current, int max)
    {
        thirstBar.value = (float)current / max;
    }
}
