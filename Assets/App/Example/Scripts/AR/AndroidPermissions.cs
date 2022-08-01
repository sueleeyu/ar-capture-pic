using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

namespace FrameworkDesign.Example
{

    public class AndroidPermissions : MonoBehaviour
    {
        private static AndroidPermissions instance;

        private int index;
        private List<string> permissionList = new List<string>();
        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
                Init();
            }                                  
        }
        //��ʼ����Ȩ������Ҫ��������
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public void Init()
        {
            //Ȩ����ӽ��б�
            permissionList.Clear();
            index = 0;
            //permissionList.Add("android.permission.READ_PHONE_STATE");
            permissionList.Add(Permission.ExternalStorageRead);
            permissionList.Add(Permission.ExternalStorageWrite);
           // permissionList.Add(Permission.FineLocation);
           // permissionList.Add(Permission.CoarseLocation);
            StartCheckPermission(0.02f); //��ʼ����
            Logger.Log("Ȩ���������");
        }
        public void StartCheckPermission(float time)
        {
            //Logger.Log("��ʼȨ������");
            if (permissionList.Count > 0)
            {
                Get(permissionList[index], time);
            }
        }

        /// <summary>
        /// �жϲ�����Ȩ��
        /// </summary>
        /// <param name="type">Ȩ����</param>
        /// <param name="time">��ܾ��ӳٶ���ٴ�����</param>
        void Get(string type, float time)
        {
            if (!Permission.HasUserAuthorizedPermission(type))
            {
                Permission.RequestUserPermission(type);
                Logger.Log("���ڻ�ȡ��Ȩ�ޣ�" + type);
                StartCoroutine(Check(type, time));
            }
            else
            {
                Logger.Log("Ȩ���Ѿ���ȡ��" + type);

                if (index < permissionList.Count - 1)
                {
                    index += 1;
                    Get(permissionList[index], time);
                }
            }
        }
        IEnumerator Check(string type, float time)
        {
            yield return new WaitForSeconds(time);
            Get(type, time);
        }

        /**
        //��õķ���������Ϸ�Լ���win������ʾ�����ȷ���Ժ��ڵ����ֻ�Ȩ��������ʾ
        public static bool IsShowPermissionPopWin()
        {
            Logger.Log("PerTest-IsShowPermissionPopWin");
            bool isShowWin = false;
            

            AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            //var pp = new AndroidJavaClass("androidx.core.app.ActivityCompat");
            var pp = new AndroidJavaClass("android.support.v4.app.ActivityCompat");
            //û�е�Ȩ��ֱ���ַ������壬���յ��õĻ���android����
            var pList = new List<string>();
            // pList.Add("android.permission.READ PHONE STATE");
            pList.Add(UnityEngine.Android.Permission.ExternalStorageRead);
            pList.Add(UnityEngine.Android.Permission.ExternalStorageWrite);

            var isShow1 = pp.CallStatic<bool>("shouldShowRequestPermissionRationale", currentActivity,
                pList[0]);
            Logger.Log("isShow1"+ isShow1);
            var isShow2 = pp.CallStatic<bool>("shouldShowRequestPermissionRationale", currentActivity,
                pList[1]);
            Logger.Log("isshow2"+ isShow2);
           // var isShow3 = pp.CallStatic<bool>("shouldShowRequestPermissionRationale", currentActivity,
            //    pList[2]);
            //Logger.Log("isShow3"+ isShow3);
            ///Google��ԭ����:
            //1��û�������Ȩ�ޣ���������ˣ����Է���false;
            //2���������û��ܾ��ˣ������Ҫ��ʾ�û��ˣ����Է���true��
            //3���û�ѡ���˾ܾ����Ҳ�����ʾ������Ҳ��Ҫ�����ˣ�Ҳ��Ҫ��ʾ�û��ˣ����Է���false��114.�Ѿ������ˣ�����Ҫ����Ҳ����Ҫ��ʾ�����Է���false��
            if (isShow1 || isShow2)
            {
                // if (
                //     !UnityEngine.Android.Permission.HasUserAuthorizedPermission(pList[0]) ||
                //     !UnityEngine.Android.Permission.HasUserAuthorizedPermission(pList[1]) ||
                //     !UnityEngine.Android.Permission.HasUserAuthorizedPermission(pList[2])
                // )
                // {
                //     UnityEngine.Android.Permission.RequestUserPermissions(pList.ToArray());
                // }
                Logger.Log("isShowWin" );
                isShowWin = true;
            }
            return isShowWin;

        }
  */
    }
}