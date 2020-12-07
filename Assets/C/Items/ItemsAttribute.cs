using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemsEffectType//物品效果列表（默认效果，主动效果，被动效果，唯一被动）
{
    DefaultEffect,//默认效果,
    ActiveEffect,//主动效果,
    PassiveEffect,//被动效果,
    OnlyPassive//唯一被动
}

public enum ItemsGainAttributeListType//物品增益属性列表
{
    未知属性,
    生命值,//当前
    魔法值,//当前
    最大生命值,
    最大魔法值,
    攻击力,
    护甲,
    力量,
    敏捷,
    智力
}

public enum EquipmentType //装备类型
{
    no,//无
    Helmet,//头盔
    Gloves,//手套
    Armor,//护甲（衣服）
    ShinGuards,//护腿（裤子）
    Shoes,//鞋子
    Weapons//武器
}
public enum ItemsType//物品类型
{
    unknown,//未知
    equipment,//装备
    material,//材料
    consumables//消耗品
}

[System.Serializable]
public class ItemsGainAttribute//增益属性
{
    public ItemsGainAttribute(){}

    public ItemsGainAttribute(ItemsEffectType itemsEffectTypeL, ItemsGainAttributeListType itemsGainAttributeListTypeL, int valueL, float cdL)
    {
        itemsEffectType = itemsEffectTypeL;
        itemsGainAttributeListType = itemsGainAttributeListTypeL;
        value = valueL;
        cd = cdL;
    }

    public ItemsEffectType itemsEffectType = ItemsEffectType.DefaultEffect;//增益属性效果
    public ItemsGainAttributeListType itemsGainAttributeListType = ItemsGainAttributeListType.未知属性;//增益属性
    public int value;//增益值
    public float cd = 0;//使用间隔，不是主动效果可为0
}

[System.Serializable]
public class ItemsAttribute// : MonoBehaviour //物品信息
{
    //类型ID，名字，图标，物品类型，装备类型，物品价格，最后是增益属性
    public int ID = -1;//类型ID //为-1为空
    public string name;//名字
    public string ico;//图标
    public ItemsType itemsType = ItemsType.unknown;//物品类型
    public EquipmentType equipmentType = EquipmentType.no;//装备类型

    public ItemsGainAttribute[] itemsGainAttribute;//增益属性列表

    //public int number;//数量，为0则无限

    //public int wearPosition = 0; //为 -1 / 0 则无穿戴，能穿戴的为1-6

    public int price;//物品价格，单个价格
    public static bool IsRectTransformOverlap(RectTransform rect1, RectTransform rect2)//是否穿戴（两个Image是否重叠）
    {
        float rect1MinX = rect1.position.x - rect1.rect.width / 2;
        float rect1MaxX = rect1.position.x + rect1.rect.width / 2;
        float rect1MinY = rect1.position.y - rect1.rect.height / 2;
        float rect1MaxY = rect1.position.y + rect1.rect.height / 2;

        float rect2MinX = rect2.position.x - rect2.rect.width / 2;
        float rect2MaxX = rect2.position.x + rect2.rect.width / 2;
        float rect2MinY = rect2.position.y - rect2.rect.height / 2;
        float rect2MaxY = rect2.position.y + rect2.rect.height / 2;

        bool xNotOverlap = rect1MaxX <= rect2MinX || rect2MaxX <= rect1MinX;
        bool yNotOverlap = rect1MaxY <= rect2MinY || rect2MaxY <= rect1MinY;

        bool notOverlap = xNotOverlap || yNotOverlap;

        return !notOverlap;
    }
    public static string ItemsTypeToString(ItemsType itemsType)
    {
        string key = "";
        if (itemsType == ItemsType.consumables)
            key = "消耗品";
        else if (itemsType == ItemsType.equipment)
            key = "装备";
        else if (itemsType == ItemsType.material)
            key = "材料";
        else
            key = "未知";
        return key;
    }

    public static string EquipmentTypeToString(EquipmentType equipmentType)
    {
        string key = "无";
        if (equipmentType == EquipmentType.Armor)
            key = "护甲";
        else if (equipmentType == EquipmentType.Helmet)
            key = "头盔";
        else if (equipmentType == EquipmentType.Gloves)
            key = "手套";
        else if (equipmentType == EquipmentType.ShinGuards)
            key = "护腿";
        else if (equipmentType == EquipmentType.Shoes)
            key = "鞋子";
        else if (equipmentType == EquipmentType.Weapons)
            key = "武器";
        return key;
    }
}

[System.Serializable]
public class SpecificItems
{
    public ItemsAttribute items = new ItemsAttribute(); //物品信息

    public int wearLocation = 0; //是否穿戴（穿戴位置），为0则默认没有

    public int number;//数量，为0则无限
}