using UnityEngine;
using System.Collections;
using System;
using System.IO;
using UnityEngine.UI;

namespace FrameworkDesign.Example
{

    /// <summary>
    /// ��ͼ���氲׿�ֻ����
    /// </summary>
    public class CaptureScreenshotMgr : MonoBehaviour
    {
        public bool isUi = false;//���ƽ�ͼ�����Ƿ��UI       
        string path = Application.persistentDataPath.Substring(0, Application.persistentDataPath.IndexOf("Android")) + "baidu/";
        private void Update()
        {
        }
        /// <summary>
        /// �������ͼƬ������ˢ����� Android
        /// </summary>
        /// <param name="name">���վͰ���ʱ������</param>
        public void CaptureScreenshot(string fileName)
        {
                   
            string name = fileName + GetCurTime() + ".jpg";

#if UNITY_STANDALONE_WIN      //PCƽ̨
       // �༭����
       // string path = Application.persistentDataPath + "/" + _name;       
        string path = Application.dataPath + "/" + _name;
        ScreenCapture.CaptureScreenshot(path, 0);
        Debug.Log("ͼƬ�����ַ" + path);
 
#elif UNITY_ANDROID     //��׿ƽ̨
            //Android�汾
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
        //����������///����UI�����������ͼ
        IEnumerator CutImage(string name)
        {
            RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 0);//����һ��RenderTexture���� 
            yield return new WaitForEndOfFrame();
            Camera.main.targetTexture = rt;//���ý�ͼ�����targetTextureΪrender
            Camera.main.Render();//�ֶ�������ͼ�������Ⱦ
            RenderTexture.active = rt;//����RenderTexture

            Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);// �½�һ��Texture2D����

            tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, true);//������
            tex.Apply();//����������Ϣ

            Camera.main.targetTexture = null;//���ý�ͼ�����targetTexture
            RenderTexture.active = null;//�ر�RenderTexture�ļ���״̬
            Destroy(rt);//ɾ��RenderTexture����

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
                //��׿ƽ̨�׳�UnauthorizedAccessException����
                CreateFileInPersistentData(pathName);
                File.WriteAllBytes(pathName, byt);
               // Logger.Log("WriteAllBytes:" + pathName);
            }
            catch (Exception e)
            {
                Logger.Log("CreateFileInPersistentData-Exception:" + e.ToString());
            }
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.MainActivity");
            AndroidJavaObject jo = jc.CallStatic<AndroidJavaObject>("GetInstance");//MainActivityʵ��
            string[] paths = new string[1];
            paths[0] = path;
            ScanFile(paths);
        }
        public void CreateFileInPersistentData(string pathName)
        {
            string path = pathName;
            if (File.Exists(path))
            {
                return;//���ڸ��ļ�
            }
            else
            {
                //�ļ�������
                FileStream fs = new FileStream(path, FileMode.Create);
                fs.Close();
            }
        }
        //��ͼ�������UI��ʹ��Texture2D��ȡ��Ļ������
        IEnumerator CutImage1(string name)
        {
            yield return new WaitForEndOfFrame();//�ȵ�֡����
            //ͼƬ��С  
            Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);// �½�һ��Texture2D����
            yield return new WaitForEndOfFrame();

            tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, true);//������
            tex.Apply();//����������Ϣ

            yield return tex;
            byte[] byt = tex.EncodeToPNG();
          
            SavePic(name, byt);
        }
        //ˢ��ͼƬ����ʾ�������
        void ScanFile(string[] path)
        {
            //Logger.Log("ScanFile:" + path[0]);
            using (AndroidJavaClass PlayerActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject playerActivity = PlayerActivity.GetStatic<AndroidJavaObject>("currentActivity");//��ǰActivity-MainActivity
                using (AndroidJavaObject Conn = new AndroidJavaObject("android.media.MediaScannerConnection", playerActivity, null))
                {
                    //Logger.Log("ScanFile:CallStatic" );
                    Conn.CallStatic("scanFile", playerActivity, path, null, null);//����MediaScannerConnection��scanFile
                }
            }

        }
        /// <summary>
        /// ��ȡ��ǰ������ʱ���룬��20181001444
        /// </summary>
        /// <returns></returns>
        string GetCurTime()
        {
            return DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
                + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
        }



    }

}