using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPicket : MonoBehaviour
{
    public AIControlledMovement aIControlledMovement;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "player") return;//不是玩家就跳过
        aIControlledMovement.SetAttackTarget(other.gameObject);//命令AI去攻击这个物体
    }
}