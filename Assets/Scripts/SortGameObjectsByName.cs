using System.Collections.Generic;
using UnityEngine;

public class SortGameObjectsByName : IComparer<GameObject>
{
    public int Compare(GameObject x, GameObject y)
    {
        if (x == null || y == null) return 0;
        return string.Compare(x.name, y.name);
    }
}
