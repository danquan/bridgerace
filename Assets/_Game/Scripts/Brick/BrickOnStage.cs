using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickOnStage : ColoredBrick
{
    [SerializeField] private Stage stage;

    override protected void Start()
    {
        base.Start();
        OnInit();
    }

    public void OnInit()
    {
    }

    public BrickOnStage SetStage(Stage stage)
    {
        this.stage = stage;
        return this;
    }

    public void RandomFill()
    {
        SetColor(stage.RandomColor());
    }


    private void OnTriggerEnter(Collider other)
    {
        Character otherChar = Cache.GetCharacter(other);

        if(other.gameObject.CompareTag(Constant.TAG_PLAYER) 
        || other.gameObject.CompareTag(Constant.TAG_BOT))
        {
            if (otherChar.GetColor() != this.GetColor())
                return;
            otherChar.AddBrick();

            this.SetColor(ColorType.INVISIBLE);
            Invoke(nameof(RandomFill), 2f);
        }
    }
}
