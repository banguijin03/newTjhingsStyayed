using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UIBase
{
    [SerializeField] Slider hpBar;

    [SerializeField] CharacterBase targetCharacter;

    StatModule statModule;

    public override void Registration(UIManager manager)
    {
        base.Registration(manager);

        statModule = targetCharacter.GetModule<StatModule>();

        statModule.HP.OnValueChanged -= RefreshHPBar;
        statModule.HP.OnValueChanged += RefreshHPBar;

        RefreshHPBar(statModule.HP.Current, statModule.HP.Max);
    }

    public override void Unregistration(UIManager manager)
    {
        base.Unregistration(manager);

        if (statModule != null)
        {
            statModule.HP.OnValueChanged -= RefreshHPBar;
        }
    }

    void RefreshHPBar(int current, int max)
    {
        hpBar.value = (float)current / max;
    }
}