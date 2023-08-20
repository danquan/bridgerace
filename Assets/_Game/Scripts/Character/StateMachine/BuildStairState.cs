using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildStairState : IState<Bot>
{
    public void OnEnter(Bot t)
    {
        t.ChangeAnim(Constant.ANIM_RUN);
        t.ChooseStair();
    }

    public void OnExecute(Bot t)
    {
        if(t.IsReached())
        {

        }
    }

    public void OnExit(Bot t)
    {

    }

}
