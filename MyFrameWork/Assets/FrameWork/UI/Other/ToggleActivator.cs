using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleActivator : BaseBehaviour
{
    public GameObject[] actives;

    public GameObject[] deactive;

    protected override void Awake()
    {
        Toggle toggle = GetComponent<Toggle>();
        for (int i = 0; i < actives.Length; i++)
        {
            actives[i].SetActive(toggle.isOn);
        }
        for (int i = 0; i < deactive.Length; i++)
        {
            deactive[i].SetActive(!toggle.isOn);
        }

        toggle.onValueChanged.AddListener(
            (b) =>
            {
                for (int i = 0; i < actives.Length; i++)
                {
                    actives[i].SetActive(b);
                }
                for (int i = 0; i < deactive.Length; i++)
                {
                    deactive[i].SetActive(!b);
                }
            });
    }
}
