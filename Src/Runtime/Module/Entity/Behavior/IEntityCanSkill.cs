/* 
 * @Author XQ
 * @Date 2022-08-05 12:54:15
 * @FilePath /Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Behavior/IEntityCanSkill.cs
 */
/// <summary>
/// 判定实体能放技能接口
/// </summary>
public interface IEntityCanSkill
{
    /// <summary>
    /// 能放技能判定逻辑
    /// </summary>
    /// <param name="skillID">具体技能ID 为0时代表任意技能</param>
    /// <returns></returns>
    bool CheckCanSkill(int skillID);
}