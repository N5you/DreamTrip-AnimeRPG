using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UserStateType //返回状态获得 //登录状态列表
{
    UserExisting,//用户已存在
    RegisteredSuccessful,//注册成功
    LoginSuccessful,//登陆成功
    AccountError,//密码错误
    OnUser//用户不存在
}

//用于服务器的存储和读取，目前是本地存档，全游戏数据存储都用它，可换为服务器存储
public class ServerStoreRead// : MonoBehaviour
{
    #region 游戏存档方面
    public static string[] GetCourseRoleWearItems()//获取当前角色已穿戴物品
    {
        //存储格式（物品ID|数量|穿戴位置）用x隔开
        string key = ServerStoreRead.GetCurrentRole() + ",WearItems0";//当前选择角色 + backpack0
        string[] str = PlayerPrefs.GetString(key).Split('x'); //当前选择角色 + backpack0
        return str;
    }

    public static void SetCourseRoleWearItems(SpecificItems[] specificItems)//存储当前角色已穿戴物品 SetCourseRoleWearItems
    {
        string vu = "";//存储格式（物品ID|数量|穿戴位置）用x隔开
        if (specificItems[0].items.ID >= 0) vu = specificItems[0].items.ID + "|" + specificItems[0].number + "|" + specificItems[0].wearLocation;
        for (int index = 1; index <= specificItems.Length - 1; index++) //存储格式（物品ID|数量|穿戴位置）用x隔开
        {
            if (specificItems[index].items.ID >= 0)
            {
                if (vu.Length > 0) vu = vu + "x";
                string str = specificItems[index].items.ID + "|" + specificItems[index].number + "|" + specificItems[index].wearLocation;
                vu = vu + str;
            }
        }
        string key = ServerStoreRead.GetCurrentRole() + ",WearItems0";//当前选择角色 + backpack0
        PlayerPrefs.SetString(key, vu);
    }

    public static void SetCourseRoleBackpack(BackpackSpace[] backpackSpace)//存储当前角色背包 //需要服务器DOTO
    {
        string vu = "";
        if (backpackSpace[0].items.items.ID >= 0) vu = backpackSpace[0].items.items.ID + "|" + backpackSpace[0].items.number + "|" + backpackSpace[0].items.wearLocation;
        for (int index = 1; index <= backpackSpace.Length - 1; index++) //存储格式（物品ID|数量|穿戴位置）用x隔开
        {
            if (backpackSpace[index].items.items.ID >= 0)
            {
                if(vu.Length > 0) vu = vu + "x";
                string str = backpackSpace[index].items.items.ID + "|" + backpackSpace[index].items.number + "|" + backpackSpace[index].items.wearLocation;
                vu = vu + str;
            }
        }
        string key = ServerStoreRead.GetCurrentRole() + ",backpack0";//当前选择角色 + backpack0
        PlayerPrefs.SetString(key, vu);
    }

    public static void SetCourseRoleBackpack(string[] backpackSpace)//存储当前角色背包 //需要服务器DOTO
    {
        string vu = "";
        //if (backpackSpace[0].items.items.ID >= 0) vu = backpackSpace[0].items.items.ID + "|" + backpackSpace[0].items.number + "|" + backpackSpace[0].items.wearLocation;
        for (int index = 0; index <= backpackSpace.Length - 1; index++) //存储格式（物品ID|数量|穿戴位置）用x隔开
        {
            if (vu.Length > 0) vu = vu + "x";
            vu = vu + backpackSpace[index];
        }
        string key = ServerStoreRead.GetCurrentRole() + ",backpack0";//当前选择角色 + backpack0
        PlayerPrefs.SetString(key, vu);
    }

    public static string[] GetCourseRoleBackpack()//获取当前角背包
    {
        string key = ServerStoreRead.GetCurrentRole() + ",backpack0";//当前选择角色 + backpack0
        string[] str = PlayerPrefs.GetString(key).Split('x'); //当前选择角色 + backpack0.
        return str;
    }

