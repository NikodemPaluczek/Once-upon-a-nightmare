using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoController : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;

    [SerializeField] private GameObject button;
    [SerializeField] private GameObject background;
    

    private void Awake()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    private void OnDestroy()
    {
        videoPlayer.loopPointReached -= OnVideoFinished;
    }

    public void PlayVideo()
    {
        videoPlayer.Stop();
        videoPlayer.Play();
        Destroy(button);
        Destroy(background);

    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(1);
    }
}