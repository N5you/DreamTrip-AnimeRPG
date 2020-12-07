using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class HeroSkills//
{
    public string shortcuts;//快捷键

    public float cooling;//冷却

    public int consume;//魔法消耗

    public string ico;//图标

    public object skills;//技能实现（方法）

    public MethodInfo methodlnfo;//

    public string describe = "";//技能描述

    public string name = "";//技能名字
}

public class HeroSkillsType : MonoBehaviour
{
    public static HeroSkills SkillsID(int id)//输入技能ID获取技能
    {
        HeroSkills heroSkills = new HeroSkills();//

        string key = "SkillsType.HeroSkills" + id;//根据ID获取技能
        Type type = Type.GetType(key);
        object obj = Activator.CreateInstance(type, true);//通过Type创建对象

        //PropertyInfo shortcuts = type.GetProperty("shortcuts", type);//获取快捷键
        heroSkills.shortcuts = (string)obj.GetType().GetField("shortcuts").GetValue(obj);
        //PropertyInfo cooling = type.GetProperty("cooling");//获取冷却
        heroSkills.cooling = (float)obj.GetType().GetField("cooling").GetValue(obj);
        //PropertyInfo consume = type.GetProperty("consume");//获取魔法消耗
        heroSkills.consume = (int)obj.GetType().GetField("consume").GetValue(obj);
        //PropertyInfo ico = type.GetProperty("ico");//获取图标
        heroSkills.ico = (string)obj.GetType().GetField("ico").GetValue(obj);

        heroSkills.name = (string)obj.GetType().GetField("name").GetValue(obj);//获取名字
        heroSkills.describe = (string)obj.GetType().GetField("describe").GetValue(obj);//获取简介

        heroSkills.skills = obj;
        heroSkills.methodlnfo = obj.GetType().GetMethod("Skills");//获取技能方法

        return heroSkills;
    }

    public static GameObject NewGameObject(GameObject go)
    {
        GameObject pL = Instantiate(go);
        return pL;
    }

    //public static void DestroyGameObject(GameObject go, float tiem)
    //{
    //    Destroy(go, tiem);
    //}
}

namespace SkillsType
{
    public class HeroSkills0 // HeroSkills + 类型ID
    {
        public static string shortcuts = "A";//快捷键

        public static float cooling = 8f;//冷却

        public static int consume = 20;//魔法消耗

        public static string ico = "icon/skills/0";//图标

        public static string describe = "释放雷霆万钧可以给予周围（256码）敌人（攻击力+）";//技能描述

        public static string name = "雷霆万钧";//技能名字

        public static void Skills(GameObject go, HeroAttribute heroAttribute)//技能实现（方法）//释放单位，伤害，目标单位
        {
            //
            Debug.Log(666);
            Debug.Log(go.name);
        }
    }

    public class HeroSkills1 // HeroSkills + 类型ID
    {
        public static string shortcuts = "D";//快捷键

        public static float cooling = 8f;//冷却

        public static int consume = 20;//魔法消耗

        public static string ico = "icon/skills/4";//图标

        public static string describe = "释放雷霆万钧可以给予周围（256码）敌人（攻击力+力量值+虚拟伤害）点魔法伤害";//技能描述

        public static string name = "雷霆万钧";//技能名字

        public static bool Skills(GameObject go, HeroAttribute heroAttribute)//技能实现（方法）//释放单位，伤害，目标单位
        {
            bool sbL = heroAttribute.MagicConsumption(consume);//魔法消耗
            if (sbL)
            {
                GameObject presetL = HeroSkillsType.NewGameObject(Resources.Load<GameObject>("effects/Effect1_Collision"));//创建特效
                Vector3 v3 = new Vector3(go.transform.position.x, 5, go.transform.position.z);
                presetL.transform.position = v3;//特性位置
                presetL.transform.rotation = Quaternion.Euler(new Vector3(0, 35, 0));
                //HeroSkillsType.DestroyGameObject(presetL, 3.14f);//删除特效
                SkillsDamageDevice skillsDamageDevice = presetL.GetComponent<SkillsDamageDevice>();
                skillsDamageDevice.damage = heroAttribute.AttributeList.damageValueInt + heroAttribute.AttributeList.powerInt;//设置伤害值
                skillsDamageDevice.damageClassType = DamageClassType.魔法伤害;
                skillsDamageDevice.murderer = heroAttribute.gameObject;
                skillsDamageDevice.StartDamage(3.14f);
            }
            return sbL;
        }
    }

