/*
 * @Author: xiang huan
 * @Date: 2023-10-24 15:14:29
 * @Description: 结算传送门组件
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Runtime/Module/SceneElement/Element/SettlePortalElementCore.cs
 * 
 */

public class SettlePortalElementCore : PortalElementCore
{
    public override eSceneElementType ElementType => eSceneElementType.SettlePortal;
    public override string GetPortalTips()
    {
        return $"{string.Format("{0:n1}", RewardRate * 100)}%";
    }
}
