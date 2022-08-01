using UnityEngine;
using System.Collections;
using System;
using System.IO;
using UnityEngine.UI;

namespace FrameworkDesign.Example
{

    /// <summary>
    /// 截图保存安卓手机相册
    /// </summary>
    public class CaptureScreenshotMgr : MonoBehaviour
    {
        public bool isUi = false;//控制截图内容是否带UI       
        string path = Application.persistentDataPath.Substring(0, Application.persistentDataPath.IndexOf("Android")) + "baidu/";
        private void Update()
        {
        }
        /// <summary>
        /// 保存截屏图片，并且刷新相册 Android
        /// </summary>
        /// <param name="name">若空就按照时间命名</param>
        public void CaptureScreenshot(string fileName)
        {
                   
            string name = fileName + GetCurTime() + ".jpg";

#if UNITY_STANDALONE_WIN      //PC平台
       // 编辑器下
       // string path = Application.persistentDataPath + "/" + _name;       
        string path = Application.dataPath + "/" + _name;
        ScreenCapture.CaptureScreenshot(path, 0);
        Debug.Log("图片保存地址" + path);
 
#elif UNITY_ANDROID     //安卓平台
            //Android版本
            if (isUi)
            {
                StartCoroutine(CutImage1(name));
            }
            else
            {
                StartCoroutine(CutImage(name));
            }
#endif
        }
        //截屏并保存///不带UI；根据相机截图
        IEnumerator CutImage(string name)
        {
            RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 0);//创建一个RenderTexture对象 
            yield return new WaitForEndOfFrame();
            Camera.main.targetTexture = rt;//设置截图相机的targetTexture为render
            Camera.main.Render();//手动开启截图相机的渲染
            RenderTexture.active = rt;//激活RenderTexture

            Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);// 新建一个Texture2D对象

            tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, true);//读像素
            tex.Apply();//保存像素信息

            Camera.main.targetTexture = null;//重置截图相机的targetTexture
            RenderTexture.active = null;//关闭RenderTexture的激活状态
            Destroy(rt);//删除RenderTexture对象

            yield return tex;
            byte[] byt = tex.EncodeToPNG();
           
            SavePic(name, byt);
        }
        private void SavePic(string name, byte[] byt)
        {
            //string path = Application.persistentDataPath.Substring(0, Application.persistentDataPath.IndexOf("Android")) + "baidu/";
            string pathName = path + name;
            Logger.Log("SavePic:" + pathName);
            try
            {
                //安卓平台抛出UnauthorizedAccessException错误
                CreateFileInPersistentData(pathName);
                File.WriteAllBytes(pathName, byt);
               // Logger.Log("WriteAllBytes:" + pathName);
            }
            catch (Exception e)
            {
                Logger.Log("CreateFileInPersistentData-Exception:" + e.ToString());
            }
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.MainActivity");
            AndroidJavaObject jo = jc.CallStatic<AndroidJavaObject>("GetInstance");//MainActivity实例
            string[] paths = new string[1];
            paths[0] = path;
            ScanFile(paths);
        }
        public void CreateFileInPersistentData(string pathName)
        {
            string path = pathName;
            if (File.Exists(path))
            {
                return;//存在该文件
            }
            else
            {
                //文件不存在
                FileStream fs = new FileStream(path, FileMode.Create);
                fs.Close();
            }
        }
        //截图并保存带UI；使用Texture2D截取屏幕内像素
        IEnumerator CutImage1(string name)
        {
            yield return new WaitForEndOfFrame();//等到帧结束
            //图片大小  
            Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);// 新建一个Texture2D对象
            yield return new WaitForEndOfFrame();

            tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, true);//读像素
            tex.Apply();//保存像素信息

            yield return tex;
            byte[] byt = tex.EncodeToPNG();
          
            SavePic(name, byt);
        }
        //刷新图片，显示到相册中
        void ScanFile(string[] path)
        {
            //Logger.Log("ScanFile:" + path[0]);
            using (AndroidJavaClass PlayerActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject playerActivity = PlayerActivity.GetStatic<AndroidJavaObject>("currentActivity");//当前Activity-MainActivity
                using (AndroidJavaObject Conn = new AndroidJavaObject("android.media.MediaScannerConnection", playerActivity, null))
                {
                    //Logger.Log("ScanFile:CallStatic" );
                    Conn.CallStatic("scanFile", playerActivity, path, null, null);//调用MediaScannerConnection的scanFile
                }
            }

        }
        /// <summary>
        /// 获取当前年月日时分秒，如20181001444
        /// </summary>
        /// <returns></returns>
        string GetCurTime()
        {
            return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
                + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
        }



    }

}