    public class HeroSkills2 // HeroSkills + 类型ID
    {
        public static string shortcuts = "Q";//快捷键

        public static float cooling = 5f;//冷却

        public static int consume = 15;//魔法消耗

        public static string ico = "icon/skills/2";//图标

        public static string describe = "";//技能描述

        public static string name = "烽火之剑";//技能名字

        public static bool Skills(GameObject go, HeroAttribute heroAttribute)//技能实现（方法）//释放单位，伤害，目标单位
        {
            bool sbL = heroAttribute.MagicConsumption(consume);//魔法消耗
            if (sbL)
            {
                GameObject presetL = HeroSkillsType.NewGameObject(Resources.Load<GameObject>("effects/Effect7_Collision"));//创建特效
                Vector3 v3 = new Vector3(go.transform.position.x, 5, go.transform.position.z);
                presetL.transform.position = v3;//特性位置
                presetL.transform.rotation = Quaternion.Euler(new Vector3(0, 35, 0));
                //HeroSkillsType.DestroyGameObject(presetL, 3.14f);//删除特效
                SkillsDamageDevice skillsDamageDevice = presetL.GetComponent<SkillsDamageDevice>();
                skillsDamageDevice.damage = heroAttribute.AttributeList.damageValueInt + heroAttribute.AttributeList.powerInt;//设置伤害值
                skillsDamageDevice.damageClassType = DamageClassType.魔法伤害;
                skillsDamageDevice.murderer = heroAttribute.gameObject;
                skillsDamageDevice.StartDamage(3.14f);
            }
            return sbL;
        }
    }

    public class HeroSkills3 // HeroSkills + 类型ID
    {
        public static string shortcuts = "W";//快捷键

        public static float cooling = 8f;//冷却

        public static int consume = 20;//魔法消耗

        public static string ico = "icon/skills/0";//图标

        public static string describe = "攻击回血";//技能描述

        public static string name = "";//技能名字

        public static bool Skills(GameObject go, HeroAttribute heroAttribute)//技能实现（方法）//释放单位，伤害，目标单位
        {
            bool sbL = heroAttribute.MagicConsumption(consume);//魔法消耗
            if (sbL)
            {

            }
            return sbL;
        }
    }

    public class HeroSkills4 // HeroSkills + 类型ID
    {
        public static string shortcuts = "E";//快捷键

        public static float cooling = 5f;//冷却

        public static int consume = 20;//魔法消耗

        public static string ico = "icon/skills/4";//图标

        public static string describe = "强化下一次普通攻击";//技能描述

        public static string name = "";//技能名字

        public static bool Skills(GameObject go, HeroAttribute heroAttribute)//技能实现（方法）//释放单位，伤害，目标单位
        {
            bool sbL = heroAttribute.MagicConsumption(consume);//魔法消耗
            if (sbL)
            {

            }
            return sbL;
        }
    }

    public class HeroSkills5 // HeroSkills + 类型ID
    {
        public static string shortcuts = "R";//快捷键

        public static float cooling = 8f;//冷却

        public static int consume = 20;//魔法消耗

        public static string ico = "icon/skills/0";//图标

        public static string describe = "自身散发火焰，让范围内（256码）敌人突然爆炸，并造成（攻击力）点魔法伤害，爆炸的敌人还会对其范围（128码）内敌人造成（攻击力）点魔法伤害";//技能描述

        public static string name = "";//技能名字

        public static bool Skills(GameObject go, HeroAttribute heroAttribute)//技能实现（方法）//释放单位，伤害，目标单位
        {
            bool sbL = heroAttribute.MagicConsumption(consume);//魔法消耗
            if (sbL)
            {

            }
            return sbL;
        }
    }
}