using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class EventBind :BaseBehaviour {

    public EventTable table;

    [HideInInspector]
    public UIEvent bindEvent;

    [HideInInspector]
    public int index;
   
}
