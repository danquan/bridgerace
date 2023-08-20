using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair : MonoBehaviour
{
    [SerializeField] public Transform tf = null;
    [SerializeField] private BrickOnStair brickPrefab = null;
    [SerializeField] private int defaultNumBricks = 1;

    private List<BrickOnStair> listBricks = new List<BrickOnStair>();

    void Start()
    {
        if (tf == null)
            tf = gameObject.GetComponent<Transform>();
    }

    public int GetNumBrick()
    { 
        return defaultNumBricks;
    }


    public void OnInit()
    {
        // Create missing bricks
        for (int i = listBricks.Count; i < defaultNumBricks; ++i)
        {
            if (brickPrefab != null)
            {
                // cache for optimization
                float brickScaleY = Cache.GetBrickScale((ColoredBrick)brickPrefab).y;
                Vector3 brickPos;

                if (listBricks.Count == 0)
                    brickPos = tf.position;
                else
                    brickPos = listBricks[^1].tf.position + new Vector3(0, brickScaleY / 2, brickScaleY);

                BrickOnStair temp  =  Instantiate(brickPrefab.gameObject, brickPos, Quaternion.identity, tf).GetComponent<BrickOnStair>();

                listBricks.Add(temp);
            }
        }

        for(int i = 0; i < defaultNumBricks; ++i)
        {
            listBricks[i].SetColor(ColorType.INVISIBLE);
        }
    }

}