    //用 玩家 + 存档ID（索引）的方式存储，//实际上是 name + 账号 + 角色名字 + 具体数据

    //角色名字 NameString

    public static bool GetHeroArchiveIfThereIs(int index)//获取该存档（玩家角色）是否存在 //需要服务器 LOTO
    {
        string key = ServerStoreRead.GetLoginUser() + "," + index;//ServerStoreRead.GetCurrentRole() + ",heroAttributeList";

        return PlayerPrefs.HasKey(key);
    }

    public static int currentHeroIndex;//当前玩家存档角色索引

    public static void SetHeroArchiveUpStorage(int index, string vu)//设置当前角色（索引，角色ID）保存角色ID
    {
        string key = ServerStoreRead.GetLoginUser() + "," + index;//玩家存档 + 索引
        PlayerPrefs.SetString(key, vu);//索引，角色ID
    }

    public static string GetCurrentRole()//获取当前角色 //需要服务器 LOTO
    {
        string key = PlayerPrefs.GetString("UserLoginRole");
        return key;//返回内容是：当前账号 + 索引（角色）
    }

    /// <summary>
    /// 获取角色名字
    /// </summary>
    /// <param name="str">索引</param>
    /// <returns></returns>
    public static string GetRoleName(string index)//获取指定（索引）角色名字 //需要服务器 LOTO
    {
        string key = ServerStoreRead.GetLoginUser() + "," + index;
        string stt = PlayerPrefs.GetString(key + ",NameString");//当前账号 + 索引（角色）+ ,NameString
        return stt;
    }

    public static string GetRoleName()//获取当前角色的名字
    {
        string key = ServerStoreRead.GetCurrentRole();
        string stt = PlayerPrefs.GetString(key + ",NameString");//当前账号 + 索引（角色）+ ,NameString
        return stt;
    }

    /// <summary>
    /// 存储当前角色
    /// </summary>
    /// <param name="str">索引</param>
    public static void SetCurrentRole(int index)//存储当前角色 //需要服务器LOTO
    {
        string key = ServerStoreRead.GetLoginUser() + "," + index;//存储内容：当前账号 + 索引（角色）
        PlayerPrefs.SetString("UserLoginRole", key);
    }

    /// <summary>
    /// 存储角色名字
    /// </summary>
    /// <param name="str">角色名字</param>
    public static bool SetRoleName(string str)//存储角色名字 //需要服务器LOTO
    {
        string key = ServerStoreRead.GetCurrentRole() + ",NameString";
        PlayerPrefs.SetString(key,str);
        return PlayerPrefs.HasKey(key);//存储失败或成功
    }

