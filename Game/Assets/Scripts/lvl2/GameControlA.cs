using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine;
using DualPantoFramework;
using static SpeechControlA.mapToAudio;

public class GameControlA : MonoBehaviour
{
    public GameObject enemy;
    //public GameObject playerPrefab;

    private int enemyCount;
    private PantoHandle upperHandle;
    private PantoHandle lowerHandle;
    private Tuple<float, float> spawnRange_x = new Tuple<float, float>(-10f, -4f);
    private Tuple<float, float> spawnRange_y = new Tuple<float, float>(-18f, -15f);
    private GameObject panto;
    private bool gameStarted = false, gameFinished = false;
    private SpeechControlA speech;
    private GameObject player;

    private void Awake()
    {
        panto = GameObject.Find("Panto");
        player = GameObject.Find("Player");
        upperHandle = panto.GetComponent<UpperHandle>();
        lowerHandle = panto.GetComponent<LowerHandle>();
    }

     void Start()
    {
        speech = GetComponent<SpeechControlA>();
        StartCoroutine(StartGame()); 
    }

    private void Update()
    {

        
    }

    // play intro by syncing audio files and handle movement
    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(speech.PlayClip(STARTJINGLE));
        Task movePlayer = upperHandle.MoveToPosition(player.transform.position, 2f);
        yield return new WaitForSeconds(speech.PlayClip(INTRO1) + 0.1f);
        yield return new WaitUntil(() => movePlayer.IsCompleted);

        Task spawnEnemies = SpawnWaveOfEnemies(1);
        yield return new WaitForSeconds(speech.PlayClip(INTRO2));
        yield return new WaitUntil(() => spawnEnemies.IsCompleted);
        yield return new WaitForSeconds(speech.PlayClip(INTRO3));
        //upperHandle.Free();
        upperHandle.FreeRotation();
        gameStarted = true;
    }



    public void RegisterEnemyDeath()
    {
        lowerHandle.Rotate(360f);
        if (--enemyCount <= 0) {
            EndGame();
        }
    }

    private void EndGame()
    {
        StartCoroutine(speech.PlayEndClips());
        gameFinished = true;
        Destroy(player, 2f);
        upperHandle.Free(); 
        upperHandle.Rotate(90);
        lowerHandle.Free();
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

    async Task SpawnWaveOfEnemies(int amountOfEnemies)
    {
        enemyCount += amountOfEnemies;
        for (int i = 0; i < amountOfEnemies; i++){
            GameObject _enemy = Instantiate(enemy, GenerateRandomSpawnPosition(), Quaternion.Euler(90f,0f,0f));
            if (i == 0){
                await lowerHandle.MoveToPosition(_enemy.transform.position, 5f, true);
            }
        }
    }

    Vector3 GenerateRandomSpawnPosition()
    {
        float randomPosX = UnityEngine.Random.Range(spawnRange_x.Item1, spawnRange_x.Item2);
        float randomPosY = UnityEngine.Random.Range(spawnRange_y.Item2, spawnRange_y.Item2);
        Vector3 randomPos = new Vector3(randomPosX, 0.0f, randomPosY);
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
    
