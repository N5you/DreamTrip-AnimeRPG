using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TownPage05Cancas : MonoBehaviour
{
    public GameObject TranscriptBgImage;

    private GameObject learningSkillsBgImage;//学习技能面板
    private void Start()
    {
        //copyLoadedBgImage = transform.Find("CopyLoadedBgImage").gameObject;
        //copyLoadedBgImageImage = copyLoadedBgImage.transform.Find("Image").GetComponent<Image>();
        learningSkillsBgImage = transform.Find("LearningSkillsBgImage").gameObject;
    }

    public void OnOpenLearningSkills()//打开学习技能面板
    {
        learningSkillsBgImage.SetActive(true);
    }

    public void OnClickTranscript()//打开发布列表
    {
        TranscriptBgImage.SetActive(true);
    }

    public void OnCloseTranscript()//关闭副本列表
    {
        TranscriptBgImage.SetActive(false);
        NpcEvent.whetherItIsClicked = true;
    }

    private GameObject copyLoadedBgImage;//异步面板（场景加载）
    private Image copyLoadedBgImageImage;//异步进度条

    //private IEnumerable onUpdateCoroutine;
    public void OnEnterDefensiveTranscript()//进入防守副本
    {
        copyLoadedBgImage = Instantiate(Resources.Load<GameObject>("preset/UI/global/CopyLoadedBgImage"));//preset/UI/global/CopyLoadedBgImage
        copyLoadedBgImageImage = copyLoadedBgImage.transform.Find("Image").GetComponent<Image>();
        copyLoadedBgImage.transform.Find("PlotText").GetComponent<Text>().text = "十魔号群魔以乱天下，副本使者怕不敌，号天下志士协助，以御群魔";

        //copyLoadedBgImage.transform.SetParent(transform);
        RectTransform rectT = copyLoadedBgImage.GetComponent<RectTransform>();
        rectT.SetParent(transform);
        rectT.localScale = new Vector3(1, 1, 1);
        //rectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
        //rectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
        rectT.sizeDelta = new Vector2(0, 0);
        rectT.localPosition = new Vector3(0, 0, 0);
        
        copyLoadedBgImage.SetActive(true);
        //异步加载场景
        StartCoroutine(loadScene("DefensivePattern05"));
        StartCoroutine(OnUpdate());
        //async.allowSceneActivation = true;//进入场景
    }

    private AsyncOperation async;
    IEnumerator loadScene(string str)
    {
        //copyLoadedBgImageImage.fillAmount = async.progress;
        //异步读取场景。
        //Globe.loadName 就是A场景中需要读取的C场景名称。
        async = SceneManager.LoadSceneAsync(str);
        async.allowSceneActivation = false;
        //读取完毕后返回， 系统会自动进入C场景
        yield return async;
        
    }

    private uint _nowprocess;
    IEnumerator OnUpdate()//进度条
    {
        uint toProcess;

        if (async.progress < 0.9f)//坑爹的progress，最多到0.9f
        {
            toProcess = (uint)(async.progress * 100);
        }
        else
        {
            toProcess = 100;
        }

        if (_nowprocess < toProcess)
        {
            _nowprocess++;
        }

        copyLoadedBgImageImage.fillAmount = toProcess / 100f;

        if (toProcess == 100)//async.isDone应该是在场景被激活时才为true //_nowprocess
        {
            async.allowSceneActivation = true;
        }
        yield return 1;

        StartCoroutine(OnUpdate());
    }
}