using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

//[RequireComponent(typeof(HeroAttribute), typeof(NavMeshAgent))]//
public class AIControlledMovement : MonoBehaviour
{
    public new string name;
    //public float scopeAlert;//警戒范围
    private NavMeshAgent agent;//导航代理
    private GameObject heroGameObject;//英雄物体
    private bool inOnMobile = false;//是否是移动中
    private Vector3 targetVector3; //目标位置
    private Animator animator;//动画控制器
    private HeroAttribute heroAttribute;//AI属性
    private GameObject target;//攻击目标

    //private GameObject sphereCollider;//检测是否进入攻击范围
    //private AIPicket aIPicket;

    private MouseControlledMovement mouseControlledMovement;//获取玩家身上的MouseControlledMovement

    private float attackInterval;//攻击间隔
    private float attackTiem;

    public Transform hpSlotCanvas { set; get; }//血条显示器

    private Coroutine onUpdateCoroutine = null;//防止重复调用
    public void Start()
    {
        heroGameObject = gameObject;
        agent = heroGameObject.GetComponent<NavMeshAgent>();//获取导航网格代理组件

        hpSlotCanvas = transform.Find("HpSlotCanvas");//

        heroAttribute = heroGameObject.GetComponent<HeroAttribute>();//获取HeroAttribute
        heroAttribute.AIStart(this);//数据同步
        heroAttribute.deathEvent.AddListener(OnDeath);//添加死亡事件

        hpSlotCanvas.Find("NameText").GetComponent<Text>().text = name;
        hpSlotCanvas.Find("LvText").GetComponent<Text>().text = "Lv:" + heroAttribute.AttributeList.levelInt;
    }

    public void MoveToTarget(Vector3 temporaryVector3)//移动到目的位置
    {
        if (onUpdateCoroutine == null) onUpdateCoroutine = StartCoroutine(OnUpdate());

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
        StopCoroutine(onUpdateCoroutine);//终止
        onUpdateCoroutine = null;
        hpSlotCanvas.rotation = Quaternion.Euler(new Vector3(45, -140, 0));//不调整UI（血条）的显示就会出现问题
    }

    public void SetAttackTarget(GameObject other)//设置攻击对象，命令AI去攻击该目标
    {
        if (other == null) return;//为null（没有游戏物体）不攻击
        if (!other.activeSelf) return;//不激活不能攻击

        this.target = other.gameObject;//设置攻击目标
        //attackTiem = 0f;//残余属性重置
        //playAttackAnimation = false;

        if(onUpdateCoroutine == null) onUpdateCoroutine = StartCoroutine(OnUpdate());
    }

    IEnumerator OnUpdate()//状态，每帧循环做：移动，攻击
    {
        yield return 1;//new WaitForSeconds(Time.deltaTime);

        if (InOnMobile() && inOnMobile)//判断导航结束 agent.remainingDistance
        {
            if (target == null)
            {
                StopCoroutine(onUpdateCoroutine);//终止
                onUpdateCoroutine = null;
                animator.SetInteger("animation", (int)HeroActionType.Default);
            }
            inOnMobile = false;
        }

        onUpdateCoroutine = StartCoroutine(OnUpdate());
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
                    targetHeroAttribute.Death();//进行死亡操作
                    StopImmediately();//立刻暂停
                }
                //AnimatorStateInfo animatorInfo = animator.GetCurrentAnimatorStateInfo(0);
            }

            attackTiem = 0f;//打到目标死亡，或目标为null就不攻击，又或自己死亡 //用迭代器，携程重复
            attackInterval = animator.GetCurrentAnimatorStateInfo(0).length;//获取动画播放需要的时间，真实可靠
        }

        hpSlotCanvas.rotation = Quaternion.Euler(new Vector3(45, -140, 0));//不调整UI（血条）的显示就会出现问题
    }

    private bool playAttackAnimation = false;//播放攻击动画中
    public bool InOnMobile()//判断移动状态，目前是否有移动
    {
        if (inOnMobile)
        {
            hpSlotCanvas.rotation = Quaternion.Euler(new Vector3(45, -140, 0));
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
                DamageAndTardget();//进行攻击

                StopCoroutine(onUpdateCoroutine);//终止
                onUpdateCoroutine = null;

                return false;
            }
            MoveToTarget(target.transform.position);//命令移动到目标
            playAttackAnimation = false;

            return false;
        }

        return Vector3.Distance(targetVector3, heroGameObject.transform.position) <= 0.55f && inOnMobile;
    }

    public void SynchronousHeroInformation()//英雄信息同步，让MouseControlledMovement需要的数据和HeroAttributeList一致
    {
        animator = heroGameObject.GetComponent<Animator>();//获取动画控制器
        agent.speed = heroAttribute.AttributeList.MovementSpeedFloat;//移动速度
        animator.speed = heroAttribute.AttributeList.damageSpeedFloat;//设置动作播放速度
    }

    public void OnDeath(GameObject gameObjectL)//调用即死亡
    {
        if (onUpdateCoroutine != null)
        {
            StopCoroutine(onUpdateCoroutine);//终止
            onUpdateCoroutine = null;
        }

        this.enabled = false;//关闭AI功能
        animator.SetInteger("animation", (int)HeroActionType.death);
        Destroy(gameObject, 3.14f);
        hpSlotCanvas.rotation = Quaternion.Euler(new Vector3(45, -140, 0));//不调整UI（血条）的显示就会出现问题
    }

    private void OnMouseDown()//被玩家点击，即是被玩家攻击
    {
        if (!heroAttribute.live) return;

        mouseControlledMovement = GameObject.Find("PlayerObject").GetComponent<MouseControlledMovement>();//获取玩家
        mouseControlledMovement.SetAttackTarget(heroGameObject);//要被玩家攻击了

        //target = mouseControlledMovement.transform.Find("Hero/hero").gameObject;//设置反击对象
        //if (onUpdateCoroutine == null) onUpdateCoroutine = StartCoroutine(OnUpdate());
        SetAttackTarget(mouseControlledMovement.transform.Find("Hero/hero").gameObject);
        
        Texture2D cursorb = Resources.Load<Texture2D>("icon/mouse/mouse01");
        Cursor.SetCursor(cursorb, Vector2.zero, CursorMode.Auto);
    }

    private void OnMouseEnter()//鼠标进入
    {
        if (heroAttribute.live)//如果目标可以攻击，就显示
        {
            Texture2D cursorb = Resources.Load<Texture2D>("icon/mouse/mouse42");
            Cursor.SetCursor(cursorb, Vector2.zero, CursorMode.Auto);
        }
    }

    private void OnMouseExit() //鼠标离开
    {
        Texture2D cursorb = Resources.Load<Texture2D>("icon/mouse/mouse01");
        Cursor.SetCursor(cursorb, Vector2.zero, CursorMode.Auto);
    }

    //public Initialize initialize; //初始化时运行该事件
}