    /// <summary>
    /// 获取指定角色属性
    /// </summary>
    /// <param name="index">索引</param>
    /// <returns>角色属性列表</returns>
    public static HeroAttributeList GetSpecifiedRoleAttribute(string index)//获取指定角色（索引）属性 //需要服务器LOTO
    {
        string key = ServerStoreRead.GetLoginUser() + "," + index + ",heroAttributeList";
        string value = PlayerPrefs.GetString(key);//获取数据，需要服务器 TODO

        HeroAttributeList heroAttributeList = new HeroAttributeList();
        string[] str = value.Split(',');//分割

        //最大生命值|最大魔法值|攻击力|攻击距离|攻击速度|移动速度|护甲|虚拟伤害|穿透力|重生数|暴击率|暴击效果|幸运值|力量|敏捷|智力|职业|觉醒|等级|经验值|伤害类型
        heroAttributeList.hpMaxInt = int.Parse(str[0]);//生命值
        heroAttributeList.mpMaxInt = int.Parse(str[1]);//魔法值
        heroAttributeList.damageValueInt = int.Parse(str[2]);//攻击力
        heroAttributeList.damageRangeFloat = float.Parse(str[3]);//攻击距离

        heroAttributeList.damageSpeedFloat = float.Parse(str[4]);//攻击速度
        heroAttributeList.MovementSpeedFloat = float.Parse(str[5]);//移动速度

        heroAttributeList.armorInt = int.Parse(str[6]);//护甲
        heroAttributeList.virtualDamageInt = int.Parse(str[7]);//虚拟伤害
        heroAttributeList.penetrationInt = int.Parse(str[8]);//穿透力
        heroAttributeList.rebirthValueInt = int.Parse(str[9]);//重生数
        heroAttributeList.critRateInt = int.Parse(str[10]);//暴击率
        heroAttributeList.critEffectInt = int.Parse(str[11]);//暴击效果
        heroAttributeList.luckyValueInt = int.Parse(str[12]);//幸运值
        heroAttributeList.powerInt = int.Parse(str[13]);//力量
        heroAttributeList.agileInt = int.Parse(str[14]);//敏捷
        heroAttributeList.intelligenceInt = int.Parse(str[15]);//智力

        heroAttributeList.professionalString = (HeroProfessionalType)System.Enum.Parse(typeof(HeroProfessionalType), str[16]);//职业
        heroAttributeList.awakeningString = str[17];//觉醒
        heroAttributeList.levelInt = int.Parse(str[18]);//等级
        heroAttributeList.expInt = int.Parse(str[19]);//经验值
        heroAttributeList.damageClassType = (DamageClassType)System.Enum.Parse(typeof(DamageClassType), str[20]);//伤害类型
        heroAttributeList.typeID = int.Parse(str[21]);

        return heroAttributeList;
    }

