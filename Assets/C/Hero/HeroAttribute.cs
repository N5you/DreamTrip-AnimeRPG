using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum HeroProfessionalType //英雄职业列表
{
    战士 = 0,
    法师 = 1,
    射手 = 2,
    刺客 = 3,
    辅助 = 4
}

public enum DamageClassType//伤害类型
{
    魔法伤害 = 0,//技能伤害
    物理伤害 = 1,//普通攻击
    真实伤害 = 2//无视护甲，无视护盾
}

[System.Serializable] //序列化
public class HeroAttributeList//属性列表
{
    public int hpMaxInt;//最大生命值
    public int mpMaxInt;//最大魔法值

    public int damageValueInt;//攻击力

    public float damageRangeFloat;//攻击距离

    public float damageSpeedFloat = 1f;//攻击速度
    public float MovementSpeedFloat = 3.5f;//移动速度

    public int armorInt;//护甲

    public int virtualDamageInt;//虚拟伤害
    public int penetrationInt;//穿透力
    public int rebirthValueInt;//重生数，每增加1重生数：+25虚拟伤害 +1穿透力

    public int critRateInt = 20;//暴击率，默认20%
    public int critEffectInt = 2;//暴击效果，默认200%

    public int luckyValueInt;//幸运值

    //增加1点主属性，增加：1点攻击力
    public int powerInt;//力量，每增加1力量：+25最大生命值
    public int agileInt;//敏捷，每增加1敏捷：+0.5攻击速度
    public int intelligenceInt;//智力,每增加1点智力：+20最大魔法值

    public HeroProfessionalType professionalString;//职业：战士/法师/射手/刺客/辅助
    public string awakeningString = "无";//觉醒

    public int levelInt;//等级，每1级力量：增加（力量/敏捷/智力）等属性
    public int expInt;//经验值，升级条件：经验值 => (100+等级)

    public DamageClassType damageClassType = DamageClassType.物理伤害;

    public int typeID;//类型ID
    private HeroAttributeList heroAttributeList;

    public HeroAttributeList(HeroAttributeList heroAttributeList)
    {
        this.heroAttributeList = heroAttributeList;
    }
    public HeroAttributeList()
    {
    }
    public static string ProfessionalIDTurnString(int key)
    {
        string str = "无";
        if (key == 0)
        {
            str = "战士";
        }
        else if (key == 1)
        {
            str = "法师";
        }
        else if (key == 2)
        {
            str = "射手";
        }
        else if (key == 3)
        {
            str = "刺客";
        }
        else if (key == 4)
        {
            str = "辅助";
        }
        return str;
    }
}
public class HeroAttribute : MonoBehaviour //英雄属性
{
    public bool isPlayer = false;//确定是不是玩家，默认不是
    public HeroAttributeList AttributeList = new HeroAttributeList();//永久属性

    //临时属性：游戏结束即无，不做保存
    public int temporaryShieldInt;//伤害护盾
    public int hatredValueInt;//仇恨值

    private MouseControlledMovement mouseControlledMovement;
    private AIControlledMovement aIControlledMovement;

    #region 实时属性：用于实时计算
    private int hpInt { set; get; }//生命值
    private int mpInt { set; get; }//魔法值

    public bool live = true;//确定目标是不是活的，默认活的
    #endregion

    //显示UI
    private Image hpImage { set; get; }//血条
    private Image mpImage { set; get; }//魔条
    private Text lvText { set; get; }//等级

    private void OnStart()//通用初始化
    {
        hpInt = AttributeList.hpMaxInt;
        mpInt = AttributeList.mpMaxInt;
    }

    public void PlayerStart(MouseControlledMovement Mouse)//玩家初始化
    {
        OnStart();
        isPlayer = true;

        mouseControlledMovement = Mouse;
        mouseControlledMovement.SynchronousHeroInformation();

        hpImage = GameObject.Find("Canvas/HeadPortraitBgImage/HPImage").GetComponent<Image>();
        mpImage = GameObject.Find("Canvas/HeadPortraitBgImage/MPImage").GetComponent<Image>();
    }

