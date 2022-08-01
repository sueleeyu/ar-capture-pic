package com.unity3d.player;
import android.os.Bundle;
import android.view.KeyEvent;
import android.widget.Toast;

public class MainActivity extends UnityPlayerActivity{

    private static MainActivity instance;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        instance = this;
    }

    public static MainActivity GetInstance()
    {
        return instance;
    }

    @Override public boolean dispatchKeyEvent(KeyEvent event)
    {
        if(event.getKeyCode() == KeyEvent.KEYCODE_BACK){
            onBackPressed();
            return true;
        }

        if (event.getAction() == KeyEvent.ACTION_MULTIPLE)
            return mUnityPlayer.injectEvent(event);
        return super.dispatchKeyEvent(event);
    }

    //------
    @Override
    public void onBackPressed() {
        runOnUiThread(new Runnable() {
            @Override
            public void run() {
                mUnityPlayer.quit();
            }
        });
        super.onBackPressed();
    }

    //unity调用android的方法
    public int Sum(int x, int y)
    {
        return x + y;
    }
    public String OnUnityFinished(String str){
        runOnUiThread(new Runnable() {
            @Override
            public void run() {
                Toast.makeText(
                        instance,
                        "OnUnityFinished=" + str,
                        Toast.LENGTH_LONG).show();
            }
        });
        return "ok";
    }
    public void CallUnityFun(String str)
    {
        String receiveObj = "MainCamera";//unity中脚本挂载的Object的name
        String receiveMethod = "Receive";//unity中Object挂载脚本的方法名
        String params = str + " Android Call Unity.";//方法要穿的参数
        //android调用unity，
        UnityPlayer.UnitySendMessage(receiveObj, receiveMethod, params);
    }
}
