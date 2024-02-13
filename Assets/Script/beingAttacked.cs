using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class beingAttacked : MonoBehaviour
{
    private EnemyLogic[] enemyList;
    private bool isAttacked = false;

    [SerializeField] private Renderer2DData shaderRender;
    private ScriptableRendererFeature hiddenShader;
    private ScriptableRendererFeature spottedShader;
    private Player player;
    void Start()
    {
        enemyList = FindObjectsOfType<EnemyLogic>();
        hiddenShader = shaderRender.rendererFeatures[0];
        spottedShader = shaderRender.rendererFeatures[1];
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < enemyList.Length; i++)
        {
            if (!enemyList[i].isAttacking)
            {
                isAttacked = false;
            } else
            {
                isAttacked = true;
                break;
            }
        }

        if (isAttacked)
        {
            hiddenShader.SetActive(false);
            spottedShader.SetActive(true);
        }
        else if (!player.amVisible)
        {
            hiddenShader.SetActive(true);
            spottedShader.SetActive(false);
        }
        else
        {
            hiddenShader.SetActive(false);
            hiddenShader.SetActive(false);
        }
    }
}
