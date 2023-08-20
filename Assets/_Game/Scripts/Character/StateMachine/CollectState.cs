using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectState : IState<Bot>
{
    private int numBrick;
    public CollectState(int numBrick = 1)
    {
        this.numBrick = numBrick;
    }

    public void OnEnter(Bot t)
    {
        t.ChangeAnim(Constant.ANIM_RUN);
        t.ChooseBrick();
    }

    public void OnExecute(Bot t)
    {
        if (t.IsReached())
        {
            if (numBrick == 1)
            {
                t.ChangeState(new BuildStairState());
            }
            else
            {
                t.ChangeState(new CollectState(numBrick - 1));
            }
        }
    }

    public void OnExit(Bot t)
    {

    }

}
