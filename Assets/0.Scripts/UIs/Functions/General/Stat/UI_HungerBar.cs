using UnityEngine;
using UnityEngine.UI;

public class UI_HungerBar : UIBase
{
    [SerializeField] Slider hungerBar;

    [SerializeField] CharacterBase targetCharacter;

    StatModule statModule;

    public override void Registration(UIManager manager)
    {
        base.Registration(manager);

        statModule = targetCharacter.GetModule<StatModule>();

        statModule.Hunger.OnValueChanged -= RefreshHungerBar;
        statModule.Hunger.OnValueChanged += RefreshHungerBar;

        RefreshHungerBar(statModule.Hunger.Current, statModule.Hunger.Max);
    }

    public override void Unregistration(UIManager manager)
    {
        base.Unregistration(manager);

        if (statModule != null)
        {
            statModule.Hunger.OnValueChanged -= RefreshHungerBar;
        }
    }

    void RefreshHungerBar(int current, int max)
    {
        hungerBar.value = (float)current / max;
    }
}
