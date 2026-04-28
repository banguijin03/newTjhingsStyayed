using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class StatusModules : MonoBehaviour
{
    public FillValue Hunger;
    public FillValue Thirst;
    public FillValue Feeling;

    //hunger
    [SerializeField] int _hungerCur = 100;
    [SerializeField] int _hungerMax = 100;
    [SerializeField] int _hungerMin = 0;
    //thirst
    [SerializeField] int _thirstCur = 100;
    [SerializeField] int _thirstMax = 100;
    [SerializeField] int _thirstMin = 0;
    //feeling
    [SerializeField] int _feelingCur = 100;
    [SerializeField] int _feelingMax = 100;
    [SerializeField] int _feelingMin = 0;
    //독시간
    [SerializeField] float poisoTime = 5.0f;
    [SerializeField] float healTime = 5.0f;
    void Awake()
    {
        Hunger = new FillValue(_hungerCur, _hungerMax, _hungerMin);
        Thirst = new FillValue(_thirstCur, _thirstMax, _thirstMin);
        Feeling = new FillValue(_feelingCur, _feelingMax, _feelingMin);
    }

    //일하고 있을때
    public void Work(int h_value, int t_value)
    {
        Hunger.DecreaseCurrent(h_value);
        Thirst.DecreaseCurrent(t_value);
    }

    //먹기
    public void Eat(int value) => Hunger.IncreaseCurrent(value);

    //물 마시기
    public void Drink(int value) => Thirst.IncreaseCurrent(value);

    //환각풀 섭취
    public void hallucinogenicGrass(int value) => Feeling.IncreaseCurrent(value);


    void Update()
    {
        Hunger.DecreaseCurrent(1);
        Thirst.DecreaseCurrent(1);
    }
}
/*1시간을 3분
6시~오전 2시
6+12+2=20

배고픔 최대 100
목마름 최대 100

배고픔을 하루에 20정도 채워줘야 게임오버 안당하는게 좋을까
아이템은?
아이템이 뭐가 있는지 알아야 뭘 할 것 같은데*/
