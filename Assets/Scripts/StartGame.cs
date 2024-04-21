using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    void Update()
    {
        //on space key press load the game scene
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Assets/Scenes/Asteroids.unity");
        }
    }
}
