using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPair
{
    public ItemPair() { }

    public ItemPair(int id, int num)
    {
        _ID = id;
        _Number = num;
    }

    public int _ID;
    public int _Number;
}