    public void AIStart(AIControlledMovement Mouse)//电脑初始化
    {
        OnStart();
        isPlayer = false;

        //hpImage = GameObject.Find("Canvas/HeadPortraitBgImage/HPImage").GetComponent<Image>();
        aIControlledMovement = Mouse;
        aIControlledMovement.SynchronousHeroInformation();
        hpImage = aIControlledMovement.hpSlotCanvas.Find("BgImage/Image").GetComponent<Image>();
    }

    public void AIStart(Transform trafm)//电脑初始化
    {
        OnStart();
        isPlayer = false;
        //hpImage = GameObject.Find("Canvas/HeadPortraitBgImage/HPImage").GetComponent<Image>();
        //aIControlledMovement = Mouse;
        //aIControlledMovement.SynchronousHeroInformation();
        hpImage = trafm.Find("BgImage/Image").GetComponent<Image>();
    }

    /// <summary>
    /// 判断攻击距离是否足够
    /// </summary>
    /// <param name="iVector3">我的距离</param>
    /// <param name="enemyVector3">敌人距离</param>
    /// <returns>返回bool类型</returns>
    public bool GetRangeOrNot(Vector3 iVector3, Vector3 enemyVector3)//判断攻击距离是否足够
    {
        return Vector3.Distance(iVector3, enemyVector3) <= AttributeList.damageRangeFloat;
    }

    #region 属性更改
    public bool HpAcceptDamage(int damage, DamageClassType damageType)//接受伤害
    {
        if (damageType != DamageClassType.真实伤害 && temporaryShieldInt > damage)//伤害护盾抵消全部伤害，但不能抵消真实伤害
        {
            temporaryShieldInt -= damage;
            if (temporaryShieldInt <= 0f) temporaryShieldInt = 0;
            return false;
        }

        damage = damage - AttributeList.armorInt;
        if (damage <= 0) damage = 1;

        hpInt -= damage;

        if (hpImage != null)
        {
            float hpL = hpInt;
            float hpMaxL = AttributeList.hpMaxInt;
            //decimal fa = hpInt / AttributeList.hpMaxInt;
            hpImage.fillAmount = hpL / hpMaxL;
        }

        return hpInt <= 0;//死了没
    }

    public bool HpAcceptDamage(int damage, DamageClassType damageType, GameObject murderer)//伤害值/伤害类型/凶手
    {
        bool diedWithout = HpAcceptDamage(damage, damageType);
        return diedWithout;
    }

    public bool ExperienceObtain(int expI)//经验值获取
    {
        AttributeList.expInt += expI;//经验值累加

        int expMaxI = AttributeList.levelInt + 100;//升级条件：经验值 => (100 + 等级)

        bool conditionsl = AttributeList.expInt >= expMaxI;
        if (conditionsl)
        {
            AttributeList.expInt -= expMaxI;
            AttributeList.levelInt += 1;//等级累加
        }

        return conditionsl;
    }

    public bool MagicConsumption(int consumption) //魔法消耗
    {
        bool bL = mpInt >= consumption;
        if (bL)
        {
            mpInt -= consumption;

            if (mpImage != null)
            {
                float mpL = mpInt;
                float mpMaxL = AttributeList.mpMaxInt;
                //decimal fa = hpInt / AttributeList.hpMaxInt;
                mpImage.fillAmount = mpL / mpMaxL;
            }
        }
        return bL;
    }

    #endregion 

    public DeathEvent deathEvent;//死亡事件
    public void Death()//执行这条，开始死亡
    {
        live = false;//让目标不能被攻击

        if (isPlayer)
        {
            hpImage.gameObject.transform.parent.parent.gameObject.SetActive(false);
        }

        deathEvent.Invoke(gameObject);//调用死亡事件
    }

}
[System.Serializable]
public class DeathEvent : UnityEvent<GameObject>{}