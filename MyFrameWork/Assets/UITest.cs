using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UITest : BaseBehaviour {
    public EventTable table;
    public NameTable nameTable;
    public GameObject cube;
    public VariableTable variableTable;
    // Use this for initialization
    IABLoader abLoader;

    protected override void Awake()
    {
        
    }

    protected override void Start () {
        UIVariable name = variableTable.FindVariable("ddddd");
        //print(name.name);
        name.SetVlaue("测试一");
        cube = nameTable.FindObj("cube");
        cube.SetActive(false);
        
        //table.ListenEvent("Click", Click);
        //table.ListenEvent("ClickThree", ClickThree);
        //       
        GlobalEventSystem.Bind(GlobalEventType.test, ClickThree);
       // GlobalEventSystem.Bind(GlobalEventType.test2, ClickTwo);

        //TimeControl timer = TimeControl.CreaterTimer();
        //timer.StartTiming(10, Complete, Func, 3, true, false, true);

        string path = IPathTool.GetAssetBundlePath();//.Substring(IPathTool.GetAssetBundlePath().IndexOf("Assets"));


        //路径/斜
        //AssetBundle ab = AssetBundle.LoadFromFile(Application.dataPath + @"/StreamingAssets/Windows/sceneone/one/cube.unity3d");
        //Debug.Log(Application.dataPath + @"/StreamingAssets/Windows/sceneone/one/cube.unity3d");
        //Debug.Log(IPathTool.GetAssetBundlePath());
        //IABResLoader abloader = new IABResLoader(ab);

        // GameObject ob = abloader["Cube"] as GameObject;
        //Instantiate(ob, transform);
        //abloader.Dispose();

        //abScene.LoadAsset("Cube.prefab", ClickTwo, Complete);

        //abLoader = new IABLoader(path);
        // print(path);

        //abLoader.OnLoadFinish(Click);
        //abLoader.OnLoad(ClickTwo);
        //StartCoroutine(abLoader.CommonLoad());


        //print(IPathTool.GetWWWAssetBundlePath() +"/" + "sceneone/one/cube.unity3d");
        
        //ab = new IABLoader(bundleName, Complete1, null);
        //StartCoroutine(ab.CommonLoad());

        // relation = new IABRelationManager(bundleName, Complete);
        //StartCoroutine(relation.LoadAssetBundle());

        //manifest = new IABManifestLoader(Complect3);
        //StartCoroutine(manifest.LoadManifet());


        //string sceneName = "SceneOne";
        //IABScenceManager abScene = new IABScenceManager(sceneName);

        //abScene.ReadConfiger(sceneName);

       
        
        IABManifestLoader manifest = new IABManifestLoader(ManifestLoadComplete);
        StartCoroutine(manifest.LoadManifet());
        
        


    }
    string bundleName = "sceneone/one/cube.unity3d";
    string sceneName = "sceneone";
    private void ManifestLoadComplete(AssetBundleManifest assetManifest)
    {
        manager = new IABScenceManager(sceneName);
        manager.Init(bundleName, Progress, Complete);
        StartCoroutine(manager.LoadAssetSys(bundleName));
    }

    private void Progress(string bundleName, float progress)
    {
        print(progress);
    }

    IABScenceManager manager;
    private void Complete(string scenceName, string bundleName)
    {
        print(manager.GetSingleResources(bundleName, "Cube") as GameObject);
    }

    IABManifestLoader manifest;
    private void Complect3(AssetBundleManifest assetManifest)
    {
        foreach (var item in assetManifest.GetAllAssetBundles())
        {
            print(item);
        }
    }

    IABRelationManager relation;
    private void Complete2(string bundleName)
    {
        GameObject go = relation.GetMutiResources("Cube")[0] as GameObject;
        Instantiate(go, transform);
        //print();
    }

    IABLoader ab;
    private void Complete1(string bundleName)
    {
 
    }

    private void onLoad(string bundleName, float progress)
    {
        print(bundleName + progress);
    }

    private void Complete4(string scenceName, string bundleName)
    {
        print(scenceName);
    }

    private void ClickTwo(string bundleName, float progress)
    {
        print(bundleName);
        print(progress);
    }

    // Update is called once per frame

    private void Func(float t)
    {

    }

    void Click(string bundleName)
    {
        GameObject go = abLoader.GetResources("cube") as GameObject;
       // Instantiate(go, cube);
    }

    void Complete()
    {

    }

    void Func(string msg)
    {
        if (msg.Equals("1"))
        {
            print("Equals");
        }
    }

    void ClickThree()
    {
        print("ClickThree");
    }
}

