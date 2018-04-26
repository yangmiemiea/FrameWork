using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseData {

    private BaseData instance;

    public BaseData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BaseData();
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }

    public BaseData()
    {
        this.__init();
    }

    public virtual void __init() { }

    public virtual void __delete() { }

    public virtual void Release()
    {
        __delete();
    }
}
