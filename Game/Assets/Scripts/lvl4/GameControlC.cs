using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine;
using DualPantoFramework;
using static SpeechControlC.mapToAudio;

// lvl 4
public class GameControlC : MonoBehaviour
{
    public GameObject enemy;
    //public GameObject playerPrefab;

    public int numberOfEnemies = 2;
    private PantoHandle upperHandle;
    private PantoHandle lowerHandle;
    private Tuple<float, float> spawnRange_x = new Tuple<float, float>(-10f, -4f);
    private Tuple<float, float> spawnRange_y = new Tuple<float, float>(-18f, -15f);
    private GameObject panto;
    private bool gameStarted = false, gameFinished = false;
    private SpeechControlC speech;
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
        speech = GetComponent<SpeechControlC>();
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
        //yield return new WaitForSeconds(speech.PlayClip(INTRO3));
        //upperHandle.Free();
        upperHandle.FreeRotation();
        gameStarted = true;
    }

    private void SetSpawnRange(Tuple<float, float> x, Tuple<float, float> y)
    {
        spawnRange_x = new Tuple<float, float>(-6f, -3f);
        spawnRange_y = new Tuple<float, float>(-8f, -6f);
    }


    public IEnumerator RegisterEnemyDeath()
    {
        if (numberOfEnemies > 0)
        {
            SetSpawnRange(new Tuple<float, float>(-6f, -3f), new Tuple<float, float>(-8f, -6f));
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
        for (int i = 0; i < amountOfEnemies; i++){
            GameObject _enemy = Instantiate(enemy, GenerateRandomSpawnPosition(), Quaternion.Euler(90f,0f,0f));
            if (i == 0){
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
    
