using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cache
{
    #region collider

    private static Dictionary<Collider, Character> cacheCollider = new Dictionary<Collider, Character>();

    static public Character GetCharacter(Collider collider)
    {
        // Add this collider
        if (!cacheCollider.ContainsKey(collider)) 
        {
            cacheCollider.Add(collider, collider.GetComponent<Character>());
        }

        return cacheCollider[collider];
    }
    #endregion

    #region brickscale

    private static Dictionary<ColoredBrick, Vector3> cacheBrickScale = new Dictionary<ColoredBrick, Vector3>();

    static public Vector3 GetBrickScale(ColoredBrick brick)
    {
        if(!cacheBrickScale.ContainsKey(brick))
        {
            cacheBrickScale.Add(brick, brick.transform.localScale);
        }
        return cacheBrickScale[brick];
    }
    #endregion
}