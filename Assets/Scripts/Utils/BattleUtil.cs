using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUtil : MonoBehaviour
{
    public static int CalcDamage(int attack, int defense)
    {
        return Mathf.CeilToInt(attack * (1 - defense / (float)(defense + attack))); 
    }
}
