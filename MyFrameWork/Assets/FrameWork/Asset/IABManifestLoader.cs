using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public delegate void LoadManifestFinish(AssetBundleManifest assetManifest);

/// <summary>
/// Manifest文件的加载(全局一份)
/// </summary>
public class IABManifestLoader
{
    public AssetBundleManifest assetManifest;   //全局一份

    AssetBundle manifestLoader;

    string manifestPath;

    bool isLoadFinish;

    LoadManifestFinish loadManifestFinish;

    public IABManifestLoader(LoadManifestFinish loadManifestFinish = null)
    {
        instance = this;

        this.loadManifestFinish = loadManifestFinish;

        assetManifest = null;

        manifestLoader = null;

        isLoadFinish = false;

        manifestPath = IPathTool.GetAssetBundlePath() + "/" + IPathTool.GetPlatformFolderName(Application.platform);
    }

    /// <summary>
    /// 加载Manifest
    /// </summary>
    /// <returns></returns>
    public IEnumerator LoadManifet()
    {
        WWW manifest = new WWW(manifestPath);
        yield return manifest;
        if (!string.IsNullOrEmpty(manifest.error))
        {
            Debug.Log(manifest.error);
        }
        else
        {
            if(manifest.progress >= 1.0f)
            {
                manifestLoader = manifest.assetBundle;
                assetManifest = manifestLoader.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
                isLoadFinish = true;
                if (loadManifestFinish != null)
                {
                    loadManifestFinish(assetManifest);
                }
            }
        }
    }

    /// <summary>
    /// 设置加载完成时回调
    /// </summary>
    /// <param name="callback"></param>
    public void SetLoadCompleteCallBack(LoadManifestFinish callback)
    {
        this.loadManifestFinish = callback;
    }

    /// <summary>
    /// 获取依赖关系
    /// </summary>
    /// <param name="name">包名</param>
    /// <returns></returns>
    public string[] GetDepences(string bundleName)
    {
        return assetManifest.GetAllDependencies(bundleName);
    }

    /// <summary>
    /// 卸载资源
    /// </summary>
    public void UnloadManifest()
    {
        manifestLoader.Unload(true);
    }

    /// <summary>
    /// 设置Manifest路径 已有默认值
    /// </summary>
    /// <param name="path"></param>
    public void SetManifestPath(string path)
    {
        manifestPath = path;
    }

    private static IABManifestLoader instance = null;

    public static IABManifestLoader Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new IABManifestLoader();
            }
            return instance;
        }
    }

    /// <summary>
    /// Manifest是否加载完毕
    /// </summary>
    /// <returns></returns>
    public bool IsLoadFinish()
    {
        return isLoadFinish;
    }
}