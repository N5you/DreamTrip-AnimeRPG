using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsDamageDevice : MonoBehaviour
{
    public int damage;//伤害值
    public DamageClassType damageClassType;//伤害类型
    public GameObject murderer;//伤害单位（伤害发出者）

    public new Collider collider;

    //private List<HeroAttribute> damageTable = new List<HeroAttribute>();

    public ByTheAttacker ByTheAttacker;
    private void OnTriggerEnter(Collider other)//当碰进入时执行
    {
        //Debug.Log("233："+other.gameObject.name);
        HeroAttribute heroAttribute = other.GetComponent<HeroAttribute>();//获得属性组件
        if (heroAttribute == null) return;
        //damageTable.Add(heroAttribute);
        //Debug.Log(other.gameObject.name);
        if (heroAttribute.live)//是否为可攻击
        {
            heroAttribute.HpAcceptDamage(damage, damageClassType, murderer);//给予伤害
        }
    }

    //private void OnTriggerStay(Collider other)//触发持续
    //{
    //    Debug.Log(other.gameObject.name);
    //}

    public void StartDamage(float tiem)//允许开始伤害
    {
        collider.enabled = true;
        //if (damageTable.Count == 0) return;
        //foreach (HeroAttribute h in damageTable)
        //{
        //    if (h.live)
        //    {
        //        h.HpAcceptDamage(damage, damageClassType, murderer);
        //    }
        //}
        Destroy(gameObject, tiem);
    }
}
[System.Serializable] public class ByTheAttacker : UnityEngine.Events.UnityEvent<GameObject> { }