using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int columns;
    public int rows;

    ItemSlot[,] slots;

    public void Initialize()
    {
        slots = new ItemSlot[rows, columns];
    }
}
