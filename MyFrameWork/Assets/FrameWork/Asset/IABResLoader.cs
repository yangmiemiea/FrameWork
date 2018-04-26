using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 单个包的“资源”加载
/// </summary>
public class IABResLoader : IDisposable
{
    private AssetBundle ABRes;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tmpBundle">assetbundle对象</param>
    public IABResLoader(AssetBundle tmpBundle)
    {
        ABRes = tmpBundle;
    }

    /// <summary>
    /// 索引器（加载单个资源）
    /// </summary>
    /// <param name="resName">资源名称</param>
    /// <returns></returns>
    public UnityEngine.Object this[string resName]
    {
        get
        {
            if (this.ABRes == null || !this.ABRes.Contains(resName))
            {
                Debug.LogError("res not contain == " + resName);
            }
            return ABRes.LoadAsset(resName);
        }
    }

    /// <summary>
    /// 加载嵌套资源（自身及所有独立出去子预制体）
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="resName">资源名称</param>
    /// <returns></returns>
    public UnityEngine.Object[] LoadResources(string resName)
    {
        if(this.ABRes == null || !this.ABRes.Contains(resName))
        {
            Debug.LogError("res not contain == " + resName);
            return null;
        }
        //加载所有相关资源
        return ABRes.LoadAssetWithSubAssets(resName);
    }

    /// <summary>
    /// 卸载单个资源
    /// </summary>
    /// <param name="resObj"></param>
    public void UnloadRes(UnityEngine.Object resObj)
    {
        Resources.UnloadAsset(resObj);
    }

    /// <summary>
    /// 释放当前ab包
    /// </summary>
    public void Dispose()
    {
        ABRes.Unload(false);
    }

    /// <summary>
    /// 调试
    /// </summary>
    public void DebugAllRes()
    {
        string[] tmpAssetName = ABRes.GetAllAssetNames();
        for (int i = 0; i < tmpAssetName.Length; i++)
        {
            Debug.Log("ABRes Contain asset name == " + tmpAssetName[i]);
        }
    }
}
