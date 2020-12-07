using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DetailedInformationBgImage : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private BackpackSpace backpackSpace;

    private Image icoImage;//图标
    private Text nameText;//名字
    private Text itemsTypeText;//物品类型
    private Text equipmentTypeText;//装备类型
    private Text priceText;//单价
    private Text cDText;//使用间隔
    private Text attributeText;//属性（介绍）
    private void OnStart()
    {
        icoImage = transform.Find("IcoImage").GetComponent<Image>();
        nameText = transform.Find("NameText").GetComponent<Text>();
        itemsTypeText = transform.Find("ItemsTypeText").GetComponent<Text>();
        equipmentTypeText = transform.Find("EquipmentTypeText").GetComponent<Text>();
        priceText = transform.Find("PriceText").GetComponent<Text>();
        cDText = transform.Find("CDText").GetComponent<Text>();
        attributeText = transform.Find("AttributeText").GetComponent<Text>();
    }

    public void SetBackpackSpace(BackpackSpace baS)
    {
        backpackSpace = baS;
        OnStart();
        PpdateContent();
    }

    private void PpdateContent()//更新显示内容
    {
        nameText.text = backpackSpace.items.items.name;//设置名字

        if (backpackSpace.items.items.itemsGainAttribute.Length > 0)//效果显示
        {
            string effectActive = "";//默认效果
            string defaultEffect = "";//主动效果
            for (int index = 0; index <= backpackSpace.items.items.itemsGainAttribute.Length - 1; index++)//效果说明
            {
                if (backpackSpace.items.items.itemsGainAttribute[index].itemsEffectType == ItemsEffectType.ActiveEffect)//如果是主动效果
                {
                    if (defaultEffect.Length == 0) defaultEffect = "主动效果："; else defaultEffect = defaultEffect + "，";

                    if (backpackSpace.items.items.itemsGainAttribute[index].value != 0)
                        defaultEffect = defaultEffect + "增加" + backpackSpace.items.items.itemsGainAttribute[index].value + backpackSpace.items.items.itemsGainAttribute[index].itemsGainAttributeListType.ToString();
                    else
                        defaultEffect = defaultEffect + "增加" + backpackSpace.items.items.itemsGainAttribute[index].itemsGainAttributeListType.ToString();
                    if (backpackSpace.items.items.itemsGainAttribute[index].cd > 0) cDText.text = "CD：" + backpackSpace.items.items.itemsGainAttribute[index].cd;//间隔设置
                }
                else if (backpackSpace.items.items.itemsGainAttribute[index].itemsEffectType == ItemsEffectType.DefaultEffect) //默认效果
                {
                    if (effectActive.Length == 0) effectActive = "默认效果："; else effectActive = effectActive + "，";

                    if (backpackSpace.items.items.itemsGainAttribute[index].value != 0)
                        effectActive = effectActive + "增加" + backpackSpace.items.items.itemsGainAttribute[index].value + backpackSpace.items.items.itemsGainAttribute[index].itemsGainAttributeListType.ToString();
                    else
                        effectActive = effectActive + "增加" + backpackSpace.items.items.itemsGainAttribute[index].itemsGainAttributeListType.ToString();
                }
            }
            attributeText.text = effectActive + System.Environment.NewLine + defaultEffect;
        }

        icoImage.sprite = Resources.Load<Sprite>(backpackSpace.items.items.ico);//设置图标

        if (backpackSpace.items.items.itemsType == ItemsType.equipment)
            equipmentTypeText.text = ItemsAttribute.EquipmentTypeToString(backpackSpace.items.items.equipmentType);
        else
            equipmentTypeText.gameObject.SetActive(false);

        itemsTypeText.text = ItemsAttribute.ItemsTypeToString(backpackSpace.items.items.itemsType);
        priceText.text = "单价：" + backpackSpace.items.items.price;
    }

    public void OnBeginDrag(PointerEventData eventData)//开始拖拉
    {
    }

    public void OnDrag(PointerEventData eventData)//拖拉进行中
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)//拖拉结束
    {
    }

    public void OnShutDown()//关闭按钮
    {
        backpackSpace.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1f);
        backpackSpace.doNotDrag = false;
        Destroy(gameObject);
    }

    public void OnWearItems()//穿戴物品
    {
    }

    public void OnAllSellItems()//全部出售
    {
        int totalPrice = backpackSpace.items.items.price;
        if (backpackSpace.items.number > 0) totalPrice = totalPrice * backpackSpace.items.number;//总价

        //置空穿戴栏

        backpackSpace.ParentPanel.itemsListBgImage.SellOut(backpackSpace, totalPrice,CurrencyType.金币);//获得金币 //背包和货币属性存储
        OnShutDown();
    }
}
