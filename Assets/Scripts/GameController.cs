using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;

    const float WIDTH = 3.7f;
    const float HEIGHT = 7f;


    public float snakeSpeed = 1;

    public BodyPart bodyPrefab = null;
    public GameObject rockPrefab = null;
    public GameObject eggPrefab = null;
    public GameObject goldEggPrefab = null;
    public GameObject spikePrefab = null;

    public Sprite tailSprite = null;
    public Sprite bodySprite = null;

    public SnakeHead snakeHead = null;

    public bool alive = true;

    public bool waitingToPlay = true;

    List<Egg> eggs = new List<Egg>();
    List<Spike> spikes = new List<Spike>();

    int level = 0;
    int noOfEggsForNextLevel = 0;
    int noOfSpike = 0;

    public int score = 0;
    public int hiScore = 0;

    public Text scoreText = null;
    public Text hiScoreText = null;


    public Text tapToPlayText = null;
    public Text gameOverText = null;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        // Debug.Log("Starting Snake Game");
        
        CreateWalls();
        
        alive = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (waitingToPlay)
        {
            foreach(Touch touch in Input.touches)  // detecting touch to start game
            {
                if (touch.phase == TouchPhase.Ended)
                {
                    StartGamePlay();
                }
            }

            if (Input.GetMouseButtonUp(0)) // detecting mouse input to start game play
            {
                StartGamePlay();
            }
        }
    }

  

    public void GameOver()
    {
        alive = false;
        waitingToPlay = true;

        gameOverText.gameObject.SetActive(true);
        tapToPlayText.gameObject.SetActive(true);

        
    }

    public void StartGamePlay()
    {
        score = 0;
        level = 0;
        noOfSpike = 0;

        scoreText.text = "Score = " + score;
        hiScoreText.text = "HiScore = " + hiScore;

        gameOverText.gameObject.SetActive(false);
        tapToPlayText.gameObject.SetActive(false);

        waitingToPlay = false;
        alive = true;

        KillOldEggs();
        KillOldSpikes();

        LevelUp();
    }


    void LevelUp()
    {
        // required features - increase speed, no of eggs, reset snake, and increase level
        level++;

        noOfEggsForNextLevel = 4 + (level * 2); // no of eggs required for next level

        noOfSpike += 1;


        snakeSpeed = 1f + (level/4f);

        // limiting maximum speed
        if (snakeSpeed > 6)
            snakeSpeed = 6;

        snakeHead.ResetSnake();
        CreateEgg();

        KillOldSpikes();
        CreateSpike();
    }

    void CreateWalls()
    {
        float z = -1f; // as z coordinate of wall and grass cannot be same

        // Left Wall
        Vector3 start = new Vector3(-WIDTH, -HEIGHT, z);
        Vector3 finish = new Vector3(-WIDTH, +HEIGHT, z);
        CreateWall(start, finish);

        // Right Wall
        start = new Vector3(+WIDTH, -HEIGHT, z);
        finish = new Vector3(+WIDTH, +HEIGHT, z);
        CreateWall(start, finish);

        // Bottom Wall
        start = new Vector3(-WIDTH, -HEIGHT, z);
        finish = new Vector3(+WIDTH, -HEIGHT, z);
        CreateWall(start, finish);

        // Top Wall
        start = new Vector3(-WIDTH, +HEIGHT, z);
        finish = new Vector3(+WIDTH, +HEIGHT, z);
        CreateWall(start, finish);

    }

    void CreateWall(Vector3 start, Vector3 finish)
    {
        float distance = Vector3.Distance(start, finish);
        int noOfRocks = (int)(distance * 3);
        Vector3 delta = (finish - start) / noOfRocks;

        Vector3 position = start;
        for(int rock = 0; rock<= noOfRocks; rock++)
        {
            float rotation = Random.Range(0, 360f);
            float scale = Random.Range(1.5f, 2f);
            CreateRock(position, scale, rotation);

            position = position + delta;
        }
    }

    void CreateRock(Vector3 position, float scale, float rotation)
    {
        GameObject rock = Instantiate(rockPrefab, position, Quaternion.Euler(0, 0, rotation));
        rock.transform.localScale = new Vector3(scale, scale, 1);
    }
    

    void CreateEgg(bool golden = false)
    {
        Vector3 position;
        position.x = -WIDTH + Random.Range(1f, (WIDTH * 2) - 2f);
        position.y = -HEIGHT + Random.Range(1f, (HEIGHT * 2) - 2f);
        position.z = -1f; // since z of grass is 0
        Egg egg = null;

        if (golden)
            egg = Instantiate(goldEggPrefab, position, Quaternion.identity).GetComponent<Egg>();
        else
            egg = Instantiate(eggPrefab, position, Quaternion.identity).GetComponent<Egg>();

        eggs.Add(egg);
    }

    void CreateSpike()
    {
        for (int i = 0; i < noOfSpike; i++)
        {
            Vector3 position;
            position.x = -WIDTH + Random.Range(1f, (WIDTH * 2) - 2f);
            position.y = -HEIGHT + Random.Range(1f, (HEIGHT * 2) - 2f);
            position.z = -0.4f; // since z of grass is 0
            Spike spike = null;

            spike = Instantiate(spikePrefab, position, Quaternion.identity).GetComponent<Spike>();
            spikes.Add(spike);
        }

        

    }


    public void EggEaten(Egg egg)
    {
        score++;

        noOfEggsForNextLevel--;
        if (noOfEggsForNextLevel == 0)
        {
            score += 10;
            LevelUp();
        }


        else if (noOfEggsForNextLevel == 1) // last egg
        {
            CreateEgg(true);
        }
        else
        {
            CreateEgg(false);
        }

        // setting up new hiscore
        if (score > hiScore)
        {
            hiScore = score;
            hiScoreText.text = "HiScore = " + hiScore;
        }


        // updating scores
        scoreText.text = "Score = " + score;

        eggs.Remove(egg);

        Destroy(egg.gameObject);
    }

    void KillOldEggs()
    {
        foreach(Egg egg in eggs)
        {
            Destroy(egg.gameObject);
        }
        eggs.Clear();
    }

    void KillOldSpikes()
    {
        foreach (Spike spike in spikes)
        {
            Destroy(spike.gameObject);
        }
        spikes.Clear();
    }


}