    public static string GetCurrentRoleAttributeString()//获取当前角色属性string集 //需要服务器LOTO
    {
        string key = ServerStoreRead.GetCurrentRole() + ",heroAttributeList"; //先获取当前面角色，再拼接,heroAttributeList
        string value = PlayerPrefs.GetString(key);//获取数据，需要服务器 TODO
        return value;
    }
    public static HeroAttributeList GetCurrentRoleAttribute()//获取当前角色属性 //需要服务器LOTO
    {
        string key = ServerStoreRead.GetCurrentRole() + ",heroAttributeList"; //先获取当前面角色，再拼接,heroAttributeList
        string value = PlayerPrefs.GetString(key);//获取数据，需要服务器 TODO

        HeroAttributeList heroAttributeList = new HeroAttributeList();
        string[] str = value.Split(',');//分割

        //最大生命值|最大魔法值|攻击力|攻击距离|攻击速度|移动速度|护甲|虚拟伤害|穿透力|重生数|暴击率|暴击效果|幸运值|力量|敏捷|智力|职业|觉醒|等级|经验值|伤害类型
        heroAttributeList.hpMaxInt = int.Parse(str[0]);//生命值
        heroAttributeList.mpMaxInt = int.Parse(str[1]);//魔法值
        heroAttributeList.damageValueInt = int.Parse(str[2]);//攻击力
        heroAttributeList.damageRangeFloat = float.Parse(str[3]);//攻击距离

        heroAttributeList.damageSpeedFloat = float.Parse(str[4]);//攻击速度
        heroAttributeList.MovementSpeedFloat = float.Parse(str[5]);//移动速度

        heroAttributeList.armorInt = int.Parse(str[6]);//护甲
        heroAttributeList.virtualDamageInt = int.Parse(str[7]);//虚拟伤害
        heroAttributeList.penetrationInt = int.Parse(str[8]);//穿透力
        heroAttributeList.rebirthValueInt = int.Parse(str[9]);//重生数
        heroAttributeList.critRateInt = int.Parse(str[10]);//暴击率
        heroAttributeList.critEffectInt = int.Parse(str[11]);//暴击效果
        heroAttributeList.luckyValueInt = int.Parse(str[12]);//幸运值
        heroAttributeList.powerInt = int.Parse(str[13]);//力量
        heroAttributeList.agileInt = int.Parse(str[14]);//敏捷
        heroAttributeList.intelligenceInt = int.Parse(str[15]);//智力

        heroAttributeList.professionalString = (HeroProfessionalType)int.Parse(str[16]);//职业
        heroAttributeList.awakeningString = str[17];//觉醒
        heroAttributeList.levelInt = int.Parse(str[18]);//等级
        heroAttributeList.expInt = int.Parse(str[19]);//经验值
        heroAttributeList.damageClassType = (DamageClassType)int.Parse(str[20]);//伤害类型
        heroAttributeList.typeID = int.Parse(str[21]);

        return heroAttributeList;
    }
    public static bool SetSpecifiedRoleAttribute(string index, HeroAttributeList heroAttributeList)//存储指定索引的角色属性 //需要服务器 LOTO
    {
        string[] str = new string[22];//共21个数据
        //最大生命值|最大魔法值|攻击力|攻击距离|攻击速度|移动速度|护甲|虚拟伤害|穿透力|重生数|暴击率|暴击效果|幸运值|力量|敏捷|智力|职业|觉醒|等级|经验值|伤害类型
        str[0] = heroAttributeList.hpMaxInt.ToString();//生命值
        str[1] = heroAttributeList.mpMaxInt.ToString();//魔法值
        str[2] = heroAttributeList.damageValueInt.ToString();//攻击力
        str[3] = heroAttributeList.damageRangeFloat.ToString();//攻击距离
        str[4] = heroAttributeList.damageSpeedFloat.ToString();//攻击速度
        str[5] = heroAttributeList.MovementSpeedFloat.ToString();//移动速度
        str[6] = heroAttributeList.armorInt.ToString();//护甲
        str[7] = heroAttributeList.virtualDamageInt.ToString();//虚拟伤害
        str[8] = heroAttributeList.penetrationInt.ToString();//穿透力
        str[9] = heroAttributeList.rebirthValueInt.ToString();//重生数
        str[10] = heroAttributeList.critRateInt.ToString();//暴击率
        str[11] = heroAttributeList.critEffectInt.ToString();//暴击效果
        str[12] = heroAttributeList.luckyValueInt.ToString();//幸运值
        str[13] = heroAttributeList.powerInt.ToString();//力量
        str[14] = heroAttributeList.agileInt.ToString();//敏捷
        str[15] = heroAttributeList.intelligenceInt.ToString();//智力
        str[16] = heroAttributeList.professionalString.ToString();//职业
        str[17] = heroAttributeList.awakeningString;//觉醒
        str[18] = heroAttributeList.levelInt.ToString();//等级
        str[19] = heroAttributeList.expInt.ToString();//经验值
        str[20] = heroAttributeList.damageClassType.ToString();//伤害类型
        str[21] = heroAttributeList.typeID.ToString();

        string result = string.Join(",", str);//把字符串拼接

        string key = ServerStoreRead.GetLoginUser() + "," + index + ",heroAttributeList";//键为：玩家（当前）账号 + 角色Index + ,heroAttributeList

        PlayerPrefs.SetString(key, result);//需要服务器 TODO

        return PlayerPrefs.HasKey(key); ;//存储失败或成功
    }
    
