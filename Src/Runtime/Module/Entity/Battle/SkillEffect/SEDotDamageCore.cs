/*
* @Author: xiang huan
* @Date: 2022-07-19 16:19:58
* @Description: 持续伤害效果
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Module/Entity/Battle/SkillEffect/SEDotDamageCore.cs
* 
*/

using GameMessageCore;
using UnityEngine;
public class SEDotDamageCore : SkillEffectBase
{

    /// <summary>
    /// 检测能否应用效果
    /// </summary>
    /// <param name="fromEntity">发送方</param>
    /// <param name="targetEntity">接受方</param>
    public override bool CheckApplyEffect(EntityBase fromEntity, EntityBase targetEntity)
    {
        //目标方已经死亡
        if (targetEntity.BattleDataCore != null)
        {
            if (!targetEntity.BattleDataCore.IsLive())
            {
                return false;
            }
        }
        return true;
    }

    public override void Start()
    {
        base.Start();
        if (EffectData == null)
        {
            return;
        }
    }
}