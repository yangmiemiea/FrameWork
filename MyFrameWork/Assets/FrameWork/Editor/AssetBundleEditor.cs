using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public static class AssetBundleEditor
{
    static string fileName = "Res";
    //资源存放路径
    static string assetsDir = Application.dataPath + "/" + fileName;
    //打包后存放路径
    static string outPath = IPathTool.GetAssetBundlePath();

    static string variant = "unity3d";

    [MenuItem("BundleTools/AutoBuildAll")]
    static void AuotBuildAll()
    {
        record.Clear();
        //清楚所有的AssetBundleName
        ClearAssetBundlesName();
        //设置指定路径下所有需要打包的assetbundlename
        SetAssetBundlesName(assetsDir);
        //
        CreatRecord(record);

        //打包所有需要打包的asset
        CreatOrConfirmPath(outPath);
        BuildPipeline.BuildAssetBundles(outPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        AssetDatabase.Refresh();
    }

    [MenuItem("BundleTools/BuildWithName")]
    static void BuildAllAssetBundle()
    {
        CreatOrConfirmPath(outPath);
        BuildPipeline.BuildAssetBundles(outPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 设置所有在指定路径下的AssetBundleName
    /// </summary>
    /// <param name="assetsDir"></param>
    private static void SetAssetBundlesName(string _assetspath)
    {
        CreatOrConfirmPath(_assetspath);
        //先获取指定路径下的所有Asset,包括子文件夹下的资源
        DirectoryInfo dir = new DirectoryInfo(_assetspath);
        //GetFileSystemInfos方法可以获取到指定目录下的所有文件及子文件夹
        FileSystemInfo[] files = dir.GetFileSystemInfos();
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i] is DirectoryInfo)//如果是文件夹则递归处理
            {
                SetAssetBundlesName(files[i].FullName);

                // Cube.prefab scenceone/ load / cube.prefab.ld
            }
            else if (!files[i].Name.EndsWith(".meta") && !files[i].Name.EndsWith(".txt"))//&& !files[i].Name.EndsWith(".txt"))//如果是文件的话，则设置AssetBundleName,并排除掉杂项文件
            {
                SetABName(files[i].FullName);   //逐个设置AssetBundleName
            }
        }
    }

    /// <summary>
    /// 设置单个AssetBundle的Name
    /// </summary>
    private static void SetABName(string assetPath)
    {
        //E:\MyFrameWork\Assets\Res\Prefabs\one 1\Cube.prefab
        //Prefabs\one 1\Cube
        FixedPath(ref assetPath);
        //Debug.Log(assetPath);
        //这个路径必须是以Assets开始的路径
        string importerPath = "Assets" + assetPath.Substring(Application.dataPath.Length);
        //Assets\Res\Prefabs\one 1\Cube.prefab
        AssetImporter assetImporter = AssetImporter.GetAtPath(importerPath); //得到Asset

        string assetName = assetPath.Substring(Application.dataPath.Length + 1 + fileName.Length + 1);

        MarkAllResources(assetPath, assetName);

        assetName = assetName.Remove(assetName.LastIndexOf('.'));

        assetImporter.assetBundleName = assetName;  //最终设置assetBundleName
        assetImporter.assetBundleVariant = variant; //设置文件后缀
    }

    static Dictionary<string, List<string>> record = new Dictionary<string, List<string>>();

    private static void MarkAllResources(string fullPath, string assetName)
    {
        if (assetName.EndsWith(".txt"))
        {
            return;
        }

        string fileName = assetName.Substring(0, assetName.IndexOf('/'));

        string filePath = fullPath.Substring(0, fullPath.IndexOf(fileName) + fileName.Length);

        if (record.ContainsKey(filePath))
        {
            record[filePath].Add(assetName);
        }
        else
        {
            List<string> pathList = new List<string>();
            pathList.Add(assetName);
            record.Add(filePath, pathList);
        }
    }

    private static void CreatRecord(Dictionary<string, List<string>> record)
    {
        foreach (string key in record.Keys)
        {
            //key  == E:/ MyFrameWork / Assets / Res / Prefabs 1
            //string filePath = key.Substring(0, key.LastIndexOf('/'));

            //Debug.Log(filePath);
            string recordPath = key + "Record.txt";
            //FixedPath(ref recordPath);

            FileStream fs = new FileStream(recordPath, FileMode.OpenOrCreate);
            //Debug.Log(fs.Name);
            StreamWriter bw = new StreamWriter(fs);
            List<string> pathList = new List<string>();
            pathList = record[key];
            bw.WriteLine(pathList.Count);

            ////遍历 res 路径         
            foreach (string item in pathList)
            {   //itm  == Prefabs 1/one/Cube.prefab

                bw.WriteLine(item.Substring(item.LastIndexOf('/') + 1) + " " + item.Remove(item.LastIndexOf(".") + 1).ToLower() + variant);
            }

            bw.Close();
            fs.Close();
        }
    }

    /// <summary>
    /// 修正路径\\ to /
    /// </summary>
    /// <param name="path"></param>
    private static void FixedPath(ref string path)
    {
        path = path.Replace('\\', '/');
    }

    /// <summary>
    /// 修正路径2 / to \
    /// </summary>
    /// <param name="path"></param>
    private static void FixedPath2(ref string path)
    {
        path = path.Replace('/', '\\');
    }

    /// <summary>
    /// 清除所有AssetBundleName,由于打包方法会将所有设置过AssetBundleName的资源打包，所以自动打包前需要清理
    /// </summary>
    private static void ClearAssetBundlesName()
    {
        //获取所有的AssetBundle名称
        string[] abNames = AssetDatabase.GetAllAssetBundleNames();
        //强制删除所有的AssetBundle名称
        for (int i = 0; i < abNames.Length; i++)
        {
            AssetDatabase.RemoveAssetBundleName(abNames[i], true);
        }
    }

    private static void CreatOrConfirmPath(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

}

