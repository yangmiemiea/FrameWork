using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单个Obj
/// </summary>
public class AssetObj
{
    public List<UnityEngine.Object> objs;

    /// <summary>
    /// params 修饰参数 可传入任意数量该类型参数
    /// </summary>
    /// <param name="tmpobj"></param>
    public AssetObj(params UnityEngine.Object[] tmpobj)
    {
        objs = new List<UnityEngine.Object>();
        objs.AddRange(tmpobj);
    }

    public void ReleaseObj()
    {
        for (int i = 0; i < objs.Count; i++)
        {
            Resources.UnloadAsset(objs[i]);
        }
    }
}

/// <summary>
/// 单个AB包所有Obj
/// </summary>
public class AssetResObj
{
    public Dictionary<string, AssetObj> resObjs;

    public AssetResObj(string name, AssetObj tmp)
    {
        resObjs = new Dictionary<string, AssetObj>();
        resObjs.Add(name, tmp);
    }

    /// <summary>
    /// 添加Obj
    /// </summary>
    /// <param name="name"></param>
    /// <param name="tmpObj"></param>
    public void AddResObj(string name, AssetObj tmpObj)
    {
        resObjs.Add(name, tmpObj);
    }

    /// <summary>
    /// 释放单个资源
    /// </summary>
    /// <param name="name"></param>
    public void ReleaseResObj(string name)
    {
        if (resObjs.ContainsKey(name))
        {
            AssetObj tmpObj = resObjs[name];
            tmpObj.ReleaseObj();
        }
        else
        {
            Debug.Log("Release object name is not exit == " + name);
        }
    }

    /// <summary>
    /// 释放所有资源
    /// </summary>
    public void ReleaseAllResObj()
    {
        List<string> keys = new List<string>();
        keys.AddRange(resObjs.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            ReleaseResObj(keys[i]);
        }
    }


    public List<UnityEngine.Object> GetResObj(string name)
    {
        if (resObjs.ContainsKey(name))
        {
            AssetObj tmpObj = resObjs[name];
            return tmpObj.objs;
        }
        else
        {
            return null;
        }
    }
}

public delegate void LoadAssetBundleCallBack(string scenceName, string bundleName);

/// <summary>
/// 单个场景所有包的管理
/// </summary>
public class IABManager
{
    //包的缓存
    Dictionary<string, IABRelationManager> loadHelper = new Dictionary<string, IABRelationManager>();

    //包内资源的缓存 每次获取资源的时候使用
    Dictionary<string, AssetResObj> loadObj = new Dictionary<string, AssetResObj>();

    string scenceName;

    public IABManager(string scenceName)
    {
        this.scenceName = scenceName;
    }

