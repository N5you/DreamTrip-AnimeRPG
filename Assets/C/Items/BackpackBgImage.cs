//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackpackBgImage : MonoBehaviour//背包
{
    public ItemsListBgImage itemsListBgImage;
    private BackpackSpace[] backpackSpace = new BackpackSpace[20];//背包的物品栏格子
    public BackpackSpace[] BackpackSpace
    {
        get { return backpackSpace; }
    }
    private void Start()//打开背包后自动初始化
    {
        Transform list = transform.Find("List");
        GameObject BackpackSpace = Resources.Load<GameObject>("preset/UI/global/items/BackpackSpace");//BackpackSpace
        for (int index = 0; index <= backpackSpace.Length - 1; index++)//创建背包空间（格子）
        {
            GameObject backpackSpaceG = Instantiate(BackpackSpace);
            backpackSpace[index] = backpackSpaceG.GetComponent<BackpackSpace>();
            backpackSpaceG.transform.SetParent(list);
            backpackSpace[index].ParentPanel = this;
        }
        GetbackpackItems();
        //ServerStoreRead.SetCourseRoleBackpack(backpackSpace);
    }

    private void GetbackpackItems()//更新全部backpackSpace信息
    {
        string[] vStr = ServerStoreRead.GetCourseRoleBackpack();//读取背包信息（物品ID|数量|穿戴位置）
        if (System.Array.Exists(vStr, string.IsNullOrEmpty)) return;//字符串数组为空就跳过不执行

        //TextAsset text = Resources.Load<TextAsset>("items");//获取装备类型列表
        string[] textItem = Resources.Load<TextAsset>("items").ToString().Split('\n');//按行分割

        for (int index = 0; index <= vStr.Length - 1; index++)
        {
            string[] strV = vStr[index].Split('|'); //格式（物品ID | 数量 | 穿戴位置）
                                                    //if (strV.Length == 0) return;
            int id = int.Parse(strV[0]);
            if (id >= 0) //为-1等于空
            {
                string[] key = textItem[id].Split('|');//类型ID，名字，图标，物品类型，装备类型，物品价格，最后是增益属性
                backpackSpace[index].items.items = new ItemsAttribute();
                backpackSpace[index].items.items.ID = int.Parse(key[0]);
                backpackSpace[index].items.items.name = key[1];
                backpackSpace[index].items.items.ico = key[2];
                backpackSpace[index].items.items.itemsType = (ItemsType)System.Enum.Parse(typeof(ItemsType), key[3]);
                backpackSpace[index].items.items.equipmentType = (EquipmentType)System.Enum.Parse(typeof(EquipmentType), key[4]);
                backpackSpace[index].items.items.price = int.Parse(key[5]);

                string[] effectL = key[6].Split(',');
                backpackSpace[index].items.items.itemsGainAttribute = new ItemsGainAttribute[effectL.Length];

                for (int indexes = 0; indexes <= effectL.Length - 1; indexes++)//增益属性*增益属性效果*增益值*使用间隔|分割
                {
                    string[] gainS = effectL[indexes].Split('*');
                    //backpackSpace[index].Items.itemsGainAttribute[indexes].itemsEffectType = (ItemsEffectType)System.Enum.Parse(typeof(ItemsEffectType), gainS[0]);
                    //backpackSpace[index].Items.itemsGainAttribute[indexes].itemsGainAttributeListType = (ItemsGainAttributeListType)System.Enum.Parse(typeof(ItemsGainAttributeListType), gainS[1]);
                    //backpackSpace[index].Items.itemsGainAttribute[indexes].value = int.Parse(gainS[2]);
                    //backpackSpace[index].Items.itemsGainAttribute[indexes].cd = float.Parse(gainS[3]);
                    backpackSpace[index].items.items.itemsGainAttribute[indexes] = new ItemsGainAttribute(
                        (ItemsEffectType)System.Enum.Parse(typeof(ItemsEffectType), gainS[0]), 
                        (ItemsGainAttributeListType)System.Enum.Parse(typeof(ItemsGainAttributeListType), gainS[1]),int.Parse(gainS[2]), float.Parse(gainS[3])
                        );
                }

                backpackSpace[index].items.number = int.Parse(strV[1]); //设置数量
                backpackSpace[index].items.wearLocation = int.Parse(strV[2]); //设置穿戴
                backpackSpace[index].ItemsDisplay();//设置显示
            }
        }
        //
    }

    public void OnShutDown()//关闭按钮
    {
        Destroy(gameObject);
    }

    public void OnStorage()//数据存储
    {
        ServerStoreRead.SetCourseRoleBackpack(backpackSpace);
    }
}