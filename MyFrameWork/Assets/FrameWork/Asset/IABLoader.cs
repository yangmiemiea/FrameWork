using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

//向上层传递数据
/// <summary>
/// 每帧回调
/// </summary>
/// <param name="bundleName">包名</param>
/// <param name="progress">进度</param>
public delegate void LoadProgrecess(string bundleName, float progress);

/// <summary>
/// AB包加载完成时的回调
/// </summary>
/// <param name="bundleName">包名</param>
public delegate void LoadFinish(string bundleName);

/// <summary>
/// 单个“包”的加载
/// </summary>
public class IABLoader
{
    private IABResLoader abResLoader;

    private string bundleName;          //ab标签

    private string commonBundlePath;    //包路径

    private WWW commonLoader;

    private float commonResLoaderProgress;  //资源加载进度

    private LoadProgrecess loadProgrecess;

    private LoadFinish loadFinish;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bundleName">ab标签</param>
    public IABLoader(string bundleName)
    {
        this.bundleName = bundleName;
        this.commonBundlePath = IPathTool.GetWWWAssetBundlePath() + "/" + bundleName;
        this.commonResLoaderProgress = 0;
        this.abResLoader = null;

        this.loadProgrecess = null;
        this.loadFinish = null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bundleName">ab标签</param>
    /// <param name="tmpFinish">完成时回调</param>
    /// <param name="tmpProgress">加载时每帧回调</param>
    public IABLoader(string bundleName, LoadFinish tmpFinish = null, LoadProgrecess tmpProgress = null)
    {
        this.bundleName = bundleName;
        this.commonBundlePath = IPathTool.GetWWWAssetBundlePath() + "/" + bundleName;
        this.commonResLoaderProgress = 0;
        this.abResLoader = null;

        this.loadProgrecess = tmpProgress;
        this.loadFinish = tmpFinish;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tmpProgress">加载时每帧回调</param>
    /// <param name="tmpFinish">完成时回调</param>
    public IABLoader(LoadProgrecess tmpProgress = null, LoadFinish tmpFinish = null)
    {
        this.bundleName = "";
        this.commonBundlePath = "";
        this.commonResLoaderProgress = 0;
        this.abResLoader = null;

        this.loadProgrecess = tmpProgress;
        this.loadFinish = tmpFinish;
    }

    /// <summary>
    /// 设置bundle
    /// </summary>
    /// <param name="bundleName"></param>
    public void SetBundleName(string bundleName)
    {
        this.bundleName = bundleName;
        this.commonBundlePath = IPathTool.GetWWWAssetBundlePath() + "/" + bundleName;
    }

    /// <summary>
    /// 协程加载资源
    /// </summary>
    /// <returns></returns>
    public IEnumerator CommonLoad()
    {
        if (string.IsNullOrEmpty(commonBundlePath))
        {
            yield return null;
        }
        commonLoader = new WWW(commonBundlePath);
        while (!commonLoader.isDone)
        {
            commonResLoaderProgress = commonLoader.progress;
            yield return commonLoader.progress;
            if(loadProgrecess != null)
            {
                loadProgrecess(bundleName, commonResLoaderProgress);
            }
            commonResLoaderProgress = commonLoader.progress;
        }
        //加载完成        
        if (commonResLoaderProgress >= 1.0)
        {
            //拿到加载的bundle创建IABResLoader对象
            abResLoader = new IABResLoader(commonLoader.assetBundle);
            if (loadProgrecess != null)
            {
                loadProgrecess(bundleName, commonResLoaderProgress);
            }
            if (loadFinish != null)
            {
                loadFinish(bundleName);
            }
        }
        else
        {
            Debug.LogError("load bundle error == " + bundleName);
        }
        //释放WWW资源
        commonLoader = null;
    }

    #region  提供给上层
    /// <summary>
    /// 包加载完成时回调
    /// </summary>
    /// <param name="bundleName"></param>
    public void OnLoadFinish(LoadFinish tmpFinish)
    {
        this.loadFinish = tmpFinish;
    }

    public void OnLoad(LoadProgrecess tmpProgress)
    {
        this.loadProgrecess = tmpProgress;
    }
    #endregion

    #region 由下层提供

    /// <summary>
    /// 加载单个资源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="resName">资源名称</param>
    /// <returns></returns>
    public UnityEngine.Object GetResources(string resName)
    {
        if (abResLoader != null)
        {
            return abResLoader[resName];
        }

        return null;
    }

    /// <summary>
    /// 加载嵌套资源（自身及所有独立出去子预制体）
    /// </summary>
    /// <param name="resName"></param>
    /// <returns></returns>
    public UnityEngine.Object[] GetMutiRes(string resName)
    {
        if (abResLoader != null)
        {
            return abResLoader.LoadResources(resName);
        }
        return null;
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    public void Dispose()
    {
        if (abResLoader != null)
        {
            abResLoader.Dispose();
            abResLoader = null;
        }
    }

    /// <summary>
    /// 卸载单个资源
    /// </summary>
    /// <param name="tmpObj"></param>
    public void UnLoadAssetRes(UnityEngine.Object tmpObj)
    {
        if (abResLoader != null)
        {
            abResLoader.UnloadRes(tmpObj);
        }
        else
        {
            Resources.UnloadAsset(tmpObj);
        }
    }

    /// <summary>
    /// 测试
    /// </summary>
    public void DebugLoader()
    {
        if (abResLoader != null)
        {
            abResLoader.DebugAllRes();
        }
    }
    #endregion
}

