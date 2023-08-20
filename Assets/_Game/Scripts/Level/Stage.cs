using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] public Transform tf = null;

    [SerializeField] Level level;
    [SerializeField] private int numRow = 6;
    [SerializeField] private int numCol = 6;
    [SerializeField] GameObject brickPrefabs = null;

    [SerializeField] private Stage nextStage = null;
    [SerializeField] private Stair[] stair = new Stair[1];

    private BrickOnStage[,] tileMap = new BrickOnStage[30, 30];
    List<ColorType> filledColor = new();
    private List<bool> openColor = new(new bool[(int)ColorType.MAX_COLOUR]);

    private void Start()
    {
        if (tf == null)
            tf = this.GetComponent<Transform>();
    }

    public void OnInit()
    {
        for(int i = 0; i < numRow; ++i)
            for (int j = 0; j < numCol; ++j)
                if (tileMap[i, j] != null)
                {
                    tileMap[i, j].SetColor(ColorType.INVISIBLE);
                }

        for (int i = 0; i < openColor.Count; ++i)
            openColor[i] = false;

        filledColor.Clear();

        return;
    }

    private void Update()
    {
        int cnt = 0;
        for (int i = 0; i < openColor.Count; ++i)
            if (openColor[i])
                ++cnt;

        if (cnt == level.GetNumCharacters())
        {
            openColor[0] = true; // never run again

            for (int i = 0; i < numRow; ++i)
                for (int j = 0; j < numCol; ++j)
                    if (tileMap[i, j].GetColor() == ColorType.INVISIBLE)
                    {
                        tileMap[i, j].RandomFill();
                    }
        }
    }

    /// <summary>
    /// Choose an arbitrary color from unlocked colors
    /// </summary>
    /// <returns>A variable of Type ColorType</returns>
    public ColorType RandomColor()
    {
        return filledColor[Random.Range(0, filledColor.Count)];
    }


    /// <summary>
    /// Get Global Position of upper-left brick. 
    /// Use for getting another brick's position.
    /// </summary>
    /// <returns>Global Position of upper-left brick</returns>
    private Vector3 GetUpperLeft()
    {
        return new Vector3(-(numCol * brickPrefabs.transform.localScale.x + (numCol - 1) * brickPrefabs.transform.localScale.x * 1.5f) / 2 + brickPrefabs.transform.localScale.x / 2,
                           0,
                           -(numRow * brickPrefabs.transform.localScale.z + (numRow - 1) * brickPrefabs.transform.localScale.z * 1.5f) / 2 + brickPrefabs.transform.localScale.z / 2);
    }

    /// <summary>
    /// Get Global Position of brick on cell (i, j).
    /// Base on Global Position of upper-left brick.
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <returns> Global Position of brick on cell (i, j)</returns>
    public Vector3 GetBrickPos(int i, int j)
    {
        Vector3 upperLeft = GetUpperLeft();

        return transform.position +
                new Vector3(upperLeft.x + j * brickPrefabs.transform.localScale.x * 2.5f,
                            brickPrefabs.transform.position.y, 
                            upperLeft.z + i * brickPrefabs.transform.localScale.z * 2.5f);

    }

    /// <summary>
    /// Calculate randomly number of brick. 
    /// Base on number of bricks to reach this stage 
    /// </summary>
    /// <returns></returns>
    public int ThisGetRandNumBricks()
    {
        return Random.Range(1, stair[0].GetNumBrick());
    }

    /// <summary>
    /// Calculate randomly number of brick. 
    /// Base on number of bricks to reach nextStage 
    /// </summary>
    /// <returns></returns>
    public int GetRandNumBricks()
    {
        return nextStage.ThisGetRandNumBricks();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="characterPos"></param>
    /// <param name="color"></param>
    /// <returns>Return position of the closet brick with ColorType `color` to characterPos</returns>
    public Vector3 GetClosetBrick(Vector3 characterPos, ColorType color)
    {
        Vector3 ans = new Vector3(-1, -1, -1);

        // Initially Brick
        for (int i = 0; i < numRow; ++i)
            for (int j = 0; j < numCol; ++j)
                if (tileMap[i,j].GetColor() == color)
                {
                    ans = GetBrickPos(i, j);
                }

        // Find Closet Brick
        for (int i = 0; i < numRow; ++i)
            for (int j = 0; j < numCol; ++j)
                if(tileMap[i, j].GetColor() == color)
                {
                    Vector3 tempPos = GetBrickPos(i, j);
                    if (Vector3.Distance(ans, characterPos) > Vector3.Distance(characterPos, tempPos))
                        ans = tempPos;
                }
        return ans + (tf.position.y - ans.y) * Vector3.up;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Constant.TAG_PLAYER) || other.CompareTag(Constant.TAG_BOT))
        {
            // cache for optimization
            Character otherChar = Cache.GetCharacter(other);

            /* If this is winpos*/
            if(this.gameObject.CompareTag(Constant.TAG_WINPOS))
            {
                if (other.CompareTag(Constant.TAG_PLAYER))
                {
                    // Player come to winpos -> End this level, go to the next
                    level.EndRound();
                }
                else
                {
                    // Bot wait until player come
                    (otherChar as Bot).ChangeState(new IdleState());
                }

                return;
            }

            /* If this stage is not winpos, then continue build tileMap */

            if (tileMap[0, 0] == null)
            {
                // Create tile map at the first come
                for (int i = 0; i < numRow; ++i)
                    for (int j = 0; j < numCol; ++j)
                    {
                        tileMap[i, j] = Instantiate(brickPrefabs,
                                                    GetBrickPos(i, j),
                                                    Quaternion.identity,
                                                    transform).
                                                GetComponent<BrickOnStage>().
                                                SetStage(this).
                                                SetColor(ColorType.INVISIBLE)
                                                .gameObject.GetComponent<BrickOnStage>();
                    }
            }


            // Set Stage for Bot
            if(other.gameObject.CompareTag(Constant.TAG_BOT))
            {
                (otherChar as Bot).SetStage(this);
            }

            // Unblock new color by the character
            if (!openColor[(int)otherChar.GetColor()])
            {
                ColorType color = otherChar.GetColor();

                filledColor.Add(color);
                openColor[(int)color] = true;

                for (int i = 0; i < numRow; ++i)
                    for (int j = 0; j < numCol; ++j)
                        if (tileMap[i, j].GetColor()
                                == ColorType.INVISIBLE
                            && Random.Range(1, 100) % 3 == 2)
                        {
                            tileMap[i, j].SetColor(color);
                        }
            }

        }
    }
}
