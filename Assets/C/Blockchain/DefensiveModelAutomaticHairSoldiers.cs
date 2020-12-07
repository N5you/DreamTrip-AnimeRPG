using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveModelAutomaticHairSoldiers : MonoBehaviour
{
    private GameObject[] littleMonsterList;//小怪列表
    private GameObject[] bossList;//BOSS列表

    public float everyTimeInterval;//每波间隔
    public int waveNumberMax;//最大波数
    public int everytimeMonsterNumber;//每次怪物数量

    private int waveNumber = 0;//当前波数
    private Vector3 presetLocation;//预设位置

    public GameObject attackTarget;//攻击目标
    //private void Start()
    //{
    //    presetLocation = transform.position;

    //    StartCoroutine(HairSoldiers());//开始刷
    //    //GameObject.Find("PlayerObject").GetComponent<MouseControlledMovement>().initialize.AddListener(OnStary);

    //    littleMonsterList = Resources.LoadAll<GameObject>("preset/creeps/DefensivePattern05");
    //    bossList = Resources.LoadAll<GameObject>("preset/creeps/DefensivePattern05/boss");
    //}

    //IEnumerator HairSoldiers()//刷兵
    //{
    //    yield return new WaitForSeconds(everyTimeInterval);

    //    System.Random ra = new System.Random();
    //    for (int i = 0; i <= everytimeMonsterNumber; i++)
    //    {
    //        int n = ra.Next(0, littleMonsterList.Length - 1);
    //        GameObject go = littleMonsterList[n];
    //        float f = (float)0.5 * i;
    //        StartCoroutine(HairSoldiersFor(f, go));
    //    }

    //    waveNumber++;
    //    if (waveNumber <= waveNumberMax)
    //    {
    //        StartCoroutine(HairSoldiers());//继续刷
    //    }
    //}

    //IEnumerator HairSoldiersFor(float f, GameObject go)//创建小怪
    //{
    //    yield return new WaitForSeconds(f);//等待后执行下面代码
    //    GameObject ag = Instantiate(go, presetLocation, new Quaternion(0, 0, 0, 0));

    //    ag.GetComponent<AIControlledMovement>().SetAttackTarget(attackTarget);//设置攻击目标
    //}
}