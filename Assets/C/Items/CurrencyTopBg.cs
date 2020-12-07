using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FlowInflationAttribute
{
    public int gold;//金币
    public int diamond;//钻石
    public int ava;//点卷

    public FlowInflationAttribute()
    {
    }

    public FlowInflationAttribute(int g, int d, int a)
    {
        gold = g;
        diamond = d;
        ava = a;
    }
}
public class CurrencyTopBg : MonoBehaviour
{
    private FlowInflationAttribute flowInflationAttribute;//静态的，全TopBgPlayerAttributes只有一个flowInflationAttribute
    public FlowInflationAttribute FlowInflationAttribute
    {
        get { return flowInflationAttribute; }
    }
    //public MouseControlledMovement mouseControlledMovement;//玩家控制器
    private Text goldText;
    private Text diamondText;
    private Text avaText;
    private void Awake()//如果不在mouseControlledMovement初始化前先添加OnStart方法，就会错过mouseControlledMovement初始化时调用OnStart方法
    {
        //mouseControlledMovement = GameObject.Find("PlayerObject").GetComponent<MouseControlledMovement>();
        goldText = transform.Find("GoldBgImage/Text").GetComponent<Text>();
        diamondText = transform.Find("DiamondBgImage/Text").GetComponent<Text>();
        avaText = transform.Find("AvaBgImage/Text").GetComponent<Text>();
        //mouseControlledMovement.initialize.AddListener(OnStart);//添加初始方法，在玩家控制器初始化后再初始化，不然空指针
        OnUpdateCurrency(ServerStoreRead.GetFlowInflationAttribute());
    }

    public void OnUpdateCurrency(FlowInflationAttribute flowInf)//更新货币信息
    {
        flowInflationAttribute = flowInf;//获取货币属性
        //flowInflationAttribute.gold = fInflationAttributeL.gold;
        //flowInflationAttribute.diamond = fInflationAttributeL.diamond;
        //flowInflationAttribute.ava = fInflationAttributeL.ava;
        goldText.text = flowInflationAttribute.gold.ToString();
        diamondText.text = flowInflationAttribute.diamond.ToString();
        avaText.text = flowInflationAttribute.ava.ToString();//货币显示
    }
}
