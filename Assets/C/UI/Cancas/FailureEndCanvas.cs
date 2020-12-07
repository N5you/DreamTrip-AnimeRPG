using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FailureEndCanvas : MonoBehaviour
{
    public Button resurrectionButton;
    public Button returnButton;

    public AudioSource audioSource;

    private GameObject copyLoadedBgImage;
    private Image copyLoadedBgImageImage;
    private void Start()
    {
        copyLoadedBgImage = transform.Find("CopyLoadedBgImage").gameObject;
        copyLoadedBgImageImage = copyLoadedBgImage.transform.Find("Image").GetComponent<Image>();
    }

    public void OnResurrectionButton()//点击返回按钮后
    {
        //SceneManager.LoadScene("DefensivePattern05");
        StartCoroutine(loadScene("TownPage05"));
        StartCoroutine(OnUpdate());
        //async.allowSceneActivation = true;//进入场景
        copyLoadedBgImage.SetActive(true);
    }

    private AsyncOperation async;
    IEnumerator loadScene(string str)//异步加载场景
    {
        audioSource.Play();
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