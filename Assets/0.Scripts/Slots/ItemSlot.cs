using UnityEngine;

public class ItemSlot 
{
    //이칸에 들어있는 아이템 정보
    [SerializeField] ItemContainer item;
    // 이 칸만의 정보
    [SerializeField] ItemType containType;
    [SerializeField] int currentStack;

    public virtual bool Containable(ItemContainer newItem)
    {
        if (item)   return true;
        else        return false;
    }
}