using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

abstract public class Character : MonoBehaviour
{
    // cache for optimizing
    [SerializeField] public Transform tf = null;
    [SerializeField] public NavMeshAgent agent = null;
    [SerializeField] protected SkinnedMeshRenderer skinRenderer = null;

    // for debugging
    [SerializeField] private int numberDefaultBrick = 0;

    [SerializeField] GameObject mainBody;
    [SerializeField] protected Animator animController;
    private string currentAnim = Constant.ANIM_IDLE;

    [SerializeField] private ColorMaterial colorMaterial;
    private ColorType selfColor;

    [SerializeField] private ColoredBrick brickPrefab;
    private List<GameObject> listBrick = new List<GameObject>();

    protected virtual void Start()
    {
        if( tf == null )
            tf = gameObject.GetComponent<Transform>();
        if( skinRenderer == null )
            skinRenderer = mainBody.GetComponent<SkinnedMeshRenderer>();
        if (agent == null)
            SetAgent(this.GetComponent<NavMeshAgent>());
        OnInit();
    }

    virtual public void OnInit()
    {
        while (GetNumBrick() > numberDefaultBrick)
            RemoveBrick();
        while (GetNumBrick() < numberDefaultBrick)
            AddBrick();
    }

    public Character SetAgent(NavMeshAgent agent)
    {
        this.agent = agent;
        return this;
    }

    /// <summary>
    /// nction for blocking running up when brick run out
    /// </summary>
    abstract public void BlockRunUp();
    /// <summary>
    /// ction for unblocking running up
    /// </summary>
    abstract public void UnBlockRunUp();

    /// <summary>
    /// Function to pause character (affect by game pause)
    /// </summary>
    abstract public void Pause();
    /// <summary>
    /// Function to resume character
    /// </summary>
    abstract public void Resume();

    public int GetNumBrick() { return listBrick == null ? 0 : listBrick.Count; }
    public void AddBrick()
    {
        // cache for optimization
        Vector3 brickScale = Cache.GetBrickScale((ColoredBrick)brickPrefab);

        Vector3 newBrickPos = tf.position 
                            - brickScale.z * tf.forward 
                            + (brickScale.y + Constant.CHAR_BRICK_GAP) * GetNumBrick() * Vector3.up;

        listBrick.Add(Instantiate(brickPrefab.gameObject,
                                  newBrickPos,
                                  tf.rotation,
                                  tf).
                                GetComponent<ColoredBrick>().
                                SetColor(GetColor()).
                                gameObject);
    }
    public void RemoveBrick()
    {
        //Destroy(listBrick[listBrick.Count - 1]);
        Destroy(listBrick[^1]);
        listBrick.RemoveAt(listBrick.Count - 1);
    }
    public void ClearBrick()
    {
        while(GetNumBrick() > 0)
        {
            RemoveBrick();
        }
    }

    public void SetColor(ColorType color)
    {
        selfColor = color;
        skinRenderer.material = colorMaterial.GetMat(selfColor);
    }
    public ColorType GetColor()
    {
        return selfColor;
    }

    public string GetCurrentAnim()
    {
        return currentAnim;
    }
    protected void PauseAnimation()
    {
        if (currentAnim != null)
            animController.speed = 0f;
    }
    protected void ResumeAnimation()
    {
        if (currentAnim != null)
            animController.speed = 1.0f;
    }
    public void ChangeAnim(string newAnim)
    {
        if (currentAnim != newAnim)
        {
            animController.ResetTrigger(currentAnim);
            currentAnim = newAnim;
            animController.SetTrigger(currentAnim);
        }
    }
}
