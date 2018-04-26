using System;
using UnityEngine;

public class UIPrefabLoader : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    private GameObject instance;

    private Action<GameObject> waitLoad;

    /// <summary>
    /// Wait the instance.
    /// </summary>
    public void Wait(Action<GameObject> wait)
    {
        if (this.instance == null)
        {
            this.waitLoad = wait;
        }
        else
        {
            wait(this.instance);
        }
    }

    private void OnEnable()
    {
        if (this.instance == null)
        {
            this.instance = GameObject.Instantiate(
                this.prefab, this.transform);
            if (this.waitLoad != null)
            {
                this.waitLoad(this.instance);
                this.waitLoad = null;
            }
        }
    }
}

