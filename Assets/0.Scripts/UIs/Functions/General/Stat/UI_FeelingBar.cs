using UnityEngine;
using UnityEngine.UI;

public class UI_FeelingBar : UIBase
{
    [SerializeField] Slider feelingBar;

    [SerializeField] CharacterBase targetCharacter;

    StatModule statModule;

    public override void Registration(UIManager manager)
    {
        base.Registration(manager);

        statModule = targetCharacter.GetModule<StatModule>();

        statModule.Feeling.OnValueChanged -= RefreshFeelingBar;
        statModule.Feeling.OnValueChanged += RefreshFeelingBar;

        RefreshFeelingBar(statModule.Feeling.Current, statModule.Feeling.Max);
    }

    public override void Unregistration(UIManager manager)
    {
        base.Unregistration(manager);

        if (statModule != null)
        {
            statModule.Feeling.OnValueChanged -= RefreshFeelingBar;
        }
    }

    void RefreshFeelingBar(int current, int max)
    {
        feelingBar.value = (float)current / max;
    }
}
