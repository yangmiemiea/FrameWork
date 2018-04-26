using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EventBindClick))]
public class EventBindClickEditor : Editor
{
    EventBindClick bind;
    private void OnEnable()
    {
        bind = (EventBindClick)target;
        bind.table = Tool.FindFather(bind.transform, a =>
        {
            return a.GetComponent<EventTable>() != null;
        }).GetComponent<EventTable>();
    }

    public override void OnInspectorGUI()
    {
        EventTable eventTable = bind.table;
        //赋值
        if (eventTable != null)
        {
            List<string> eventList = new List<string>();
            for (int i = 0; i < eventTable.events.Length; i++)
            {
                eventList.Add(eventTable.events[i].eventName);
            }
            string[] events = eventList.ToArray();
            bind.index = EditorGUILayout.Popup("Event logic:", bind.index, events);
            if (events.Length > 0)
            {
                foreach (var item in eventTable.events)
                {
                    if (item.eventName == events[bind.index])
                    {
                        //进行绑定
                        bind.bindEvent = item;
                    }
                }
            }
        }
        //刷新
        base.OnInspectorGUI();
    }
}
