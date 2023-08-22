using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class BrickOnStair : ColoredBrick
{
    override protected void Start()
    {
        base.Start();
    }

    private void OnTriggerEnter(Collider other)
    {
        // cache for optimizing
        Character otherChar = Cache.GetCharacter(other);

        if (other.gameObject.CompareTag(Constant.TAG_PLAYER)
        || other.gameObject.CompareTag(Constant.TAG_BOT))
        {

            if (otherChar.GetColor() != this.GetColor())
            {
                if(otherChar.GetNumBrick() == 0)
                {
                    otherChar.BlockRunUp();
                }
                else
                {
                    // Change to character's color
                    this.SetColor(otherChar.GetColor());
                    otherChar.RemoveBrick();
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //if (other.gameObject.CompareTag(Constant.TAG_PLAYER)
        //|| other.gameObject.CompareTag(Constant.TAG_BOT))
        {
            // cache for optimization
            Cache.GetCharacter(other).UnBlockRunUp();
        }
    }
}
