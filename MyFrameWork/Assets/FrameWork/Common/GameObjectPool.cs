using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 对象池
/// </summary>

public class GameObjectPool : BaseManager<GameObjectPool>
{ 
    //对象池
    private Dictionary<string, List<GameObject>> cache;
    //初始化：在对象创建时执行一次
    protected override void Init()
    {
        base.Init();
        cache = new Dictionary<string, List<GameObject>>();
    }

    //创建对象
    /// <summary>
    /// 通过对象池创建对象
    /// </summary>
    /// <param name="key">需要创建的对象种类</param>
    /// <param name="prefab">需要创建的预制件</param>
    /// <param name="pos">创建的位置</param>
    /// <param name="dir">创建的旋转</param>
    /// <returns></returns>
    public GameObject CreateObject(string key, GameObject prefab, Vector3 pos, Quaternion dir)
    {   
        //在池中查找 
        GameObject tempGo = FindUsableObject(key);
        //如果没有找到
        if (tempGo == null)
        {
            //创建物体 
            tempGo = Instantiate(prefab);
            //加入池中
            Add(key, tempGo);
            //将通过对象池创建的物体，存入对象池子物体列表中。
            tempGo.transform.SetParent(transform);
        }
        //使用
        UseObject(tempGo, pos, dir);
        return tempGo;
    }

    private void UseObject(GameObject go,Vector3 pos, Quaternion dir)
    {  
        //先设置位置
        go.transform.position = pos;
        go.transform.rotation = dir;
        //再启用物体
        go.SetActive(true);  
        //重置通过对象池创建物体的所有脚本对象
        foreach (var item in go.GetComponentsInChildren<IResetable>())
        {
            item.OnReset();
        }
    }

    private void Add(string key, GameObject tempGo)
    {
        //如果池中没有键  则 添加键
        if (!cache.ContainsKey(key)) cache.Add(key, new List<GameObject>());
        //将物体加入池中
        cache[key].Add(tempGo);
    }

    private GameObject FindUsableObject(string key)
    {
        if (cache.ContainsKey(key))
        {
            //  public delegate bool Predicate<T>(T obj);
            //查找池中禁用的物体
            return cache[key].Find(o => !o.activeInHierarchy);
        }
        return null;
    }

    //即时回收
    public void CollectObject(GameObject go)
    {
        go.SetActive(false);
    }

    //延迟回收
    public void CollectObject(GameObject go, float delay)
    {
        StartCoroutine(DelayCollect(go, delay));
    }

    private IEnumerator DelayCollect(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        CollectObject(go);
    }

    //清空
    public void ClearAll()
    { 
        //将字典中所有键存入集合
        List<string> listKey =new List<string>(cache.Keys);
        foreach (var item in listKey)
        {
            //遍历集合元素 删除字典记录
            Clear(item);
        }
    }

    //根据键清除
    public void Clear(string key)
    { 
        //倒序删除
        for (int i = cache[key].Count -1; i>=0 ; i--)
        {
            Destroy(cache[key][i]);
        }
        //在字典集合中清空当前记录（集合列表）
        cache.Remove(key);
    }
}