    public static bool SetCurrentRoleAttribute(HeroAttributeList heroAttributeList)//存储当前角色属性 //需要服务器TODO
    {
        string[] str = new string[22];//共21个数据
        //最大生命值|最大魔法值|攻击力|攻击距离|攻击速度|移动速度|护甲|虚拟伤害|穿透力|重生数|暴击率|暴击效果|幸运值|力量|敏捷|智力|职业|觉醒|等级|经验值|伤害类型|英雄类型
        str[0] = heroAttributeList.hpMaxInt.ToString();//生命值
        str[1] = heroAttributeList.mpMaxInt.ToString();//魔法值
        str[2] = heroAttributeList.damageValueInt.ToString();//攻击力
        str[3] = heroAttributeList.damageRangeFloat.ToString();//攻击距离
        str[4] = heroAttributeList.damageSpeedFloat.ToString();//攻击速度
        str[5] = heroAttributeList.MovementSpeedFloat.ToString();//移动速度
        str[6] = heroAttributeList.armorInt.ToString();//护甲
        str[7] = heroAttributeList.virtualDamageInt.ToString();//虚拟伤害
        str[8] = heroAttributeList.penetrationInt.ToString();//穿透力
        str[9] = heroAttributeList.rebirthValueInt.ToString();//重生数
        str[10] = heroAttributeList.critRateInt.ToString();//暴击率
        str[11] = heroAttributeList.critEffectInt.ToString();//暴击效果
        str[12] = heroAttributeList.luckyValueInt.ToString();//幸运值
        str[13] = heroAttributeList.powerInt.ToString();//力量
        str[14] = heroAttributeList.agileInt.ToString();//敏捷
        str[15] = heroAttributeList.intelligenceInt.ToString();//智力
        str[16] = heroAttributeList.professionalString.ToString();//职业
        str[17] = heroAttributeList.awakeningString;//觉醒
        str[18] = heroAttributeList.levelInt.ToString();//等级
        str[19] = heroAttributeList.expInt.ToString();//经验值
        str[20] = heroAttributeList.damageClassType.ToString();//伤害类型
        str[21] = heroAttributeList.typeID.ToString();

        string result = string.Join(",", str);//把字符串拼接

        string key = ServerStoreRead.GetCurrentRole() + ",heroAttributeList";//键为：玩家（当前）账号 + 角色Index + ,heroAttributeList

        PlayerPrefs.SetString(key, result);//需要服务器 TODO
        
        return PlayerPrefs.HasKey(key);//存储失败或成功 //需要TODO
    }
   
    #endregion

    #region 登录方面（账号方面）

    //user 用户
    //login 登录
    //registered 注册
    //failure 失败
    //successful 成功
    //existing 已存在
    //repeat 重复
    //error 错误
    //account 密码
    //read 读取
    //no 没有

    /// <summary>
    /// 判断用户名是否已经存在
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <returns>存在返回true不然则反之</returns>
    public static bool UserIsNoExist(string userName)//需要连服务器TODO
    {
        string str = "name" + userName;
        //PlayerPrefs.SetString("name", str);
        //string m_info = PlayerPrefs.GetString("name");
        //Debug.Log(m_info);
        if (PlayerPrefs.HasKey(str))
        {
            //Debug.Log("存在");
            return true;
        }
        //Debug.Log("不存在");
        return false;
    }

    public static string GetLoginUser()//读取用户名
    {
        string str = PlayerPrefs.GetString("UserLogin");
        return str;
    }

    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <param name="account">密码</param>
    /// <returns></returns>
    public static UserStateType UserLogin(string userName,string account)//需要连服务器TODO
    {
        if (UserIsNoExist(userName))//用户名正确
        {
            //Debug.Log(AccountRead(userName));
            if (Md5Sum(account) == AccountRead(userName))
            {
                string str = "name," + userName;
                PlayerPrefs.SetString("UserLogin", str);
                //密码正确
                return UserStateType.LoginSuccessful;
            }
            else
            {
                return UserStateType.AccountError;//密码错误
            }
        }
        else
        {
            return UserStateType.OnUser;//用户不存在
        }
    }

    private static string AccountRead(string userName)//按用户名获取用户密码 //需要连服务器TODO
    {
        //PlayerPrefs.SetString()
        string strha = userName + "account";
        string str = PlayerPrefs.GetString(strha);
        return str;
    }

