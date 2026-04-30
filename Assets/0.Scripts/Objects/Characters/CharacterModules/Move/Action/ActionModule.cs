using System.Reflection;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;
//현재 행동
public class ActionModule : CharacterModule
{
    [SerializeField] Animator jump;
    [SerializeField] Animator rotate;
    [SerializeField] Animator production;
    [SerializeField] Animator cuttingwood;
    [SerializeField] Animator fishing;
    [SerializeField] Animator hoeing;
    [SerializeField] Animator sickling;
    [SerializeField] Animator polishing;
    [SerializeField] Animator watering;
    [SerializeField] Animator cutting;
    
    public MoveCheckType CurrentAction { get; private set; } = MoveCheckType.Idle;

    public void SetAction(MoveCheckType action)
    {
        if (CurrentAction == action) return;

        CurrentAction = action;
        OnActionChanged(action);
    }

    void OnActionChanged(MoveCheckType action)
    {
        switch (action)
        {
            case MoveCheckType.Idle:
                // 아무것도 안함 / 이동 멈춤
                break;

            case MoveCheckType.Walk:
                // 걷기
                break;

            case MoveCheckType.Jump:
                // 점프
                break;

            case MoveCheckType.Rotate:
                // 상호작용
                break;

            case MoveCheckType.Production:
                // 제작
                break;

            case MoveCheckType.CuttingWood:
                // 나무베기
                break;

            case MoveCheckType.Gathering:
                // 채집
                break;

            case MoveCheckType.Fishing:
                // 낚시
                break;

            case MoveCheckType.Hoeing:
                // 호미질
                break;

            case MoveCheckType.Sickling:
                // 낫질
                break;

            case MoveCheckType.Polishing:
                // 광질
                break;

            case MoveCheckType.Watering:
                // 물주기
                break;

            case MoveCheckType.Cutting:
                // 칼질
                break;
        }
    }
}