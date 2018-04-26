using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Tool
{
    public static Transform FindFather(Transform target, Func<Transform,bool> func)
    {
        if (target.parent == null)
        {
            return target;
        }
        if (func(target.parent))
        {
            return target.parent;
        }
        else
        {
            target = FindFather(target.parent, func);
        }
        return target;
    }
}

