using UnityEngine;

public enum ItemType
{
    None,
    Tool,           // 도구
    Equipment,      // 의복
    Gem,            // 보석
    Mineral,        // 광물
    Herb,           // 약초
    Food,           // 요리
    Book,           // 책
    Structure,      // 시설물
    Hallucinogen,   // 환각제
    Record,         // 기록
    Misc,            // 기타
    Length
}

/*public enum ItemType
{
    Equipment, Consumable, Material, Miscellaneous, Quest, Impotrtant, Resource,
    Length
}*/

[CreateAssetMenu(fileName = "ItemContainer", menuName = "Item/ItemBase")]
public class ItemContainer : InfoContainer
{
    public ItemType type;
    public int maxStack;
    public float weight;
}