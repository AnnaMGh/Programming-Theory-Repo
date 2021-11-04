using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{

    private Plant plant;
    private Renderer objRenderer;
    private Color[] stateColor;
   

    // Start is called before the first frame update
    void Start()
    {
        stateColor = new Color[] {
        new Color(0.14f, 0.04f, 0f),
        new Color(0.3f, 0.5f, 0.2f),
        new Color(0.15f, 0.25f, 0.15f),
        new Color(0.1f,0.1f,0.1f) };

        objRenderer = this.GetComponent<Renderer>();
        objRenderer.material.color = stateColor[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (plant != null && objRenderer.material.color != stateColor[(int)(plant.state + 1)])
        {
            objRenderer.material.color = stateColor[(int)(plant.state + 1)];
        }
    }


    // ENCAPSULATION of the variable plant
    public void SetPlant(Plant plant)
    {
        this.plant = plant;
    }
    // ENCAPSULATION of the variable plant
    public Plant GetPlant()
    {
        return this.plant;
    }

    public bool HasPlant()
    {
        return this.plant != null;
    }
}
