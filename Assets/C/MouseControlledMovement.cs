using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public enum HeroActionType //英雄动作列表
{
    Default = 0,
    移动 = 1,
    death = 10,
    攻击一 = 2,
    攻击二 = 3,
    攻击三 = 4,
    一技能 = 5,
    二技能 = 6,
    三技能 = 7,
    四技能 = 8,
    五技能 = 9,
}

public class MouseControlledMovement : MonoBehaviour
{
    public float detectionDistance;//检测距离
    //public float speed; //移动速度

    private GameObject target = null;//攻击目标

    private NavMeshAgent agent;//导航代理
    private GameObject heroGameObject;//英雄物体
    public GameObject GetHeroGameObject
    {
        get
        {
            return heroGameObject;
        }
    }

    private Ray ray;//用射线获取鼠标指向位置
    private RaycastHit hit;

    private bool inOnMobile = false;//是否是移动中
    private Vector3 targetVector3; //目标位置
    private Animator animator;//动画控制器
    private HeroAttribute heroAttribute;//英雄属性
    public HeroAttribute GetHeroAttribute
    {
        get
        {
            return heroAttribute;
        }
    }
    private Transform MainCameraTransform;//玩家摄像机位置
    private Transform MapCameraTransform;//地图摄像机位置

    private float attackInterval;//攻击间隔
    private float attackTiem = 0f;

    private Text nameText;//角色名字
    private void Start()
    {
        heroGameObject = transform.Find("Hero").gameObject;//

        agent = heroGameObject.GetComponent<NavMeshAgent>();//获取导航网格代理组件
        MainCameraTransform = transform.Find("MainCamera").transform;
        MapCameraTransform = transform.Find("MapCamera").transform;

        nameText = transform.Find("MainCamera/Canvas/NameText").GetComponent<Text>();
        nameText.text = ServerStoreRead.GetRoleName();

        ReadSynchronousHeroAttribute();

        initialize.Invoke();//运行初始化事件
    }

    private void ReadSynchronousHeroAttribute()//读取同步英雄属性
    {
        //HeroAttributeList herListL = new HeroAttributeList(ServerStoreRead.GetCurrentRoleAttribute());//属性读取
        string[] herListL = ServerStoreRead.GetCurrentRoleAttributeString().Split(',');//属性string集获取
        string[] heroTypeList = Resources.Load<TextAsset>("hero").ToString().Split('\n');//英雄类型列表获取
        string str = heroTypeList[int.Parse(herListL[21])];//读取英雄类型
        string[] kv = str.Split('|');//获取英雄模型

        heroGameObject = Instantiate(Resources.Load<GameObject>(kv[0]), heroGameObject.transform);//英雄获取
        heroGameObject.name = "hero";
        heroAttribute = heroGameObject.GetComponent<HeroAttribute>();//获取英雄属性组件
        //heroAttribute.AttributeList = ServerStoreRead.GetCurrentRoleAttribute();//读取的属性列表赋予

        //最大生命值|最大魔法值|攻击力|攻击距离|攻击速度|移动速度|护甲|虚拟伤害|穿透力|重生数|暴击率|暴击效果|幸运值|力量|敏捷|智力|职业|觉醒|等级|经验值|伤害类型
        heroAttribute.AttributeList.hpMaxInt = int.Parse(herListL[0]);//生命值
        heroAttribute.AttributeList.mpMaxInt = int.Parse(herListL[1]);//魔法值
        heroAttribute.AttributeList.damageValueInt = int.Parse(herListL[2]);//攻击力
        heroAttribute.AttributeList.damageRangeFloat = float.Parse(herListL[3]);//攻击距离
        heroAttribute.AttributeList.damageSpeedFloat = float.Parse(herListL[4]);//攻击速度
        heroAttribute.AttributeList.MovementSpeedFloat = float.Parse(herListL[5]);//移动速度
        heroAttribute.AttributeList.armorInt = int.Parse(herListL[6]);//护甲
        heroAttribute.AttributeList.virtualDamageInt = int.Parse(herListL[7]);//虚拟伤害
        heroAttribute.AttributeList.penetrationInt = int.Parse(herListL[8]);//穿透力
        heroAttribute.AttributeList.rebirthValueInt = int.Parse(herListL[9]);//重生数
        heroAttribute.AttributeList.critRateInt = int.Parse(herListL[10]);//暴击率
        heroAttribute.AttributeList.critEffectInt = int.Parse(herListL[11]);//暴击效果
        heroAttribute.AttributeList.luckyValueInt = int.Parse(herListL[12]);//幸运值
        heroAttribute.AttributeList.powerInt = int.Parse(herListL[13]);//力量
        heroAttribute.AttributeList.agileInt = int.Parse(herListL[14]);//敏捷
        heroAttribute.AttributeList.intelligenceInt = int.Parse(herListL[15]);//智力
        heroAttribute.AttributeList.professionalString = (HeroProfessionalType)System.Enum.Parse(typeof(HeroProfessionalType), herListL[16]); //(HeroProfessionalType)int.Parse(herListL[16]);//职业
        heroAttribute.AttributeList.awakeningString = herListL[17];//觉醒
        heroAttribute.AttributeList.levelInt = int.Parse(herListL[18]);//等级
        heroAttribute.AttributeList.expInt = int.Parse(herListL[19]);//经验值
        heroAttribute.AttributeList.damageClassType = (DamageClassType)System.Enum.Parse(typeof(DamageClassType), herListL[20]); //(DamageClassType)int.Parse(herListL[20]);//伤害类型
        heroAttribute.AttributeList.typeID = int.Parse(herListL[21]);

        //heroAttribute = heroGameObject.transform.Find("hero").GetComponent<HeroAttribute>();//获取HeroAttribute
        heroAttribute.PlayerStart(this);//属性同步
        heroAttribute.deathEvent.AddListener(OnDeath);//增加死亡事件
    }
    private LayerMask mask = ~(1 << 9);