    /// <summary>
    /// 用户注册
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <param name="account">密码</param>
    /// <returns>登录状态</returns>
    public static UserStateType UserRegistered(string userName, string account)
    {
        if (UserIsNoExist(userName))
        {
            return UserStateType.UserExisting;//用户已存在，无法注册
        }
        else
        {
            string strNama = userName + "account";//
            string str = Md5Sum(account);//先进行MD5加密

            string strUs = "name" + userName;//存储用户名： name + 用户名
            PlayerPrefs.SetString(strUs, str);//让strUs为键，密码为值

            //StorageUserNameAccount(userName, account);//再存储密码
            PlayerPrefs.SetString(strNama, str);//不需要StorageUserNameAccount方法了，直接自己存储

            return UserStateType.RegisteredSuccessful;//注册成功
        }
    }

    /// <summary>
    /// 存储用户名和密码
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <param name="account">密码</param>
    private static void StorageUserNameAccount(string userName, string account) //需要连服务器TODO
    {
        string str = "name," + userName;
        PlayerPrefs.SetString(str, Md5Sum(account));
    }

    public static bool UserIsNoLogin()//判断用户是否登陆 //需要连服务器TODO
    {
        if (PlayerPrefs.HasKey("UserLogin"))
        {
            return true;
        }
        return false;
    }

    #endregion

    #region 加密方面
    public static string Md5Sum(string input)//MD5加密
    {
        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        byte[] hash = md5.ComputeHash(inputBytes);
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("x2"));
        }
        return sb.ToString();
    }
    #endregion

    #region 货币方面
    public static FlowInflationAttribute GetFlowInflationAttribute()//获取当前玩家的货币（不区分存档）
    {
        string key = ServerStoreRead.GetLoginUser() + ",flowInflationAttribute1";//当前账号 + flowInflationAttribute1
        string[] keyV = PlayerPrefs.GetString(key).Split('x');
        FlowInflationAttribute flowInflationAttribute = new FlowInflationAttribute(int.Parse(keyV[0]), int.Parse(keyV[1]), int.Parse(keyV[2]));
        //FlowInflationAttribute flowInflationAttribute = new FlowInflationAttribute(10000, 50000, 60000);
        return flowInflationAttribute;
    }

    public static void SetFlowInflationAttribute(FlowInflationAttribute flowInflationAttribute)
    {
        string key = ServerStoreRead.GetLoginUser() + ",flowInflationAttribute1";//金币x钻石x点卷
        string vlu = flowInflationAttribute.gold + "x" + flowInflationAttribute.diamond + "x" + flowInflationAttribute.ava;
        PlayerPrefs.SetString(key, vlu);
    }

    #endregion

    #region 商城方面

    public static string GetStoreGoods(int index)//获取商城商品 0金币 1钻石 2点卷
    {
        string key = "";//每个空格代表一个物品，用X分割（物品ID x 价格 x 数量）
        if (index == 0) key = "0x15x1 1x15x1";
        return key;
    }

    #endregion
}

public class ServerList //服务器列表类
{
    private int number;//服务器数量

    public List<Server> serverList = new List<Server>(); //服务器集合 //服务器名，服务器

    public int Number
    {
        get { return number; }
        set { number = value; }
    }
    #region
    //public List<Server> ServerLists
    //{
    //    get { return serverList; }
    //    set { serverList = value; }
    //}
    #endregion
}

public class Server //服务器类
{
    private string ip;//服务器IP
    private string name;//服务器名字
    private int number;//服务器的人数

    public string IP
    {
        get { return ip; }
        set { ip = value; }
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public int Number
    {
        get { return number; }
        set { number = value; }
    }
    

    #region 服务器列表方面

    public static ServerList GetServerList()//需要连服务器TODO
    {
        ServerList serverList = new ServerList();
        serverList.Number = 10;
        for (int i = 0; i >= 10; i++)
        {
            Server server = new Server();
            server.Name = "飞向" + i + "服";
            server.Number = Random.Range(0, 2000);
            serverList.serverList.Add(server);
        }
        return serverList;
    }

    #endregion
}