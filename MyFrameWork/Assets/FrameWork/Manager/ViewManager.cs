using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI管理类
/// </summary>
public class ViewManager : BaseManager<ViewManager> {

    /// <summary>
    /// view Object池
    /// </summary>
    Dictionary<string, GameObject> viewDic;

    /// <summary>
    /// 开启的view 脚本池
    /// </summary>
    List<BaseView> openViewList = new List<BaseView>();

    /// <summary>
    /// view 脚本池
    /// </summary>
    Dictionary<string, BaseView> viewList = new Dictionary<string, BaseView>();

    protected override void Init()
    {
       
    }

    /// <summary>
    /// 所有BaseView模块加载完成时 再初始化 
    /// </summary>
    public void InitViewObject()
    {
        viewDic = new Dictionary<string, GameObject>();
        foreach (var item in viewList)
        {
            viewDic.Add(item.Key, Resources.Load<GameObject>(item.Key));
        }
    }

/// <summary>
/// 打开一个界面
/// </summary>
/// <param name="name"></param>
public void Open(string viewName)
    {
        if (viewDic.ContainsKey(viewName))
        {
            //如果存在 打开
            if (openViewList.Exists(a => a == viewList[viewName]))
            {
                viewList[viewName].Open();
            }
            else
            {
                //不存在 加载
                GameObject go = GameObject.Instantiate(viewDic[viewName]);
                go.transform.SetParent(Game.Instance.UILayer, false);
                go.transform.localScale = new Vector3(1, 1, 1);
                go.transform.position = Game.Instance.UILayer.position;
                viewList[viewName].PrefabLoadCallBack(go);
                openViewList.Add(viewList[viewName]);
            }
        }
    }

    public void CloseAllView()
    {
        foreach (var item in openViewList)
        {
            item.Close();
        }
    }

    public bool CheckOpen(BaseView view)
    {
        return view.IsOpen();
    }

    public void RemoveOpen(BaseView view)
    {
        openViewList.Remove(view);
    }

    public void RegisterView(string viewName, BaseView baseView)
    {
        this.viewList.Add(viewName, baseView);
    }

    public void UnRegisterView(string viewName)
    {
        this.viewList.Remove(viewName);
        this.viewDic.Remove(viewName);
    }
}