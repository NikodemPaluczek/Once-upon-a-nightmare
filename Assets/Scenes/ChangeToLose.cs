using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeToLose : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(3);
        }
    }
}
