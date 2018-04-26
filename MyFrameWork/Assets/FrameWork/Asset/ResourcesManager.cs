using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ResourcesManager : BaseManager<ResourcesManager>
{
    IABScenceManager manager;

    protected override void Awake()
    {
        //运行开始加载manifest
        IABManifestLoader manifest = new IABManifestLoader(ManifestLoadComplete);
        StartCoroutine(manifest.LoadManifet());
    }

    /// <summary>
    /// 加载完manifest
    /// </summary>
    /// <param name="assetManifest"></param>
    private void ManifestLoadComplete(AssetBundleManifest assetManifest)
    {
        //IABManager manager = new IABManager(sceneName);
        //manager.Init(bundleName, null, LoadCallBack);
        //StartCoroutine(manager.LoadAssetBundles(bundleName));
    }

    //public T Load<T>(string sceneName, string bundleName)
    //{
        
        
    //}

    private void LoadCallBack(string scenceName, string bundleName)
    {
        
    }
}
