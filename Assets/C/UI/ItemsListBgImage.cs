using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsListBgImage : MonoBehaviour//物品栏（穿戴栏）
{
    public BackpackBgImage backpackBgImage;
    public CurrencyTopBg currencyTopBg;

    private ItemsContainer[] backpackSpace = new ItemsContainer[6];
    public ItemsContainer[] ItemsContainer
    {
        get { return backpackSpace; }
    }

    private void Start()
    {
        Transform list = transform.Find("List");

        for (int index = 0; index <= backpackSpace.Length - 1; index++)
        {
            backpackSpace[index] = list.GetChild(index).GetComponent<ItemsContainer>();
        }

        foreach (var nini in backpackSpace)
        {
            if (nini == null) return;
            //Debug.Log(nini.name);
        }
        GetItems();//读取
    }

    private void GetItems()//更新全部backpackSpace信息
    {
        string[] vStr = ServerStoreRead.GetCourseRoleWearItems();//读取背包信息（物品ID|数量|穿戴位置）
        if (System.Array.Exists(vStr, string.IsNullOrEmpty)) return;//字符串数组为空就跳过不执行

        string[] textItem = Resources.Load<TextAsset>("items").ToString().Split('\n');//按行分割

        for (int index = 0; index <= vStr.Length - 1; index++)
        {
            string[] strV = vStr[index].Split('|'); //格式（物品ID | 数量 | 穿戴位置）
            int id = int.Parse(strV[0]);
            if (id >= 0) //为-1等于空
            {
                string[] key = textItem[id].Split('|');//类型ID，名字，图标，物品类型，装备类型，物品价格，最后是增益属性
                backpackSpace[index].specificItems = new SpecificItems();
                backpackSpace[index].specificItems.items.ID = int.Parse(key[0]);
                backpackSpace[index].specificItems.items.name = key[1];
                backpackSpace[index].specificItems.items.ico = key[2];
                backpackSpace[index].specificItems.items.itemsType = (ItemsType)System.Enum.Parse(typeof(ItemsType), key[3]);
                backpackSpace[index].specificItems.items.equipmentType = (EquipmentType)System.Enum.Parse(typeof(EquipmentType), key[4]);
                backpackSpace[index].specificItems.items.price = int.Parse(key[5]);

                string[] effectL = key[6].Split(',');
                backpackSpace[index].specificItems.items.itemsGainAttribute = new ItemsGainAttribute[effectL.Length];

                for (int indexes = 0; indexes <= effectL.Length - 1; indexes++)//增益属性*增益属性效果*增益值*使用间隔|分割
                {
                    string[] gainS = effectL[indexes].Split('*');

                    backpackSpace[index].specificItems.items.itemsGainAttribute[indexes] = new ItemsGainAttribute(
                        (ItemsEffectType)System.Enum.Parse(typeof(ItemsEffectType), gainS[0]),
                        (ItemsGainAttributeListType)System.Enum.Parse(typeof(ItemsGainAttributeListType), gainS[1]), int.Parse(gainS[2]), float.Parse(gainS[3])
                        );
                }

                backpackSpace[index].specificItems.number = int.Parse(strV[1]); //设置数量
                backpackSpace[index].specificItems.wearLocation = int.Parse(strV[2]); //设置穿戴
                backpackSpace[index].ItemsDisplay();//设置显示
            }
        }
        //
    }

    private GameObject presetBackpack;//背包预设体
    public void OnOpenBackpack()//打开背包
    {
        if (presetBackpack != null) return;
        presetBackpack = Instantiate(Resources.Load<GameObject>("preset/UI/global/items/BackpackBgImage"));//preset/UI/global/items/BackpackBgImage
        presetBackpack.transform.SetParent(transform.parent, false);
        presetBackpack.transform.localPosition = new Vector3(0, 0, 0);
        backpackBgImage = presetBackpack.GetComponent<BackpackBgImage>();
        backpackBgImage.itemsListBgImage = this;
    }

    public void OnStorage()//数据存储
    {
        SpecificItems[] specificItems = new SpecificItems[backpackSpace.Length];
        for (int index = 0; index <= backpackSpace.Length - 1; index++)
        {
            specificItems[index] = backpackSpace[index].specificItems;
        }
        ServerStoreRead.SetCourseRoleWearItems(specificItems);
    }

    public void SellOut(BackpackSpace backpack, int deduct, CurrencyType currencyType)
    {
        backpack.AEmpty();//置空BackpackSpace//并且更新显示
        backpack.ParentPanel.OnStorage();//用BackpackBgImage保存

        if (currencyTopBg != null)
        {
            FlowInflationAttribute flowInflation = currencyTopBg.FlowInflationAttribute;

            switch (currencyType)
            {
                case CurrencyType.金币:
                    flowInflation.gold += deduct;
                    break;
                case CurrencyType.钻石:
                    flowInflation.diamond += deduct;
                    break;
                case CurrencyType.点卷:
                    flowInflation.ava += deduct;
                    break;
            }

            ServerStoreRead.SetFlowInflationAttribute(flowInflation);//保存
            currencyTopBg.OnUpdateCurrency(flowInflation);
        }
    }
}