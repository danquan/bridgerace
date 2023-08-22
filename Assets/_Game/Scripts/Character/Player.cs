using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Joystick joystick;
    // Start is called before the first frame update

    protected bool isPaused = false;
    protected float velocityUp = 1e9f;

    override protected void Start()
    {
        base.Start();
    }

    public override void OnInit()
    {
        // when we replay or pass to next level, joystich should be reseted
        joystick.OnPointerUp(null);
        base.OnInit();
    }

    override public void BlockRunUp()
    {
        velocityUp = 0;
    }
    override public void UnBlockRunUp()
    {
        velocityUp = 1e9f;
    }

    public override void Pause()
    {
        isPaused = true;
        this.PauseAnimation();
    }
    public override void Resume()
    {
        isPaused = false;
        this.ResumeAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused)
            return;

        if (joystick.Horizontal != 0f && joystick.Vertical != 0f)
        {
            ChangeAnim("run");

            Vector3 moveVector = (Vector3.right * joystick.Horizontal + Vector3.forward * joystick.Vertical);
            tf.rotation = Quaternion.LookRotation(moveVector);
            tf.Translate((new Vector3(moveVector.x, moveVector.y, Mathf.Min(velocityUp, moveVector.z))) * moveSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            ChangeAnim("idle");
        }
    }
}
