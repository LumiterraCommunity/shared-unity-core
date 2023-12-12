/** 
 * @Author XQ
 * @Date 2022-08-09 17:39:06
 * @FilePath /Assets/Plugins/SharedCore/Src/Runtime/Util/NetUtilCore.cs
 */
using System;
using System.Collections.Generic;
using GameMessageCore;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 处理网络数据转换的工具类 共享库
/// </summary>
public static class NetUtilCore
{
    /// <summary>
    /// 实体ID转成协议需要string型 TODO:将来这个方法会废弃掉 方便一键替换
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string EntityIDToNet(long id)
    {
        return id.ToString();
    }

    /// <summary>
    /// 协议 location 转客户端坐标
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public static UnityEngine.Vector3 LocFromNet(GameMessageCore.EntityLocation location)
    {
        return new UnityEngine.Vector3(location.Loc.X, location.Loc.Y, location.Loc.Z);
    }

    public static UnityEngine.Vector3 LocFromNet(GameMessageCore.Vector3 location)
    {
        return new UnityEngine.Vector3(location.X, location.Y, location.Z);
    }

    /// <summary>
    /// 客户端坐标转协议location
    /// </summary>
    /// <param name="clientPos"></param>
    /// <returns></returns>
    public static GameMessageCore.EntityLocation LocToNet(UnityEngine.Vector3 clientPos, int mapId = 0)
    {
        return new GameMessageCore.EntityLocation()
        {
            MapId = mapId,
            Loc = Vector3ToNet(clientPos)
        };
    }

    /// <summary>
    /// 客户端坐标转成协议坐标
    /// </summary>
    /// <param name="clientVector3"></param>
    /// <returns></returns>
    public static GameMessageCore.Vector3 Vector3ToNet(UnityEngine.Vector3 clientVector3)
    {
        return new GameMessageCore.Vector3()
        {
            X = clientVector3.x,
            Y = clientVector3.y,
            Z = clientVector3.z
        };
    }



    /// <summary>
    /// 服务器坐标转客户端
    /// </summary>
    /// <param name="svrVector3"></param>
    /// <returns></returns>
    public static UnityEngine.Vector3 Vector3FromNet(GameMessageCore.Vector3 svrVector3)
    {
        return svrVector3 == null
        ? UnityEngine.Vector3.zero
        : new UnityEngine.Vector3(svrVector3.X, svrVector3.Y, svrVector3.Z);
    }

    /// <summary>
    /// 客户端坐标列表转成协议坐标列表
    /// </summary>
    /// <param name="clientVector3List"></param>
    /// <returns></returns>
    public static Google.Protobuf.Collections.RepeatedField<GameMessageCore.Vector3> Vector3ListToNet(UnityEngine.Vector3[] clientVector3List)
    {
        Google.Protobuf.Collections.RepeatedField<GameMessageCore.Vector3> netVector3List = new();
        if (clientVector3List != null && clientVector3List.Length > 0)
        {
            foreach (UnityEngine.Vector3 item in clientVector3List)
            {
                netVector3List.Add(Vector3ToNet(item));
            }
        }

        return netVector3List;
    }

    /// <summary>
    /// 协议坐标列表转客户端坐标列表
    /// </summary>
    /// <param name="netVector3List"></param>
    /// <returns></returns>
    public static List<UnityEngine.Vector3> Vector3ListFromNet(Google.Protobuf.Collections.RepeatedField<GameMessageCore.Vector3> netVector3List)
    {
        List<UnityEngine.Vector3> clientVector3List = new();
        if (netVector3List != null && netVector3List.Count > 0)
        {
            foreach (GameMessageCore.Vector3 item in netVector3List)
            {
                clientVector3List.Add(Vector3FromNet(item));
            }
        }
        return clientVector3List;
    }
    /// <summary>
    /// 协议方向向量转客户端 返回必不为空或者长度为0
    /// </summary>
    /// <param name="svrDir"></param>
    /// <returns></returns>
    public static UnityEngine.Vector3 DirFromNet(GameMessageCore.Vector3 svrDir)
    {
        if (svrDir == null)
        {
            return UnityEngine.Vector3.back;
        }

        if (svrDir.X.ApproximatelyEquals(0) && svrDir.Y.ApproximatelyEquals(0) && svrDir.Z.ApproximatelyEquals(0))
        {
            return UnityEngine.Vector3.back;
        }

        return Vector3FromNet(svrDir);
    }

    /// <summary>
    /// 客户端方向向量转协议
    /// </summary>
    /// <param name="clientDir"></param>
    /// <returns></returns>
    public static GameMessageCore.Vector3 DirToNet(UnityEngine.Vector3 clientDir)
    {
        return clientDir == null
            ? new GameMessageCore.Vector3()
            {
                X = 0,
                Y = 0,
                Z = -1
            }
            : Vector3ToNet(clientDir);
    }

    /// <summary>
    /// 客户端采集资源基础信息转协议
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    public static GameMessageCore.CollectResourceBaseInfo CollectResourceBaseToNet(ICollectResourceCore resource)
    {
        return new GameMessageCore.CollectResourceBaseInfo()
        {
            Id = resource.Id,
            Type = (GameMessageCore.CollectResourceType)resource.ResourceType
        };
    }

    /// <summary>
    /// 将实体和对应的多个具体效果转换为实体伤害服务器包
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="skillId"></param>
    /// <param name="effects"></param>
    /// <returns></returns>
    public static EntityDamage AssembleEntityDamageNetMsg(EntityBase entity, int skillId, List<DamageEffect> effects)
    {
        if (entity == null || effects == null || effects.Count == 0)
        {
            return null;
        }

        EntityDamage entityDamage = new()
        {
            SkillId = skillId,
            Entity = ToNetEntityId(entity.BaseData)
        };
        entityDamage.Effect.AddRange(effects);
        return entityDamage;
    }
    public static EntityId ToNetEntityId(EntityBaseData data)
    {
        EntityId netEntityId = new();
        netEntityId.Id = data.Id;
        netEntityId.Type = data.Type;
        return netEntityId;
    }

}