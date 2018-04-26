using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 路径工具
/// </summary>
public class IPathTool {

    /// <summary>
    /// 获取当前build平台
    /// </summary>
    /// <param name="platform"></param>
    /// <returns></returns>
    public static string GetPlatformFolderName(RuntimePlatform platform)
    {
        switch (platform)
        {
            case RuntimePlatform.Android:
                return "Android";

            case RuntimePlatform.IPhonePlayer:
                return "IOS";

            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                return "Windows";

            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.OSXPlayer:
                return "OSX";

            default:
                return null;
        }
    }

    /// <summary>
    /// 获取当前资源打包文件夹路径（streamingAssetsPath or dataPath）
    /// </summary>
    /// <returns></returns>
    public static string GetAppFilePath()
    {
        string tmpPath = "";
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            tmpPath = Application.streamingAssetsPath;
        }
        else
        {
            tmpPath = Application.dataPath;
        }
        return tmpPath;
    }

    /// <summary>
    /// 获取当前资源打包完整路径
    /// </summary>
    /// <returns></returns>
    public static string GetAssetBundlePath()
    {
        //Windows Android...
        string platFolder = GetPlatformFolderName(Application.platform);

        string allPath = GetAppFilePath() + "/" + platFolder;

        return allPath;
    }

    /// <summary>
    /// 获取AssetBundle文件夹路径
    /// </summary>
    /// <returns></returns>
    public static string GetWWWAssetBundlePath()
    {
        string tmpStr = "";
        
        if(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            tmpStr = "file:///" + GetAssetBundlePath();
        }
        else
        {
            string tmpPath = GetAssetBundlePath();
#if UNITY_ANDROID
            tmpStr = "jar:file://" + tmpPath;
#elif UNITY_STANDALONE_WIN
            tmpStr = "jar:file:///" + tmpPath;
#else
            tmpStr = "jar:file://" + tmpPath;
#endif
        }
        return tmpStr;
    }
}
