using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityGameFramework.Runtime;
using static HomeDefine;

/// <summary>
/// 家园动物core 需要依赖AnimalDataCore和PetDataCore 装配时需要先添加依赖项
/// </summary>
public abstract class HomeAnimalCore : EntityBaseComponent, ICollectResourceCore
{
    public ulong Id => (ulong)RefEntity.BaseData.Id;

    public eResourceType ResourceType => eResourceType.Animal;

    public GameObject LogicRoot => RefEntity.EntityRoot;

    public Vector3 Position => RefEntity.Position;

    public int GetActionLevel(eAction action)
    {
        if ((action & COLLECT_RESOURCE_ACTION_MASK) != 0)
        {
            return RefEntity.EntityAttributeData.GetBaseValue(eAttributeType.CollectionLv);
        }
        else
        {
            return RefEntity.EntityAttributeData.GetBaseValue(eAttributeType.FarmingLv);
        }
        //作为家园动作等级是没有战斗等级的
    }

    public eAction SupportAction { get; set; } = eAction.Appease;

    public eAction HarvestAction { get; private set; } = eAction.None;//收获动作

    /// <summary>
    /// 动物数据
    /// </summary>
    /// <value></value>
    public AnimalDataCore Data { get; private set; }
    /// <summary>
    /// 宠物数据 方便获取及节省性能 其实和实体上挂载的一致
    /// </summary>
    /// <value></value>
    public PetDataCore PetData { get; private set; }

    /// <summary>
    /// 自动收获的掉落实体
    /// </summary>
    protected GameObject DropEntity { get; private set; }
    private bool _isListenColliderLoadFinish;

    //缓存一些读表值 节省tick中反复读取
    private int _animalDeadTimeFromHunger;
    private float _petProduceHungerRate = 1;

    protected virtual void Awake()
    {
        Data = gameObject.GetComponent<AnimalDataCore>();
        PetData = gameObject.GetComponent<PetDataCore>();

        _animalDeadTimeFromHunger = TableUtil.GetGameValue(eGameValueID.animalDeadTimeFromHunger).Value;
        _petProduceHungerRate = TableUtil.GetGameValue(eGameValueID.PetProduceHungerRate).Value * TableDefine.THOUSANDTH_2_FLOAT;

        if (Data == null || PetData == null)
        {
            Log.Error($"家园动物 Data 组件 is null，也可能是添加顺序反了，这个强依赖顺序");
        }
    }

    protected virtual void Start()
    {
        RefEntity.EntityEvent.InputSkillRelease += OnInputSkillRelease;

        if (PetData.PetCfg != null)
        {
            HarvestAction = TableUtil.ConvertToBitEnum<eAction>(PetData.PetCfg.HarvestAction);
            SupportAction |= HarvestAction;//收获动作添加到支持列表

            if (PetData.PetCfg.AutoHarvest)
            {
                //https://codingmonkey.feishu.cn/docx/BgbRdOKxPo25mEx8apNcH65enlf
                Log.Error($"动物目前不能再配置自动收获 cid:{PetData.PetCfgId}");
            }
        }
        else
        {
            Log.Error($"动物配置表为空 cid:{PetData.PetCfgId}");
        }

        if (gameObject.TryGetComponent(out EntityCollisionCore entityCollisionCore))
        {
            if (entityCollisionCore.CollisionObject != null)
            {
                OnColliderLoadFinish(entityCollisionCore.CollisionObject);
            }
            else
            {
                RefEntity.EntityEvent.ColliderLoadFinish += OnColliderLoadFinish;
                _isListenColliderLoadFinish = true;
            }
        }

        InitStatus();
    }

    protected virtual void OnDestroy()
    {
        RefEntity.EntityEvent.InputSkillRelease -= OnInputSkillRelease;

        if (DropEntity != null)
        {
            Destroy(DropEntity);
            DropEntity = null;
        }

        if (_isListenColliderLoadFinish)
        {
            RefEntity.EntityEvent.ColliderLoadFinish -= OnColliderLoadFinish;
            _isListenColliderLoadFinish = false;
        }
    }

    private void OnInputSkillRelease(InputSkillReleaseData data)
    {
        int needCostHunger = TableUtil.GetGameValue(eGameValueID.PetCastSkillHungerCost).Value;
        PetData.SetHungerValue(PetData.HungerValue - needCostHunger, false);
    }

    private void InitStatus()
    {
        if (Data.SaveData.IsDead)
        {
            EnterAnimalDeadStatus(true);
        }
        else
        {
            if (Data.IsCanHarvest && !PetData.PetCfg.AutoHarvest)
            {
                OnEnterHarvestStatus(true);
            }
            else
            {
                gameObject.GetComponent<HomeActionProgressData>().StartProgressAction(eAction.Appease, TableUtil.GetGameValue(eGameValueID.animalAppeaseMaxActionValue).Value);
            }
        }
    }

