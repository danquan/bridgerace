using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : Singleton<LevelManager>
{
    // cache transform for optimizing 
    [SerializeField] protected Transform tf = null;

    [SerializeField] GameManager gameManager = null;

    [SerializeField] private Level[] listLevels = new Level[2];

    // Player -> Character 0
    [SerializeField] private Player player;
    [SerializeField] private Player playerPrefab;
    [SerializeField] private Bot botPrefab;
    [SerializeField] private Character[] listChars;


    private ColorType[] idColor = new ColorType[(int)ColorType.MAX_COLOUR];
    private Level level = null;
    private int currentLevel;

    void Start()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
        if(tf == null)
            tf = gameObject.GetComponent<Transform>();
        //OnInit();
    }

    public void OnInit()
    {
        for (int i = 0; i < idColor.Length; i++)
            idColor[i] = (ColorType)((i + 1) % idColor.Length);
        SetLevel(0);
    }

    public void RePlay()
    {
        SetLevel(currentLevel);
    }

    public void NextLevel()
    {
        SetLevel(currentLevel + 1);
    }

    public void RoundPassed()
    {
        if (currentLevel + 1 == listLevels.Length)
        {
            gameManager.SetGameState(GameState.WIN);
        }
        else
        {
            gameManager.SetGameState(GameState.LEVELPASS);
        }
    }

    /// <summary>
    /// Pause game
    /// </summary>
    public void Pause()
    {
        listChars[0].GetComponent<Player>().Pause();
        for (int i = 1; i < listChars.Length; ++i)
            listChars[i].GetComponent<Bot>().Pause();
    }
    /// <summary>
    /// Resume game
    /// </summary>
    public void Resume()
    {
        listChars[0].GetComponent<Player>().Resume();
        for (int i = 1; i < listChars.Length; ++i)
            listChars[i].GetComponent<Bot>().Resume();
    }

    /// <summary>
    /// Prepare things for new level
    /// </summary>
    /// <param name="newLevel"> newLevel: id of level in listLevels</param>
    private void SetLevel(int newLevel)
    {
        // Deactive last level
        if(level != null)
            level.gameObject.SetActive(false);

        // Active new level
        currentLevel = newLevel;
        level = listLevels[currentLevel];
        level.gameObject.SetActive(true);
        level.OnInit();

        int numCharacters = level.GetNumCharacters();

        for(int i = 1; i < listChars.Length; i++)
        {
            Destroy(listChars[i].gameObject);
        }

        // set character for player
        listChars = new Character[numCharacters];

        player.SetColor(idColor[0]);
        player.OnInit();

        Vector3 playerPosition = playerPrefab.transform.position
                                 + level.GetStartPosCharacters(0)
                                 + tf.position;

        // player will be still in that pos if agent isn't disabled
        if(player.agent != null) player.agent.enabled = false; 
        player.tf.SetPositionAndRotation(playerPosition, playerPrefab.transform.rotation);
        if (player.agent != null) player.agent.enabled = true;

        listChars[0] = player;
        
        // set character for bot
        for(int i = 1; i < listChars.Length; ++i)
        {
            Vector3 botPosition = botPrefab.transform.position // default position of that character
                                 + level.GetStartPosCharacters(i) // local start position 
                                 + tf.position; // global position of that stage

            listChars[i] = Instantiate(botPrefab, botPosition, Quaternion.identity, tf);

            (listChars[i] as Bot).SetLevelManager(this);
            listChars[i].SetColor(idColor[i]);
            listChars[i].tag = Constant.TAG_BOT;
        }
    }

    /// <summary>
    /// Get position of the closet brick with ColorType color to characterPos.
    /// Just make level calculate itself.
    /// Haven't done, I changed my gameplay so it doesn't need this function
    /// </summary>
    /// <returns>
    /// Return position of the closet brick with ColorType color to characterPos.
    /// </returns>
    public Vector3 GetClosetBrick(Vector3 characterPos, ColorType color)
    {
        return level.GetClosetBrick(characterPos, color);
        //return new Vector3(-1, 0, -1);
    }

    /// <summary>
    /// Get Position of Winning Position
    /// </summary>
    /// <returns>A vector3 stand for winning position</returns>
    public Vector3 GetWinPos()
    {
        return level.GetWinPos();
    }
}
