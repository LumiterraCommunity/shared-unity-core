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

    public int Lv => RefEntity.BattleDataCore.Level;

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

    private int _animalDeadTimeFromHunger;
    private bool _isListenColliderLoadFinish;

    protected virtual void Awake()
    {
        Data = gameObject.GetComponent<AnimalDataCore>();
        PetData = gameObject.GetComponent<PetDataCore>();
        _animalDeadTimeFromHunger = TableUtil.GetGameValue(eGameValueID.animalDeadTimeFromHunger).Value;

        if (Data == null || PetData == null)
        {
            Log.Error($"家园动物 Data 组件 is null，也可能是添加顺序反了，这个强依赖顺序");
        }
    }

    protected virtual void Start()
    {
        if (PetData.PetCfg != null)
        {
            HarvestAction = TableUtil.ToHomeAction(PetData.PetCfg.HarvestAction);
            SupportAction |= HarvestAction;//收获动作添加到支持列表

            if (PetData.PetCfg.AutoHarvest)
            {
                //https://codingmonkey.feishu.cn/docx/BgbRdOKxPo25mEx8apNcH65enlf
                Log.Error($"动物目前不能再配置自动收获 cid:{PetData.PetCfgId}");
            }

            //TODO pet 表
            // PetFeature = TableUtil.ToPetFeature(Data.DRMonster.PetFeature);
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
        if (Data.IsHunger)
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
        if (saveData.HungerProgress > 0)
        {
            saveData.HungerProgress -= PetData.PetCfg.HungerSpeed * Time.deltaTime;

            if (saveData.HungerProgress <= 0)//开始完全饥饿
            {
                saveData.HungerProgress = 0;
                saveData.LastCompleteHungerStamp = TimeUtil.GetServerTimeStamp();
            }
            else
            {
                saveData.LastCompleteHungerStamp = 0;
            }
        }
        else
        {
            if (saveData.LastCompleteHungerStamp > 0)
            {
                if ((TimeUtil.GetServerTimeStamp() - saveData.LastCompleteHungerStamp) * TimeUtil.MS2S >= _animalDeadTimeFromHunger)
                {
                    saveData.LastCompleteHungerStamp = 0;
                    OnTimerHungerDead();
                }
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
        Data.SaveData.HungerProgress = progress;
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

    public void ExecuteAction(eAction action, int toolCid, int skillId, object actionData, long playerId)
    {
        if (action == eAction.Appease)//安抚
        {
            try
            {
                int usedHappy = (int)actionData;
                OnExecuteAppease(Data.SaveData.IsComforted == false, usedHappy, playerId);
            }
            catch (System.Exception e)
            {
                Log.Error($"安抚动物异常 actionData:{JsonConvert.SerializeObject(actionData)} error:{e}");
            }
        }
        else if (action == HarvestAction)//收获 能执行的都是手动收货的
        {
            OnExecuteHarvest(action);
        }
    }

    public virtual void ExecuteProgress(eAction targetCurAction, int skillId, int deltaProgress, bool isCrit, bool isPreEffect)
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
    /// <param name="playerId">抚摸的玩家id</param>
    protected virtual void OnExecuteAppease(bool appeaseValid, int usedHappy, long playerId)
    {
        if (appeaseValid)
        {
            Data.SaveData.IsComforted = true;
            PetData.Favorability += TableUtil.GetGameValue(eGameValueID.animalAddFavorabilityEveryAppease).Value;
            Data.MsgFavorabilityChanged?.Invoke(PetData.Favorability);
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
            Log.Error($"动物配置表错误 自动收获的动物不能手动收获 cid:{PetData.PetCfgId}");
            return;
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
}