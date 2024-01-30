/*
 * @Author: xiang huan
 * @Date: 2023-02-13 19:12:29
 * @Description: 
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/SceneElement/Data/PortalElementNetData.cs
 * 
 */

using Newtonsoft.Json;
public class PortalElementNetData
{
    public long StartTime;
    public ePortalStatusType StatusType;
    public int CurUseNum;
    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }
}