    /// <summary>
    /// 是否加载了某个包
    /// </summary>
    /// <param name="bundleName"></param>
    /// <returns></returns>
    public bool IsLoadingAssetBundle(string bundleName)
    {
        if (!loadHelper.ContainsKey(bundleName))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void DisposeResObj(string bundleName)
    {
        if (loadObj.ContainsKey(bundleName))
        {
            AssetResObj tmpObj = loadObj[bundleName];
            tmpObj.ReleaseAllResObj();
        }
        Resources.UnloadUnusedAssets();
    }

    public void DisposeResObj(string bundleName, string resName)
    {
        if (loadObj.ContainsKey(bundleName))
        {
            AssetResObj tmpObj = loadObj[bundleName];
            tmpObj.ReleaseResObj(resName);
        }
    }

    /// <summary>
    /// 释放所有OBJ资源
    /// </summary>
    public void DisposeAllObj()
    {
        List<string> keys = new List<string>();
        keys.AddRange(loadObj.Keys);
        for (int i = 0; i < loadObj.Count; i++)
        {
            DisposeResObj(keys[i]);
        }
        loadObj.Clear();
    }

    /// <summary>
    /// 卸载包以及未被其他包依赖的依赖包
    /// </summary>
    /// <param name="bundleName"></param>
    public void DisposeBundle(string bundleName)
    {
        if (loadHelper.ContainsKey(bundleName))
        {
            IABRelationManager loader = loadHelper[bundleName];

            List<string> depences = loader.GetDependence();

            //先卸载依赖关系
            for (int i = 0; i < depences.Count; i++)
            {
                if (loadHelper.ContainsKey(depences[i]))
                {
                    IABRelationManager dependence = loadHelper[depences[i]];

                    if (dependence.RemoveRefference(bundleName))
                    {
                        DisposeBundle(dependence.GetBundleName());
                    }
                }
            }
            if(loader.GetRefference().Count <= 0)
            {
                loader.Dispose();

                loadHelper.Remove(bundleName);
            }
        }
    }

    /// <summary>
    /// 释放所有RelationManager对象
    /// </summary>
    public void DisposeAllBundle()
    {
        List<string> keys = new List<string>();
        keys.AddRange(loadHelper.Keys);

        for (int i = 0; i < loadHelper.Count; i++)
        {
            IABRelationManager loader = loadHelper[keys[i]];

            loader.Dispose();
        }
        loadHelper.Clear();
    }

    /// <summary>
    /// 卸载所有资源
    /// </summary>
    public void DisposeAllBundleAndRes()
    {
        DisposeAllObj();

        DisposeAllBundle();
    }

    #region 由下层提供的API

    public void DebugAssetBundle(string bundleName)
    {
        if (loadHelper.ContainsKey(bundleName))
        {
            IABRelationManager loader = loadHelper[bundleName];
            loader.DebuggerAsset();
        }
    }

    /// <summary>
    /// 是否加载完毕
    /// </summary>
    /// <param name="bundleName"></param>
    /// <returns></returns>
    public bool IsLoadingFinish(string bundleName)
    {
        if (loadHelper.ContainsKey(bundleName))
        {
            IABRelationManager loader = loadHelper[bundleName];
            return loader.IsBundleLoadFinish();
        }
        else
        {
            Debug.Log("IABRelation no contain bundle == " + bundleName);
            return false;
        }
    }

    /// <summary>
    /// 获取单个资源
    /// </summary>
    /// <param name="bundleName"></param>
    /// <param name="resName"></param>
    /// <returns></returns>
    public UnityEngine.Object GetSingleResources(string bundleName, string resName)
    {
        if (loadObj.ContainsKey(bundleName))
        {
            AssetResObj tmpRes = loadObj[bundleName];
            List<UnityEngine.Object> tmpObj = tmpRes.GetResObj(resName);

            if (tmpObj != null)
            {
                return tmpObj[0];
            }
        }

        //表示已经加载过bundle
        if (loadHelper.ContainsKey(bundleName))
        {
            IABRelationManager loader = loadHelper[bundleName];
            UnityEngine.Object tmpObj = loader.GetSingleResources(resName);

            AssetObj tmpAssetObj = new AssetObj(tmpObj);

            if (loadObj.ContainsKey(bundleName))
            {
                AssetResObj tmpRes = loadObj[bundleName];
                tmpRes.AddResObj(resName, tmpAssetObj);
            }
            else
            {
                //没有加载过这个包
                AssetResObj tmpRes = new AssetResObj(resName, tmpAssetObj);
                loadObj.Add(bundleName, tmpRes);
            }
            return tmpObj;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 获取多个资源
    /// </summary>
    /// <param name="bundleName"></param>
    /// <param name="resName"></param>
    /// <returns></returns>
    public UnityEngine.Object[] GetMutiResources(string bundleName, string resName)
    {
        if (loadObj.ContainsKey(bundleName))
        {
            AssetResObj tmpRes = loadObj[bundleName];

            List<UnityEngine.Object> tmpObj = tmpRes.GetResObj(resName);
            if(tmpObj != null)
            {
                return tmpObj.ToArray();
            }
        }
        //表示已经加载过bundle
        if (loadHelper.ContainsKey(bundleName))
        {
            IABRelationManager loader = loadHelper[bundleName];
            UnityEngine.Object[] tmpObj = loader.GetMutiResources(resName);

            AssetObj tmpAssetObj = new AssetObj(tmpObj);
            //缓存里面是否已经有这个包
            if (loadObj.ContainsKey(bundleName))
            {
                AssetResObj tmpRes = loadObj[bundleName];
                tmpRes.AddResObj(resName, tmpAssetObj);
            }
            else
            {
                //没有加载过这个包
                AssetResObj tmpRes = new AssetResObj(resName, tmpAssetObj);
                loadObj.Add(bundleName, tmpRes);
            }
            return tmpObj;
        }
        else { return null; }
    }
    #endregion

    /// <summary>
    /// 初始化 -- 创建包的资源加载器（未实际加载）
    /// </summary>
    /// <param name="bundleName">ab标签</param>
    /// <param name="progress">加载时每帧回调</param>
    /// <param name="callback">回调</param>
    public void Init(string bundleName, LoadProgrecess progress, LoadAssetBundleCallBack callback)
    {   
        //如果没加载过
        if (!loadHelper.ContainsKey(bundleName))
        {
            IABRelationManager loader = new IABRelationManager();
            //加载
            loader.Initial(bundleName, null, progress);
            //添加到loadHelper
            loadHelper.Add(bundleName, loader);

            //提供给上层 启动 IEnumerator LoadAssetBundles(string BundleName) 返回当前场景名 ab标签名
            callback(scenceName, bundleName);
        }
        else
        {
            Debug.Log("IABManager has contain bundle name == " + bundleName);
        }
    }

    /// <summary>
    /// 获取某个包的所有依赖包
    /// </summary>
    /// <param name="bundleName">ab标签</param>
    /// <returns></returns>
    string[] GetDependences(string bundleName)
    {
        return IABManifestLoader.Instance.GetDepences(bundleName);
    }

    private IEnumerator LoadAssetBundleDependences(string bundleName, string refName, LoadProgrecess progress)
    {
        if (!loadHelper.ContainsKey(bundleName))
        {
            IABRelationManager loader = new IABRelationManager();
            //初始化IABRelationManager  创建bundleName包的加载对象 progress加载的回调
            loader.Initial(bundleName, null, progress);
            if (refName != null)
            {
                loader.AddRefference(refName);
            }
            loadHelper.Add(bundleName, loader);
            yield return LoadAssetBundles(bundleName);
        }
        else
        {
            if (refName != null)
            {
                IABRelationManager loader = loadHelper[bundleName];
                loader.AddRefference(bundleName);
            }
        }
    }

    /// <summary>
    /// 加载包及依赖包 需先加载manifest
    /// </summary>
    /// <param name="bundleName">ab标签</param>
    /// <returns></returns>
    public IEnumerator LoadAssetBundles(string bundleName)
    {
        while (!IABManifestLoader.Instance.IsLoadFinish())
        {
            yield return null;
        }
        IABRelationManager loader = loadHelper[bundleName];
        string[] depences = GetDependences(bundleName);

        for (int i = 0; i < depences.Length; i++)
        {
            //加载依赖包
            yield return LoadAssetBundleDependences(depences[i], bundleName, loader.GetProgress());
        }
        yield return loader.LoadAssetBundle();
    }
}
