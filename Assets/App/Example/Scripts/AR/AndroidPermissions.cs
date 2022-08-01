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
        //初始化，权限申请要尽可能早
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public void Init()
        {
            //权限添加进列表
            permissionList.Clear();
            index = 0;
            //permissionList.Add("android.permission.READ_PHONE_STATE");
            permissionList.Add(Permission.ExternalStorageRead);
            permissionList.Add(Permission.ExternalStorageWrite);
           // permissionList.Add(Permission.FineLocation);
           // permissionList.Add(Permission.CoarseLocation);
            StartCheckPermission(0.02f); //开始申请
            Logger.Log("权限申请完毕");
        }
        public void StartCheckPermission(float time)
        {
            //Logger.Log("开始权限申请");
            if (permissionList.Count > 0)
            {
                Get(permissionList[index], time);
            }
        }

        /// <summary>
        /// 判断并申请权限
        /// </summary>
        /// <param name="type">权限名</param>
        /// <param name="time">如拒绝延迟多久再次申请</param>
        void Get(string type, float time)
        {
            if (!Permission.HasUserAuthorizedPermission(type))
            {
                Permission.RequestUserPermission(type);
                Logger.Log("正在获取的权限：" + type);
                StartCoroutine(Check(type, time));
            }
            else
            {
                Logger.Log("权限已经获取：" + type);

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
        //最好的方法是用游戏自己的win来做提示，点击确定以后在弹出手机权限申请提示
        public static bool IsShowPermissionPopWin()
        {
            Logger.Log("PerTest-IsShowPermissionPopWin");
            bool isShowWin = false;
            

            AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            //var pp = new AndroidJavaClass("androidx.core.app.ActivityCompat");
            var pp = new AndroidJavaClass("android.support.v4.app.ActivityCompat");
            //没有的权限直接字符串定义，最终调用的还是android方法
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
            ///Google的原意是:
            //1，没有申请过权限，申请就是了，所以返回false;
            //2，申请了用户拒绝了，那你就要提示用户了，所以返回true，
            //3，用户选择了拒绝并且不再提示，那你也不要申请了，也不要提示用户了，所以返回false，114.已经允许了，不需要申请也不需要提示，所以返回false，
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