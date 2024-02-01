/*
 * @Author: xiang huan
 * @Date: 2023-02-13 19:12:29
 * @Description: 
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/SceneElement/Data/SafeAreaElementNetData.cs
 * 
 */

using System.Numerics;
using Newtonsoft.Json;

public class SafeAreaElementNetData
{

    public long StartTime;
    public Vector3 Position;
    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }
}