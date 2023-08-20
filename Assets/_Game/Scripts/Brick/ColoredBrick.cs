using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoredBrick : MonoBehaviour
{
    [SerializeField] public Transform tf = null;
    [SerializeField] private MeshRenderer meshRenderer = null;
    [SerializeField] private ColorMaterial colorMaterial = null;
    private ColorType selfColor;

    virtual protected void Start()
    {
        if (meshRenderer == null)
            meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if(tf == null)
            tf = gameObject.GetComponent<Transform>();
    }

    public ColoredBrick SetColor(ColorType color)
    {
        meshRenderer.enabled = true;

        selfColor = color;

        if (selfColor == ColorType.INVISIBLE)
            meshRenderer.enabled = false;
        else
            meshRenderer.material = colorMaterial.GetMat(selfColor);

        return this;
    }
    public ColorType GetColor() { return selfColor; }
}
