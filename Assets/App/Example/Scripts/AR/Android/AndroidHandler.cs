using UnityEngine;
using UnityEngine.UI;

namespace UnityEngine.XR.ARFoundation.Samples
{
    public class AndroidHandler : MonoBehaviour
    {
        //private AndroidJavaObject javaObject;//MainActivity对象

        // [SerializeField]
        //private Text messageText;//unity场景显示Text
        //[SerializeField]
        // private Text receiveMessageText;//unity场景显示Text

        // Start is called before the first frame update
        void Start()
        {
            //第二种
            //AndroidJavaClass android = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            //javaObject = android.GetStatic<AndroidJavaObject>("currentActivity");
        }
        public void OnClickToJavaFunSum()
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.MainActivity");
            AndroidJavaObject jo = jc.CallStatic<AndroidJavaObject>("GetInstance");
            string text = jo.Call<int>("Sum", 5, 3).ToString();
        }


        //unity事件触发，com.unity3d.player.MainActivity为单例类，实现OnUnityFinished接口。
        //OnUnityMethod方法通过native方式获取到MainActivity对象，调用调用android端的OnUnityFinished方法，实现参数传递
        public void OnUnityMethod(string msg)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.MainActivity");
            AndroidJavaObject jo = jc.CallStatic<AndroidJavaObject>("GetInstance");
            string text = jo.Call<int>("OnUnityFinished", msg).ToString();
        }

        //接收来自android的参数
        /**android端调用方法
        String receiveObj = "MainCamera";//unity中脚本挂载的Object的name
        String receiveMethod = "Receive";//unity中Object挂载脚本的方法名
        String params = str + " Android Call Unity.";//方法要穿的参数
        //android调用unity，
        UnityPlayer.UnitySendMessage(receiveObj, receiveMethod, params);
        */
        public void Receive(string str)
        {
            //receiveMessageText.text = str;//str为从android端传过来的数据
        }

        /**
         * 
        public void OnClickCall1()//android调用unity的启动示例，unity调用android的CallUnityFun方法，通过UnityPlayer.UnitySendMessage向unity发送消息
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.MainActivity");//MainActivity继承UnityPlayerActivity
            AndroidJavaObject jo = jc.CallStatic<AndroidJavaObject>("GetInstance");//GetInstance方法获取的当前对象
            jo.Call("CallUnityFun", "Test2");//调用android端方法CallUnityFun
        }
        public void OnClickCall2()
        {
            // javaObject.Call("CallUnityFun", "Test1");//调用android端方法CallUnityFun
        }

        */
    }
}