using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine;
using DualPantoFramework;
using static SpeechControlD.mapToAudio;

// lvl 4
public class GameControlD : MonoBehaviour
{
    public GameObject enemy;
    //public GameObject playerPrefab;
    public int numberOfEnemies = 10;
    private PantoHandle upperHandle;
    private PantoHandle lowerHandle;
    private Tuple<float, float> spawnRange_x = new Tuple<float, float>(-10f, -4f);
    private Tuple<float, float> spawnRange_y = new Tuple<float, float>(-18f, -15f);
    private GameObject panto;
    private bool gameStarted = false, gameFinished = false;
    private SpeechControlD speech;
    private GameObject player;
    private float enemySpeed = 0.2f;

    private void Awake()
    {
        panto = GameObject.Find("Panto");
        player = GameObject.Find("Player");
        upperHandle = panto.GetComponent<UpperHandle>();
        lowerHandle = panto.GetComponent<LowerHandle>();
    }

     void Start()
    {
        speech = GetComponent<SpeechControlD>();
        StartCoroutine(StartGame()); 
    }

    private void Update()
    {
        
    }

    // play intro by syncing audio files and handle movement
    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(speech.PlayClip(STARTJINGLE));
        Task movePlayer = upperHandle.MoveToPosition(player.transform.position, 2f, false);
        yield return new WaitForSeconds(speech.PlayClip(INTRO1) + 0.1f);
        yield return new WaitUntil(() => movePlayer.IsCompleted);

        Task spawnEnemies = SpawnWaveOfEnemies(1);
        yield return new WaitForSeconds(speech.PlayClip(INTRO2));
        yield return new WaitUntil(() => spawnEnemies.IsCompleted);
        //yield return new WaitForSeconds(speech.PlayClip(INTRO3));
        upperHandle.Free();
        gameStarted = true;
    }

    private void SetSouthSpawn()
    {
        SetSpawnRange(new Tuple<float, float>(-10f, -4f), new Tuple<float, float>(-18f, -15f));
    }

    private void SetNorthSpawn()
    {
        SetSpawnRange(new Tuple<float, float>(-6f, -3f), new Tuple<float, float>(-8f, -6f));
    }

    private void SetSpawnRange(Tuple<float, float> x, Tuple<float, float> y)
    {
        spawnRange_x = x;
        spawnRange_y = y;
    }


    public IEnumerator RegisterEnemyDeath()
    {
        if (numberOfEnemies > 0)
        {
            if (numberOfEnemies % 2 == 1) SetSouthSpawn(); else SetNorthSpawn();
            Task spawnEnemy = SpawnWaveOfEnemies(1);
            yield return new WaitUntil(() => spawnEnemy.IsCompleted);
        }
        else
        {
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

    int CountEnemies()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    async Task SpawnWaveOfEnemies(int amountOfEnemies)
    {
        numberOfEnemies -= amountOfEnemies;
        for (int i = 0; i < amountOfEnemies; i++)
        {
            GameObject _enemy = Instantiate(enemy, GenerateRandomSpawnPosition(), Quaternion.Euler(90f, 0f, 0f));
            _enemy.GetComponent<EnemyD>().speed = enemySpeed;
            enemySpeed = Math.Min(2f, enemySpeed + 0.2f);
            if (i == 0)
            {
                await lowerHandle.SwitchTo(_enemy, 5f);
            }
        }
    }

    Vector3 GenerateRandomSpawnPosition()
    {
        float randomPosX = UnityEngine.Random.Range(spawnRange_x.Item1, spawnRange_x.Item2);
        float randomPosY = UnityEngine.Random.Range(spawnRange_y.Item2, spawnRange_y.Item2);
        Vector3 randomPos = new Vector3(randomPosX, 0f, randomPosY);
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
    
