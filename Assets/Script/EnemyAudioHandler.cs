using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioHandler : MonoBehaviour
{
    public EnemyLogic enemyLogic;
    public AudioClip groaningNoise;
    public int wait = 10;
    bool playing = true; // can manipulate maybe based on whether the player is in range or not? 
    // Start is called before the first frame update
    void Start()
    {
        enemyLogic = GetComponent<EnemyLogic>();
        StartCoroutine(SoundOut());
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale == 0) {
            GetComponent<AudioSource>().Pause();
        } else {
            GetComponent<AudioSource>().UnPause();
        }
    }

    IEnumerator SoundOut() {
        while(playing && !enemyLogic.isAttacking) {
            GetComponent<AudioSource>().PlayOneShot(groaningNoise);
            
            yield return new WaitForSeconds(wait);
        }
    }
}
