using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 包之间依赖关系管理
/// </summary>
public class IABRelationManager
{
    IABLoader abLoader;

    List<string> dependenceBundle;  //依赖包

    List<string> referBundle;       //被依赖包

    LoadProgrecess loadProgerss;

    LoadFinish loadFinish;

    string bundleName;

    bool IsLoadFinish;

    public IABRelationManager()
    {
        dependenceBundle = new List<string>();
        referBundle = new List<string>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bundleName"></param>
    /// <param name="progress"></param>
    public IABRelationManager(string bundleName, LoadFinish finish = null, LoadProgrecess progress = null)
    {
        dependenceBundle = new List<string>();
        referBundle = new List<string>();
        Initial(bundleName, finish, progress);
    }

    /// <summary>
    /// 初始化(创建IABLoader)
    /// </summary>
    /// <param name="bundleName"></param>
    /// <param name="progress"></param>
    public void Initial(string bundleName, LoadFinish finish, LoadProgrecess progress)
    {
        IsLoadFinish = false;
        this.bundleName = bundleName;
        this.loadProgerss = progress;

        loadFinish = BundleLoadFinish;
        if (finish != null)
        {
            loadFinish += finish;
        }
        //创建包的加载对象
        abLoader = new IABLoader(bundleName, loadFinish, progress);
    }

    private void BundleLoadFinish(string bundleName)
    {
        IsLoadFinish = true;
    }

    public bool IsBundleLoadFinish()
    {
        return IsLoadFinish;
    }

    public LoadProgrecess GetProgress()
    {
        return loadProgerss;
    }

    public string GetBundleName()
    {
        return bundleName;
    }

    #region 被依赖包管理

    /// <summary>
    /// 获取所有被依赖包
    /// </summary>
    /// <returns></returns>
    public List<string> GetRefference()
    {
        return referBundle;
    }

    /// <summary>
    /// 添加被依赖关系
    /// </summary>
    /// <param name="bundleName"></param>
    public void AddRefference(string bundleName)
    {
        if (!referBundle.Contains(bundleName))
        {
            referBundle.Add(bundleName);
        }
        else
        {
            Debug.Log("the refference(" + bundleName + ") is already added");
        }
    }

    /// <summary>
    /// 移除被依赖包
    /// </summary>
    /// <param name="bundleName"></param>
    /// <returns>是否存在被依赖包</returns>
    public bool RemoveRefference(string bundleName)
    {
        for (int i = 0; i < referBundle.Count; i++)
        {
            if (referBundle.Contains(bundleName))
            {
                referBundle.RemoveAt(i);
            }
        }
        if (referBundle.Count <= 0)
        {
            Dispose();
            referBundle.Clear();
            return true;
        }
        return false;
    }

    #endregion

    #region 依赖包管理
    /// <summary>
    /// 获取当前包依赖的所有包
    /// </summary>
    /// <returns></returns>
    public List<string> GetDependence()
    {
        return dependenceBundle;
    }

    /// <summary>
    /// 添加依赖关系
    /// </summary>
    /// <param name="bundleName"></param>
    public void AddDependence(string bundleName)
    {
        if (!dependenceBundle.Contains(bundleName))
        {
            referBundle.Add(bundleName);
        }
        else
        {
            Debug.Log("the dependence(" + bundleName + ") is already added");
        }
    }

    /// <summary>
    /// 设置多个依赖关系
    /// </summary>
    /// <param name="dependences"></param>
    public void SetDependences(string[] dependences)
    {
        if (dependenceBundle.Count > 0)
        {
            dependenceBundle.AddRange(dependences);
        }
    }

    /// <summary>
    /// 移除依赖包
    /// </summary>
    /// <param name="bundleName"></param>
    /// <returns>是否存在依赖包</returns>
    public bool RemoveDependence(string bundleName)
    {
        for (int i = 0; i < dependenceBundle.Count; i++)
        {
            if (dependenceBundle.Contains(bundleName))
            {
                dependenceBundle.RemoveAt(i);
            }
        }
        if (dependenceBundle.Count <= 0)
        {
            Dispose();
            dependenceBundle.Clear();
            return true;
        }
        return false;
    }

    #endregion

    #region 下层提供的功能

    /// <summary>
    /// 测试
    /// </summary>
    public void DebuggerAsset()
    {
        if (abLoader != null)
        {
            abLoader.DebugLoader();
        }
    }

    /// <summary>
    /// 加载资源  调用前需初始化 
    /// </summary>
    /// <returns></returns>
    public IEnumerator LoadAssetBundle()
    {
        if (abLoader != null)
        {
            yield return abLoader.CommonLoad();
        }
        else
        {
            yield return null;
        }
    }

    /// <summary>
    /// 获取单个资源
    /// </summary>
    /// <param name="resName">资源名称</param>
    /// <returns></returns>
    public UnityEngine.Object GetSingleResources(string resName)
    {
        if (abLoader != null)
        {
            return abLoader.GetResources(resName);
        }
        return null;
    }

    /// <summary>
    /// 加载嵌套资源（自身及所有独立出去子预制体）
    /// </summary>
    /// <param name="resName"></param>
    /// <returns></returns>
    public UnityEngine.Object[] GetMutiResources(string resName)
    {
        if (abLoader != null)
        {
            return abLoader.GetMutiRes(resName);
        }
        return null;
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    public void Dispose()
    {
        if (abLoader != null)
        {
            abLoader.Dispose();
        }
    }
    #endregion
}
