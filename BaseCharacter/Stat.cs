using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Stat 
{
    [SerializeField]private int baseValue;

    public List<int> modifers;

    public int GetValue()
    {
        int finalValue = baseValue;

        foreach(int modifer in modifers)
        {
            finalValue += modifer;
        }
        return finalValue;
    }

    public void SetDefaultValue(int _value)
    {
        baseValue = _value;
    }
    
    public void AddModifier(int _modifer)
    {
        modifers.Add(_modifer);
    }

    public void RemoveModifer(int _modifer)
    {
        modifers.Remove(_modifer);
    }
}
