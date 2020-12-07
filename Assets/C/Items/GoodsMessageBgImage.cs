using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public enum CurrencyType//货币类型
{
    金币 = 0,
    钻石 = 1,
    点卷 = 2
}

public class GoodsMessageBgImage : MonoBehaviour//购买物品信息面板
{
    public OptionsImage optionsImage;
    private Image icoImage;
    private Text nameText;
    private Text totalPriceText;//总价
    private InputField inputField;//输入框
    private Text utemsTypeText;//物品类型
    private Text equipmentTypeText;//装备类型
    private Text priceText;//单价
    private Text cDText;//使用间隔
    private Text attributeText;//具体属性说明

    private int id = -1;//物品ID
    public int ID
    {
        get { return id; }
    }
    private int numbe = 1;//数量
    private int totalPrice = 0;//总价
    private int price = 0;//单价
    private CurrencyType currencyType;
    private ItemsType itemsType;//物品类型

    private Text currencyText;//货币数量
    private void Awake()
    {
        currencyText = transform.Find("CurrencyText").GetComponent<Text>();
        icoImage = transform.Find("IcoImage").GetComponent<Image>();
        nameText = transform.Find("NameText").GetComponent<Text>();
        totalPriceText = transform.Find("TotalPriceText").GetComponent<Text>();
        inputField = transform.Find("InputField").GetComponent<InputField>();
        utemsTypeText = transform.Find("ItemsTypeText").GetComponent<Text>();
        equipmentTypeText = transform.Find("EquipmentTypeText").GetComponent<Text>();
        priceText = transform.Find("PriceText").GetComponent<Text>();
        cDText = transform.Find("CDText").GetComponent<Text>();
        attributeText = transform.Find("AttributeText").GetComponent<Text>();
    }
    public void UpdateCommodityInformation(SpecificItems specificItems, CurrencyType currencyTypeL)//更新商品信息
    {
        nameText.text = specificItems.items.name;//设置名字

        if (specificItems.items.itemsGainAttribute.Length > 0)//效果显示
        {
            string effectActive = "";//默认效果
            string defaultEffect = "";//主动效果
            for (int index = 0; index <= specificItems.items.itemsGainAttribute.Length - 1; index++)//效果说明
            {
                if (specificItems.items.itemsGainAttribute[index].itemsEffectType == ItemsEffectType.ActiveEffect)//如果是主动效果
                {
                    if (defaultEffect.Length == 0) defaultEffect = "主动效果："; else defaultEffect = defaultEffect + "，";

                    if (specificItems.items.itemsGainAttribute[index].value != 0)
                        defaultEffect = defaultEffect + "增加" + specificItems.items.itemsGainAttribute[index].value + specificItems.items.itemsGainAttribute[index].itemsGainAttributeListType.ToString();
                    else
                        defaultEffect = defaultEffect + "增加" + specificItems.items.itemsGainAttribute[index].itemsGainAttributeListType.ToString();
                    if (specificItems.items.itemsGainAttribute[index].cd > 0) cDText.text = "CD：" + specificItems.items.itemsGainAttribute[index].cd;//间隔设置
                }
                else if (specificItems.items.itemsGainAttribute[index].itemsEffectType == ItemsEffectType.DefaultEffect) //默认效果
                {
                    if (effectActive.Length == 0) effectActive = "默认效果："; else effectActive = effectActive + "，";

                    if (specificItems.items.itemsGainAttribute[index].value != 0)
                        effectActive = effectActive + "增加" + specificItems.items.itemsGainAttribute[index].value + specificItems.items.itemsGainAttribute[index].itemsGainAttributeListType.ToString();
                    else
                        effectActive = effectActive + "增加" + specificItems.items.itemsGainAttribute[index].itemsGainAttributeListType.ToString();
                }
            }
            attributeText.text = effectActive + System.Environment.NewLine + defaultEffect;
        }

        icoImage.sprite = Resources.Load<Sprite>(specificItems.items.ico);//设置图标

        if (specificItems.items.itemsType == ItemsType.equipment)
            equipmentTypeText.text = ItemsAttribute.EquipmentTypeToString(specificItems.items.equipmentType);
        else
            equipmentTypeText.gameObject.SetActive(false);

        utemsTypeText.text = ItemsAttribute.ItemsTypeToString(specificItems.items.itemsType);
        itemsType = specificItems.items.itemsType;//物品类型
        priceText.text = "单价：" + specificItems.items.price;

        if (specificItems.items.itemsType != ItemsType.consumables)
        {
            numbe = 1;
            inputField.text = "1";
            inputField.enabled = false;
        }
        id = specificItems.items.ID;
        price = specificItems.items.price;
        currencyType = currencyTypeL;

        switch (currencyTypeL)//拥有货币显示
        {
            case CurrencyType.金币:
                currencyText.text = "拥有：" + optionsImage.FlowInflationAttribute.gold + " 金币";
                break;
            case CurrencyType.钻石:
                currencyText.text = "拥有：" + optionsImage.FlowInflationAttribute.diamond + " 钻石";
                break;
            case CurrencyType.点卷:
                currencyText.text = "拥有：" + optionsImage.FlowInflationAttribute.ava + " 点卷";
                break;
        }
        OnUpdateTotalPrice();
    }

