﻿/*
 * @Author: xiang huan
 * @Date: 2023-10-24 15:14:29
 * @Description: 场景元素组件
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/SceneElement/Element/SceneElementCore.cs
 * 
 */
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SceneElementCore : SharedCoreComponent
{

    public static Action<long, SceneElementCore> OnSceneElementInitHook;
    public static Action<long, SceneElementCore> OnSceneElementDestroyHook;
    public virtual eSceneElementType ElementType => eSceneElementType.None;
    [SerializeField]
    [Header("全局ID 自动生成 不要乱改")]
    private long _id;
    public long Id => _id;
    protected bool IsSyncData = false; //是否需要同步数据
    protected virtual void Awake()
    {

#if UNITY_EDITOR
        //为了自动序列化ID和服务器场景同步使用
        if (!Application.isPlaying)
        {
            if (_id == default//直接在场景中添加组件
            || (PrefabUtility.GetCorrespondingObjectFromSource(this) != null && gameObject.scene != null && gameObject.scene.isLoaded))//发生在预制件拖动到场景中，场景加载时的awake不会走这里
            {
                AutoSetID();
            }
        }
#endif
        OnSceneElementInitHook?.Invoke(_id, this);
    }

    protected virtual void OnDestroy()
    {
        OnSceneElementDestroyHook?.Invoke(_id, this);
    }

    protected virtual void Update()
    {

#if UNITY_EDITOR
        UpdateID();
#endif
    }

#if UNITY_EDITOR
    private void UpdateID()
    {
        if (Application.isPlaying)
        {
            return;
        }

        CheckIDRevertFromPrefab();
    }

    /// <summary>
    // 检查是否从预制件中恢复ID成了预制件的ID  需要再改掉
    /// </summary>
    private void CheckIDRevertFromPrefab()
    {
        SceneElementCore prefabComponent = PrefabUtility.GetCorrespondingObjectFromSource(this);
        if (prefabComponent == null)
        {
            return;
        }

        if (_id == prefabComponent._id && _id != GetInstanceID())
        {
            AutoSetID();
        }
    }

    private void AutoSetID()
    {
        _id = GetInstanceID();//给定一个全局ID
        EditorUtility.SetDirty(this);
    }
#endif

    public virtual void InitNetData(string netData)
    {

    }
    public virtual string GetNetData()
    {
        return "";
    }
}
