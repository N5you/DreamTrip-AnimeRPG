using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillsImage : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public KeyCode keyCode;//技能快捷键
    //public int consume;//魔法消耗
    //public static string describe = "";//技能描述
    //public static string skillsName = "";//技能名字
    public HeroSkills heroSkills { get; set; }//技能属性
    public MouseControlledMovement mouseControlledMovement { get; set; }//玩家控制器

    public SkillsEvent skillsEvent; //调用该事件即触发技能
    //public delegate void skillsDelegate(GameObject go, int sh, GameObject[] mub); //skillsDelegate.skillsEvent
    //public event skillsDelegate skillsEvent;

    public float skillsCd;//技能CD
    private float cdDelta;//冷却了多久了

    private bool availableOrNot = true;//是否可用，默认可用
    //private bool availableOrYot = false;

    private Text cdText;//当前CD时
    private Image cdImgae;//
    private void Start()
    {
        cdText = transform.Find("Text").GetComponent<Text>();
        cdImgae = transform.Find("Image").GetComponent<Image>();
    }
    private void Update()
    {
        if (availableOrNot)
        {
            if (Input.GetKeyDown(keyCode))
            {
                availableOrNot = false;

                //availableOrYot = true;
                cdText.gameObject.SetActive(true);
                int cdInt = (int)skillsCd;
                cdText.text = cdInt.ToString();

                cdImgae.gameObject.SetActive(true);
                cdImgae.fillAmount = 1f;

                OnSkills();//触发技能
                //skillsEvent(gameObject, 10, new GameObject[8]);

                coroutine = StartCoroutine(SkillsCD());
            }
        }
    }

    private GameObject skillsInstructions;//heroSkills
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if(skillsInstructions != null) Destroy(skillsInstructions);

        skillsInstructions = Instantiate(Resources.Load<GameObject>("preset/UI/global/SkillsInstructionsBgImage"));// preset/UI/global/SkillsInstructionsBgImage

        skillsInstructions.transform.SetParent(transform.parent.parent);//设置父物体

        skillsInstructions.transform.Find("InstructionsText").GetComponent<Text>().text = heroSkills.describe;//介绍
        skillsInstructions.transform.Find("NameText").GetComponent<Text>().text = heroSkills.name;//名字
        skillsInstructions.transform.Find("ConsumptionText").GetComponent<Text>().text = "MP：" + heroSkills.consume;//消耗
        skillsInstructions.transform.Find("Shortcuts").GetComponent<Text>().text = "快捷键：" + heroSkills.shortcuts;//快捷键
        skillsInstructions.transform.Find("CDText").GetComponent<Text>().text = "CD：" + heroSkills.cooling;//冷却

        skillsInstructions.transform.GetComponent<RectTransform>().localPosition = new Vector3(0, -125, 0);//设置位置
    }
    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)//点击结束时触发
    {
        Destroy(skillsInstructions);
    }

    private Coroutine coroutine;//用返回的Coroutine来yield return可以等待协同程序运行后，再继续运行
    private IEnumerator SkillsCD()
    {
        yield return 1;

        cdDelta += Time.deltaTime;

        int cdInt = (int)(skillsCd - cdDelta);
        cdText.text = cdInt.ToString();

        cdImgae.fillAmount = (skillsCd - cdDelta) / skillsCd;

        if (cdDelta >= skillsCd)
        {
            ResetCooling();
        }
        else
        {
            coroutine = StartCoroutine(SkillsCD());
        }
    }

    public void ResetCooling() //重置技能冷却时间
    {
        availableOrNot = true;
        //availableOrYot = false;
        cdText.gameObject.SetActive(false);
        cdImgae.gameObject.SetActive(false);
        cdDelta = 0f;
        StopCoroutine(coroutine);
    }

    public void OnSkills()//触发绑定技能
    {
        object[] obj = new object[2];
        obj[0] = mouseControlledMovement.GetHeroGameObject;
        obj[1] = mouseControlledMovement.GetHeroAttribute;
        object ni = heroSkills.methodlnfo.Invoke(heroSkills.skills, obj);

        if (!(bool)ni)//魔法值不够时
        {
            ResetCooling();
            return;
        }
        skillsEvent.Invoke();
    }

    private void OnDestroy()//脚本或物体被销毁时调用（在执行OnDestroy前会先执行OnDisable）
    {
        //StopCoroutine(coroutine);//因为脚本销毁时coroutine被释放，所以控制台窗口输出（报错）为空
    }
}

[System.Serializable]
public class SkillsEvent : UnityEngine.Events.UnityEvent { }//使用技能事件