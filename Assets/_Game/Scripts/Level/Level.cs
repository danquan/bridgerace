using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private Vector3[] startPos = new Vector3[3];
    /// <summary>
    /// For the convinient, let set: stage[0] -> Winpos
    /// </summary>
    [SerializeField] private Stage[] stage = new Stage[4];
    [SerializeField] private Stair[] stair = new Stair[6];

    private void Start()
    {
        if(levelManager == null)
            levelManager = FindObjectOfType<LevelManager>();
    }

    public void OnInit()
    {
        for(int i = 0; i < stage.Length; ++i)
        {
            stage[i].OnInit();
        }
        for(int i = 0; i < stair.Length; ++i)
        {
            stair[i].OnInit();
        }
    }

    public int GetNumStages() { return stage.Length; }
    public int GetNumCharacters() { return startPos.Length; }
    public Vector3 GetStartPosCharacters(int id) { return startPos[id]; }

    /// <summary>
    /// Search on all stage of this level.
    /// Haven't done, I changed my gameplay so it doesn't need this function
    /// </summary>
    /// <returns>
    /// Return position of the closet brick with ColorType color to characterPos.
    /// </returns>
    public Vector3 GetClosetBrick(Vector3 characterPos, ColorType color)
    {
        return new Vector3(-1, 0, -1);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Return a vector3 stand for position of winpos</returns>
    public Vector3 GetWinPos()
    {
        return stage[0].tf.position;
    }

    /// <summary>
    /// Player has just come to winpos, this round shoulb be finished
    /// </summary>
    public void EndRound()
    {
        // Ping LevelManager
        levelManager.RoundPassed();
    }
}
