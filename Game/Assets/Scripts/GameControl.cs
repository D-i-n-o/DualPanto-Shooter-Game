using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine;
using DualPantoFramework;

public class GameControl : MonoBehaviour
{
    private Tuple<float, float> spawnRange_x = new Tuple<float, float>(-11f, 0f);
    private Tuple<float, float> spawnRange_y = new Tuple<float, float>(-16f, -6f);
    private int enemyCount = 0;
    private GameObject enemy;
    private GameObject panto;
    private bool gameStarted = false;
    private bool gameFinished = false;


    void Start()
    {

        //StartCoroutine(WaitFor(3.0f));
        //await SpawnWaveOfEnemies(1);
        panto = GameObject.Find("Panto");
        gameStarted = true;

    }
    void Update()
    {
        //if(CountEnemies() == 0)
        //{
        //    gameFinished = true;
        //}
    }

    void PauseGame()
    {
        Time.timeScale = 0;
    }

    void ContinueGame()
    {
        Time.timeScale = 1;
    }

    IEnumerator WaitFor(float duration)
    {
        yield return new WaitForSeconds(duration);
    }

    int CountEnemies()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    async Task SpawnWaveOfEnemies(byte amountOfEnemies) 
    {
        for(int i = 0; i < amountOfEnemies; i++){
            GameObject _enemy = Instantiate(enemy, GenerateRandomSpawnPosition(), enemy.transform.rotation);
            if(i == 0) {
                await panto.GetComponent<LowerHandle>().SwitchTo(_enemy);
            }
        }
    }

    Vector3 GenerateRandomSpawnPosition()
    {
        float randomPosX = UnityEngine.Random.Range(spawnRange_x.Item1, spawnRange_x.Item2);
        float randomPosY = UnityEngine.Random.Range(spawnRange_y.Item2, spawnRange_y.Item2);
        Vector3 randomPos = new Vector3(randomPosX, randomPosY, -0.1f);
        return randomPos;
    }

    public bool HasGameStarted()
    {
        return gameStarted;
    }

    public bool HasGameFinished()
    {
        return gameFinished;
    }
}   
    
