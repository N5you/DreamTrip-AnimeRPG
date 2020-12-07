using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipPlateBgImage : MonoBehaviour
{
    private Text nameText;
    private Text instructionsText;
    private void Awake()
    {
        nameText = transform.Find("NameText").GetComponent<Text>();
        instructionsText = transform.Find("InstructionsText").GetComponent<Text>();
    }
    public void UpdatePromptContent(string name, string instructions)//更新提示内容
    {
        nameText.text = name;
        instructionsText.text = instructions;
    }
    public void OnShutDown()//关闭按钮
    {
        Destroy(gameObject);
    }
}