/*
 * @Author: xiang huan
 * @Date: 2023-02-13 19:12:29
 * @Description: 
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SkillEffect/Data/SkillEffectSaveDataConfig.cs
 * 
 */

using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class SkillEffectSaveDataConfig
{
    public List<SkillEffectSaveData> RunTimeList;
    public List<SkillEffectSaveData> StaticList;
    public List<SkillEffectSaveData> StaticUpdateList;
    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }
}