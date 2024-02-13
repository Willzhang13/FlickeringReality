using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class StartGame : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    private bool scenefinished = false;
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneDone()) {
            MoveToGame();
        }
     
    }

    void MoveToGame() {
        SceneManager.LoadScene("Main Game");
    }

    bool SceneDone() {
        if(videoPlayer.time > 1 && !videoPlayer.isPlaying) {
            print("scene is finished");
            return true;
        }
        return false;
    }
}