    protected virtual void Update()
    {
        TickHunger();

        TickHarvest();
    }

    private void TickHarvest()
    {
        //饥饿
        if (PetData.IsHunger)
        {
            return;
        }

        //幸福值太低
        if (Data.IsHappyValid == false)
        {
            return;
        }

        bool oldCanHarvest = Data.IsCanHarvest;
        float curTime = Data.SaveData.HarvestProgress / ANIMAL_HARVEST_PROCESS_MAX_UNIT * Data.HarvestMaxTime;
        float targetTime = curTime + Time.deltaTime;
        float targetProgress = targetTime / Data.HarvestMaxTime * ANIMAL_HARVEST_PROCESS_MAX_UNIT;
        Data.SaveData.SetHarvestProgress(targetProgress);

        if (Data.IsCanHarvest != oldCanHarvest && Data.IsCanHarvest)
        {
            if (!PetData.PetCfg.AutoHarvest)
            {
                OnEnterHarvestStatus(false);
            }
        }
    }

    /// <summary>
    /// 饥饿值变化
    /// </summary>
    private void TickHunger()
    {
        AnimalSaveData saveData = Data.SaveData;

        //饥饿度减少
        float hungerSpeed = PetData.PetCfg.HungerSpeed;
        if (saveData.IsProduceStage)
        {
            hungerSpeed *= _petProduceHungerRate;
        }
        float newHunger = PetData.HungerValue - (hungerSpeed * Time.deltaTime);
        PetData.SetHungerValue(newHunger, false);

        //检查饿死
        if (PetData.IsHunger && saveData.LastCompleteHungerStamp > 0)
        {
            if ((TimeUtil.GetServerTimeStamp() - saveData.LastCompleteHungerStamp) * TimeUtil.MS2S >= _animalDeadTimeFromHunger)
            {
                saveData.LastCompleteHungerStamp = 0;
                OnTimerHungerDead();
            }
        }
    }

    /// <summary>
    /// 检测到饥饿到了饿死的时候 客户端不处理饿死状态 由服务器主动告知
    /// </summary>
    protected virtual void OnTimerHungerDead()
    {
    }

    /// <summary>
    /// 吃下食物后设置饥饿值
    /// </summary>
    public virtual void EatenSetHunger(float progress)
    {
        PetData.SetHungerValue(progress, false);
    }

    public bool CheckSupportAction(eAction action)
    {

        if ((SupportAction & action) == 0)
        {
            return false;
        }

        if (Data.SaveData.IsDead)//死了
        {
            return false;
        }

        if (PetData.PetCfg.AutoHarvest)
        {
            return action == eAction.Appease;//自动收获的只支持安抚 不允许手动收获
        }
        else//需要手动收获的
        {
            if (action == eAction.Appease)
            {
                return !Data.IsCanHarvest;//不在收获状态下才可以安抚
            }

            //收获动作
            return Data.IsCanHarvest;
        }
    }

    public bool CheckPlayerAction(long playerId, eAction action)
    {
        return CheckSupportAction(action);
    }

    public void ExecuteAction(eAction action, int toolCid, long playerId, long entityId, int skillId, object actionData)
    {
        if (action == eAction.Appease)//安抚
        {
            try
            {
                GameMessageCore.AppeaseResult appeaseResult = (GameMessageCore.AppeaseResult)actionData;
                OnExecuteAppease(Data.SaveData.IsComforted == false, appeaseResult.AnimalHappyValue, appeaseResult.Proficiency, playerId);
            }
            catch (System.Exception e)
            {
                Log.Error($"安抚动物异常 actionData:{JsonConvert.SerializeObject(actionData)} error:{e}");
                throw e;
            }
        }
        else if (action == HarvestAction)//收获 能执行的都是手动收货的
        {
            OnExecuteHarvest(action);
        }
    }

    public virtual void ExecuteProgress(eAction targetCurAction, long triggerEntityId, int skillId, int deltaProgress, bool isCrit, bool isPreEffect)
    {

    }

    /// <summary>
    /// 进入收获状态 只有手动收获才会触发
    /// </summary>
    protected virtual void OnEnterHarvestStatus(bool isInit)
    {
        gameObject.GetComponent<HomeActionProgressData>().StartProgressAction(HarvestAction, TableUtil.GetGameValue(eGameValueID.animalHarvestMaxActionValue).Value);
    }

    /// <summary>
    /// 进入动物死亡状态
    /// </summary>
    public virtual void EnterAnimalDeadStatus(bool isInit)
    {
        if (!isInit)
        {
            gameObject.GetComponent<HomeActionProgressData>().EndProgressAction();
            Data.SaveData.IsDead = true;
        }

        RefEntity.BattleDataCore.SetHP(0);

        ClearDropProduct();//TODO: home 暂时这样
    }

