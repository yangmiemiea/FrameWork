using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class IABScenceManager
{
    IABManager abManager;

    public IABScenceManager(string scenceName)
    {
        ReadConfiger(scenceName);
    }

    //存储资源 与资源路径 对应关系
    Dictionary<string, string> allAsset = new Dictionary<string, string>();

    /// <summary>
    /// 读取场景配置文件
    /// </summary>
    /// <param name="scenceName"></param>
    public void ReadConfiger(string scenceName)
    {
        string textFileName = "Record.txt";
        string path = IPathTool.GetAssetBundlePath() + "/" + scenceName + textFileName;

        abManager = new IABManager(scenceName);
        ReadConfig(path);
    }

    private void ReadConfig(string path)
    {
        FileStream fs = new FileStream(path, FileMode.Open);
        StreamReader br = new StreamReader(fs);
        string line = br.ReadLine();
        int allCount = int.Parse(line);
        for (int i = 0; i < allCount; i++)
        {
            string tmpStr = br.ReadLine();
            string[] tmpArr = tmpStr.Split(" ".ToCharArray());
            allAsset.Add(tmpArr[1], tmpArr[0]);
        }
        br.Close();
        fs.Close();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="bundleName">ab标签</param>
    /// <param name="progress">加载时回调</param>
    /// <param name="callback">回调</param>
    public void Init(string bundleName, LoadProgrecess progress, LoadAssetBundleCallBack callback)
    {
        if (allAsset.ContainsKey(bundleName))
        {
            abManager.Init(bundleName, progress, callback);
        }
        else
        {
            Debug.Log("Dont contain the bundle == " + bundleName);
        }
    }

    #region 由下层提供的功能

    /// <summary>
    /// 加载资源 需先加载完manifest
    /// </summary>
    /// <param name="bundleName"></param>
    /// <returns></returns>
    public IEnumerator LoadAssetSys(string bundleName)
    {
        yield return abManager.LoadAssetBundles(bundleName);
    }

    public Object GetSingleResources(string bundleName, string resName)
    {
        if (allAsset.ContainsKey(bundleName))
        {
            return abManager.GetSingleResources(bundleName, resName);
        }
        else
        {
            Debug.Log("Dont contain the bundle == " + bundleName);
            return null;
        }
    }

    public Object[] GetMutiResources(string bundleName, string resName)
    {
        if (allAsset.ContainsKey(bundleName))
        {
            return abManager.GetMutiResources(bundleName, resName);
        }
        else
        {
            Debug.Log("Dont contain the bundle == " + bundleName);
            return null;
        }
    }

    public void DisposeResObj(string bundleName, string res)
    {
        if (allAsset.ContainsKey(bundleName))
        {
            abManager.DisposeResObj(bundleName, res);
        }
        else
        {
            Debug.Log("Dont contain the bundle == " + bundleName);
        }
    }

    public void DisposeBundleRes(string bundleName)
    {
        if (allAsset.ContainsKey(bundleName))
        {
            abManager.DisposeResObj(bundleName);
        }
        else
        {
            Debug.Log("Dont contain the bundle == " + bundleName);
        }
    }

    public void DisposeAllRes()
    {
        abManager.DisposeAllObj();
    }

    public void DisposeBunlde(string bundleName)
    {
        if (allAsset.ContainsKey(bundleName))
        {
            abManager.DisposeBundle(bundleName);
        }
    }

    public void DisposeAllBunlde()
    {
        abManager.DisposeAllBundle();

        allAsset.Clear();
    }

    public void DisposeAllBundleAndRes()
    {
        abManager.DisposeAllBundleAndRes();

        allAsset.Clear();
    }

    public void DebugAllAsset()
    {
        List<string> keys = new List<string>();

        keys.AddRange(allAsset.Keys);

        for (int i = 0; i < keys.Count; i++)
        {
            abManager.DebugAssetBundle(allAsset[keys[i]]);
        }
    }
    #endregion
}
