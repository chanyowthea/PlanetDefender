using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SingletonManager
{
    public static SQLiteHelper SqliteHelper{private set; get; }

    public static void Init()
    {
        SqliteHelper = new SQLiteHelper("Game");
    }

    public static void Clear()
        {

        }
}
