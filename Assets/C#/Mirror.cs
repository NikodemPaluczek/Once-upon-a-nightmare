using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Mirror : MonoBehaviour
{
    [SerializeField] private Camera mirrorCamera;
    [SerializeField] private Renderer asleepRenderer;
    [SerializeField] private Renderer awakeRenderer;

    private void Awake()
    {
      //  var data = mirrorCamera.GetUniversalAdditionalCameraData();
     //   data.rende
    }
    private void ToggleLayers(bool isSleeping)
    {
      //  mirrorCamera.Rendering = isSleeping ? asleepRenderer : awakeRenderer;
    }
}
