using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomUtil : TSingleton<RandomUtil>
{
    System.Random _random; 
    public System.Random random
    {
        get
        {
            if (_random == null)
            {
                _random = new System.Random(19930108); 
            }
            return _random;
        }
    }
}
