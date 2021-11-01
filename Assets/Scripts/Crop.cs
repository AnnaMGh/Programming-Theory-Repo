using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{

    private Plant plant;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    // ENCAPSULATION of the variable plant
    public void SetPlant(Plant plant)
    {
        this.plant = plant;
    }
    public Plant GetPlant()
    {
        return this.plant;
    }


    public bool HasPlant() {
        return this.plant != null;
    }
}