    public void OnShutDown()//关闭按钮
    {
        Destroy(gameObject);
    }

    public void OnUpdateTotalPrice()//更新总价
    {
        int key = 1;
        if (inputField.text.Length > 0)
            key = int.Parse(inputField.text);
        else
            inputField.text = "1";

        if (key <= 0)
        {
            inputField.text = "1";
            key = 1;
        }
        numbe = key;//数量设置
        totalPrice = price * key;//总价显示
        totalPriceText.text = "总共需要：" + totalPrice + " " + System.Enum.GetName(currencyType.GetType(), currencyType);
    }

    public void OnBuy()//购买按钮
    {
        int key = 0;
        switch (currencyType)//获取货币属性
        {
            case CurrencyType.金币:
                key = optionsImage.FlowInflationAttribute.gold;
                break;
            case CurrencyType.钻石:
                key = optionsImage.FlowInflationAttribute.diamond;
                break;
            case CurrencyType.点卷:
                key = optionsImage.FlowInflationAttribute.ava;
                break;
        }

        if (totalPrice > key)//判断货币是否足够
        {
            int difference = totalPrice - key;
            totalPriceText.text = "买不起，还差 " + difference + " " + System.Enum.GetName(currencyType.GetType(), currencyType);
            return;
        }

        TipPlateBgImage prompt = Instantiate(Resources.Load<GameObject>("preset/UI/TipPlateBgImage")).GetComponent<TipPlateBgImage>();//提示
        if (GiveItems(id, numbe))//给予物品
        {
            switch (currencyType)//扣钱
            {
                case CurrencyType.金币:
                    optionsImage.FlowInflationAttribute.gold -= totalPrice;
                    break;
                case CurrencyType.钻石:
                    optionsImage.FlowInflationAttribute.diamond -= totalPrice;
                    break;
                case CurrencyType.点卷:
                    optionsImage.FlowInflationAttribute.ava -= totalPrice;
                    break;
            }
            ServerStoreRead.SetFlowInflationAttribute(optionsImage.FlowInflationAttribute);//保存
            prompt.UpdatePromptContent("购买成功", "购买成功后，物品将放入背包，消耗品和材料会自动叠加");
        }
        else
        {
            prompt.UpdatePromptContent("购买失败", "背包已满");
        }
        prompt.transform.SetParent(transform.parent);
        prompt.transform.localPosition = new Vector3(0, 0, 0);
        prompt.transform.localScale = new Vector3(1, 1, 1);

        OnShutDown();//没有更新现有货币，所以直接关闭好了，考虑把更新现有货币做成单独方法
    }

    private bool GiveItems(int itemsID, int itemsNumbe)//物品ID 数量
    {
        bool addB = false;

        ArrayList strL = new ArrayList(ServerStoreRead.GetCourseRoleBackpack());//读取背包信息（物品ID|数量|穿戴位置）//获取当前背包
        if ((itemsType == ItemsType.material || itemsType == ItemsType.consumables) && strL.Count > 1)//是否是消耗品 //是否有同类物品，没有就直接添加
        {
            string[] vStr = (string[])strL.ToArray(typeof(string));
            for (int index = 0; index <= vStr.Length - 1; index++)
            {
                string[] imeS = vStr[index].Split('|');
                if (int.Parse(imeS[0]) == id)
                {
                    int imeSN = int.Parse(imeS[1]) + itemsNumbe;
                    vStr[index] = imeS[0] + "|" + imeSN + "|" + imeS[2];
                    ServerStoreRead.SetCourseRoleBackpack(vStr);
                    return true;//退出循环
                }
            }
        }

        if (strL.Count < 20)//(vStr.Length < optionsImage.itemsListBgImage.backpackBgImage.BackpackSpace.Length)//是否还有容量
        {
            string vlul = itemsID + "|" + itemsNumbe + "|0";
            strL.Add(vlul);
            string[] vStr = (string[])strL.ToArray(typeof(string));
            ServerStoreRead.SetCourseRoleBackpack(vStr);
            addB = true;
        }
        
        return addB;//添加成功或失败，物品栏已满
    }
}