    /// <summary>
    /// 抚摸操作
    /// </summary>
    /// <param name="appeaseValid">是否有效抚摸 无效代表一个成熟阶段重复抚摸</param>
    /// <param name="usedHappy">抚摸使用的的幸福值</param>
    /// <param name="proficiency">抚摸获得的熟练度</param>
    /// <param name="playerId">抚摸的玩家id</param>
    protected virtual void OnExecuteAppease(bool appeaseValid, int usedHappy, int proficiency, long playerId)
    {
        if (appeaseValid)
        {
            Data.SaveData.IsComforted = true;
            PetData.Favorability += TableUtil.GetGameValue(eGameValueID.animalAddFavorabilityEveryAppease).Value;
            Data.MsgFavorabilityChanged?.Invoke(PetData.Favorability);
            Data.SetProficiency(proficiency);
        }
        gameObject.GetComponent<HomeActionProgressData>().StartProgressAction(eAction.Appease, TableUtil.GetGameValue(eGameValueID.animalAppeaseMaxActionValue).Value);

        Data.SetHappiness(usedHappy);
    }

    /// <summary>
    /// 主动操作的收获
    /// <param name="action">单一具体动作</param>
    /// </summary>
    protected virtual void OnExecuteHarvest(eAction action)
    {
        if (PetData.PetCfg.AutoHarvest)
        {
            throw new Exception($"动物配置表错误 自动收获的动物不能手动收获 cid:{PetData.PetCfgId}");
        }

        Data.ClearDataAfterHarvest();
        gameObject.GetComponent<HomeActionProgressData>().StartProgressAction(eAction.Appease, TableUtil.GetGameValue(eGameValueID.animalAppeaseMaxActionValue).Value);
    }

    /// <summary>
    /// 创建一个掉落的产品
    /// </summary>
    /// <param name="productSaveData"></param>
    /// <typeparam name="TDrop"></typeparam>
    protected void CreateDropProduct<TDrop>(AnimalProductSaveData productSaveData, Transform parent, bool isInit) where TDrop : AnimalDropCore
    {
        if (productSaveData == null)
        {
            Log.Error($"动物收获的产品数据为空 cid:{PetData.PetCfgId}");
            return;
        }

        if (Data.SaveData.ProductSaveData != null && !isInit)
        {
            Log.Error($"动物收获的产品数据不为空 cid:{PetData.PetCfgId}");
            return;
        }

        if (DropEntity != null)
        {
            Log.Error($"动物掉落实体不为空 cid:{PetData.PetCfgId}");
        }

        if (productSaveData.ProductId == 0)
        {
            Log.Error($"动物掉落产品数据错误 id==0  product :{productSaveData}");
            return;
        }

        Data.SaveData.ProductSaveData = productSaveData;
        DropEntity = GameObjectUtil.CreateGameObject($"{RefEntity.BaseData.Id}_{productSaveData.ProductId}", parent);
        DropEntity.transform.position = NetUtilCore.Vector3FromNet(productSaveData.Pos);
        AnimalDropCore drop = DropEntity.AddComponent<TDrop>();
        drop.InitAnimalDrop(productSaveData, RefEntity.BaseData.Id);
    }

    /// <summary>
    /// 清除掉落的产品
    /// </summary>
    public void ClearDropProduct()
    {
        if (DropEntity != null)
        {
            Destroy(DropEntity);
            DropEntity = null;
        }
        Data.SaveData.ProductSaveData = null;
    }

    private void OnColliderLoadFinish(GameObject colliderGo)
    {
        colliderGo.layer = MLayerMask.HOME_RESOURCE;//将怪物碰撞盒改成家园触发层
        colliderGo.tag = MTag.HOME_ANIMAL;
    }

    /// <summary>
    /// 生成一个用于网络传输的宠物基础数据
    /// </summary>
    /// <returns></returns>
    public GameMessageCore.ProxyAnimalBaseData ToProxyPetData()
    {
        GameMessageCore.ProxyAnimalBaseData proxyData = new()
        {
            AnimalId = RefEntity.BaseData.Id,
            Name = RefEntity.RoleBaseDataCore.Name,
        };
        PetData.ToProxyPetData(proxyData);

        return proxyData;
    }

    /// <summary>
    /// 从网络传输的宠物基础数据来初始化
    /// </summary>
    /// <param name="data"></param>
    public void InitFromNetData(GameMessageCore.ProxyAnimalBaseData data)
    {
        RefEntity.RoleBaseDataCore.SetName(data.Name);
        PetData.InitFromProxyPetData(data);
    }

    /// <summary>
    /// 获取下一次收获的时间 秒 无效时返回-1
    /// </summary>
    /// <returns></returns>
    public float GetNextHarvestTime()
    {
        if (PetData.IsHunger)
        {
            return -1f;
        }

        if (!Data.IsHappyValid)
        {
            return -1f;
        }

        float remainProgress = 1 - (Data.SaveData.HarvestProgress / ANIMAL_HARVEST_PROCESS_MAX_UNIT);
        return Mathf.Max(Data.HarvestMaxTime * remainProgress, 0);
    }
}