    public void MoveToTarget(Vector3 temporaryVector3)//移动到目的位置
    {
        targetVector3 = temporaryVector3;
        agent.SetDestination(temporaryVector3);//寻路目标设置为射线检测到那一点
        inOnMobile = true;
        animator.SetInteger("animation", (int)HeroActionType.移动);
        //temporaryVector3 = transform.InverseTransformPoint(temporaryVector3);//坐标转换
        //characterController.Move(vector3 * speed * Time.deltaTime); //移动角色
    }

    public void StopImmediately()//立刻暂停
    {
        MoveToTarget(heroGameObject.transform.position);
        target = null;
        inOnMobile = false;
        animator.SetInteger("animation", (int)HeroActionType.Default);
        //aIPicket.transform.position = heroGameObject.transform.position;
    }

    public void SetAttackTarget(GameObject other)//设置攻击对象，命令AI去攻击该目标
    {
        target = other;//设置攻击目标
        //attackTiem = 0f;//残余属性重置
        //playAttackAnimation = false;
    }

    [SerializeField] private EventSystem eventSystem;//禁止鼠标穿透UGUI
    [SerializeField] private GraphicRaycaster RaycastInCanvas;//Canvas上有这个组件
    private bool CheckGuiRaycastObjects()//测试UI射线
    {
        if (eventSystem == null || RaycastInCanvas == null) return false;
        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;
        List<RaycastResult> list = new List<RaycastResult>();
        RaycastInCanvas.Raycast(eventData, list);
        //Debug.Log(list.Count);
        return list.Count > 0;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))//获取鼠标点击
        {
            if (CheckGuiRaycastObjects()) return;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, detectionDistance, mask))//从屏幕上鼠标一点发射射线
            {
                target = null;
                MoveToTarget(hit.point);
                Texture2D cursorb = Resources.Load<Texture2D>("icon/mouse/mouse01");
                Cursor.SetCursor(cursorb, Vector2.zero, CursorMode.Auto);
            }
            //Vector3 cubeScreenPos = Camera.main.WorldToScreenPoint(heroGameObject.transform.position);//得到物体的屏幕坐标
            //Vector3 curMousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cubeScreenPos.z);//目前的鼠标二维坐标转为三维坐标
            //curMousePos = Camera.main.ScreenToWorldPoint(curMousePos);//目前的鼠标三维坐标转为世界坐标
            ////heroGameObject.transform.position = curMousePos;//把物体运动到鼠标点击的位置
            //MoveToTarget(curMousePos);
        }

        if (InOnMobile() && inOnMobile)//判断导航结束 agent.remainingDistance
        {
            if (target == null)
            {
                animator.SetInteger("animation", (int)HeroActionType.Default);
            }
            inOnMobile = false;
        }
    }

    private void killReward(int luv)//奖励金币
    {
        FlowInflationAttribute optionsImage = ServerStoreRead.GetFlowInflationAttribute();
        optionsImage.gold += luv + 1;//怪物等级 + 1个金币
        ServerStoreRead.SetFlowInflationAttribute(optionsImage);//保存
    }

    private void DamageAndTardget()//攻击进行中执行，移动下（不够攻击距离）不执行
    {
        attackTiem += Time.deltaTime;

        heroGameObject.transform.forward = target.transform.position - heroGameObject.transform.position;//面向攻击对象

        if (attackTiem >= attackInterval) //已间隔时间大于等于攻击间隔就给予伤害
        {
            HeroAttribute targetHeroAttribute = target.GetComponent<HeroAttribute>();
            if (targetHeroAttribute != null && targetHeroAttribute.live)//是否满足给予伤害条件
            {
                //计算并给予伤害
                bool bDiedWithout = targetHeroAttribute.HpAcceptDamage(heroAttribute.AttributeList.damageValueInt, heroAttribute.AttributeList.damageClassType);
                if (bDiedWithout)//判断目标接受伤害后，死了没有
                {
                    killReward(targetHeroAttribute.AttributeList.levelInt);//给予金币奖励
                    if (heroAttribute.ExperienceObtain(1))//给予经验值
                    {
                        upGradeLevel.Invoke();//运行升级事件
                    }
                    targetHeroAttribute.Death();//进行死亡操作
                    StopImmediately();//立刻暂停
                }
                //AnimatorStateInfo animatorInfo = animator.GetCurrentAnimatorStateInfo(0);
            }

            attackTiem = 0f;//打到目标死亡，或目标为null就不攻击，又或自己死亡 //用迭代器，携程重复
            attackInterval = animator.GetCurrentAnimatorStateInfo(0).length;//获取动画播放需要的时间，真实可靠
        }
    }

    private bool playAttackAnimation = false;//播放攻击动画中
    public bool InOnMobile()//判断移动状态，目前是否有移动
    {
        if (inOnMobile)
        {
            MainCameraTransform.position = new Vector3(heroGameObject.transform.position.x + 10f, MainCameraTransform.position.y, heroGameObject.transform.position.z + 15f);
            MapCameraTransform.position = new Vector3(heroGameObject.transform.position.x, MapCameraTransform.position.y, heroGameObject.transform.position.z);
        }

        if (target != null)
        {
            Vector3 iVector3 = heroGameObject.transform.position;//我的位置
            Vector3 enemyVector3 = target.transform.position;//目标位置
            if (heroAttribute.GetRangeOrNot(iVector3, enemyVector3))//判断是否还在攻击范围内
            {
                if (!playAttackAnimation)//攻击距离不够，进入移动状态，所以之前的属性归零
                {
                    MoveToTarget(heroGameObject.transform.position);//停止移动
                    animator.SetInteger("animation", (int)HeroActionType.攻击一);//播放攻击动画

                    attackTiem = 0f;//打到目标死亡，或目标为null就不攻击，又或自己死亡 //用迭代器，携程重复
                    attackInterval = animator.GetCurrentAnimatorStateInfo(0).length;//获取动画播放需要的时间，真实可靠

                    playAttackAnimation = true;
                }
                DamageAndTardget();//

                return false;
            }
            //heroGameObject.transform.forward = target.transform.position - heroGameObject.transform.position;//面向攻击对象
            MoveToTarget(target.transform.position);//命令移动到目标
            playAttackAnimation = false;

            return false;
        }

        return Vector3.Distance(targetVector3, heroGameObject.transform.position) <= 0.55f && inOnMobile;
    }

    public void SynchronousHeroInformation()//英雄信息同步，让MouseControlledMovement需要的数据和HeroAttributeList一致
    {
        animator = heroAttribute.GetComponent<Animator>();//获取动画控制器
        //OnlySynchronousHeroInformation();//同步HeroAttributeList
        agent.speed = heroAttribute.AttributeList.MovementSpeedFloat;//移动速度
        animator.speed = heroAttribute.AttributeList.damageSpeedFloat;//设置动作播放速度
    }

    public void OnDeath(GameObject gameObject)//调用即死亡
    {
        this.enabled = false;//关闭玩家控制功能
        animator.SetInteger("animation", (int)HeroActionType.death);//播放死亡动画
        Instantiate(Resources.Load<GameObject>("preset/UI/global/FailureEndCanvas")).SetActive(true);//创建并显示失败面板
    }


    public Initialize initialize; //初始化时运行该事件
    public UpGradeLevel upGradeLevel;//角色升级时运行该事件
}

[System.Serializable]
public class Initialize : UnityEngine.Events.UnityEvent { }
[System.Serializable]
public class UpGradeLevel : UnityEngine.Events.UnityEvent { }