/*
 * @Author: xiang huan
 * @Date: 2023-02-13 19:12:29
 * @Description: 
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/SceneElement/Data/SafeAreaElementNetData.cs
 * 
 */

using Newtonsoft.Json;
using UnityEngine;

public class SafeAreaElementNetData
{
    public Vector3 Position;
    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }
}