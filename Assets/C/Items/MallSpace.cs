using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MallSpace : MonoBehaviour, IPointerDownHandler
{
    public MallBgImage mallBgImage;//商城面板
    private SpecificItems specificItems = null;//物品信息
    private int price;//价格

    public void UpdateItemInformation(string textItem,int number, int priceL)//更新物品信息（物品信息 数量 价格）
    {
        string[] key = textItem.Split('|');//类型ID，名字，图标，物品类型，装备类型，物品价格，最后是增益属性
        specificItems = new SpecificItems();
        specificItems.items.ID = int.Parse(key[0]);
        specificItems.items.name = key[1];
        specificItems.items.ico = key[2];
        specificItems.items.itemsType = (ItemsType)System.Enum.Parse(typeof(ItemsType), key[3]);
        specificItems.items.equipmentType = (EquipmentType)System.Enum.Parse(typeof(EquipmentType), key[4]);
        specificItems.items.price = int.Parse(key[5]);

        string[] effectL = key[6].Split(',');
        specificItems.items.itemsGainAttribute = new ItemsGainAttribute[effectL.Length];

        for (int indexes = 0; indexes <= effectL.Length - 1; indexes++)//增益属性*增益属性效果*增益值*使用间隔|分割
        {
            string[] gainS = effectL[indexes].Split('*');

            specificItems.items.itemsGainAttribute[indexes] = new ItemsGainAttribute(
                (ItemsEffectType)System.Enum.Parse(typeof(ItemsEffectType), gainS[0]),
                (ItemsGainAttributeListType)System.Enum.Parse(typeof(ItemsGainAttributeListType), gainS[1]), int.Parse(gainS[2]), float.Parse(gainS[3])
                );
        }

        specificItems.number = number; //设置数量
        price = priceL;//设置价格
        GetComponent<Image>().sprite = Resources.Load<Sprite>(key[2]);
    }

    public void OnPointerDown(PointerEventData eventData)//按下时触发
    {
        if (mallBgImage.message == null)
        {
            mallBgImage.message = Instantiate(Resources.Load<GameObject>("preset/UI/global/items/GoodsMessageBgImage"));
            mallBgImage.message.transform.SetParent(mallBgImage.transform.parent);
            mallBgImage.message.transform.localPosition = new Vector3(0, 0, 0);
            mallBgImage.message.transform.localScale = new Vector3(1, 1, 1);
        }
        GoodsMessageBgImage goodsMessageBgImage = mallBgImage.message.GetComponent<GoodsMessageBgImage>();
        if (specificItems.items.ID == goodsMessageBgImage.ID) return;
        if (goodsMessageBgImage.optionsImage == null) goodsMessageBgImage.optionsImage = mallBgImage.optionsImage;
        goodsMessageBgImage.UpdateCommodityInformation(specificItems, mallBgImage.CurrencyType);
    }
}
