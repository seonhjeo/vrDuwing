using Agora_RTC_Plugin.API_Example.Examples.Advanced.CustomCaptureVideo;
using Agora_RTC_Plugin.API_Example.Examples.Advanced.JoinChannelVideoToken;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgoraManager : MonoBehaviour
{
    public static AgoraManager Instance;

    public JoinChannelVideoToken JoinChannelVideoToken;
    public CustomCaptureVideo CustomCaptureVideo;

    public GameObject screenPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Click_Btn_JoinAgoraWebCam()
    {
        JoinChannelVideoToken.JoinChannel();
    }

    public void Click_Btn_JoinAgoraGameCam()
    {
        CustomCaptureVideo.JoinChannel();
    }

    public void Click_Btn_Dis()
    {
        CustomCaptureVideo.StopCam();
    }
}
