//------------------------------------------------------------------------------
// This file is part of MistLand project in Nirvana.
// Copyright © 2016-2016 Nirvana Technology Co., Ltd.
// All Right Reserved.
//------------------------------------------------------------------------------

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used to adapt the difference resolution.
/// </summary>
[RequireComponent(typeof(CanvasScaler))]
[ExecuteInEditMode]
public sealed class ResolutionAdapter : MonoBehaviour
{
	private Canvas canvas;
	private CanvasScaler scaler;

	private void Start()
	{
		this.canvas = this.GetComponent<Canvas>();
		if (null == this.canvas || !this.canvas.isRootCanvas)
		{
			return;
		}

		this.AdaptResolution();
	}

#if UNITY_EDITOR
    private void Update()
    {
        this.AdaptResolution();
    }
#endif

	private void AdaptResolution()
	{
#if UNITY_EDITOR
        var prefabType = PrefabUtility.GetPrefabType(this.gameObject);
        if (prefabType == PrefabType.Prefab)
        {
            return;
        }
#endif

		if (null == this.scaler)
		{
			this.scaler = this.GetComponent<CanvasScaler>();
		}

		var radio = (float)Screen.width / Screen.height;
		var refRadio = this.scaler.referenceResolution.x / this.scaler.referenceResolution.y;
		if (radio > refRadio)
		{
			this.scaler.matchWidthOrHeight = 1.0f;
		}
		else
		{
			this.scaler.matchWidthOrHeight = 0.0f;
		}
	}
}

