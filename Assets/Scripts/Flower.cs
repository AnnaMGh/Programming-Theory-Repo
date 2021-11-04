using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : Plant // INHERITANCE
{
    //ABSTRACTION
    public override void Rename(int index)
    {
        this.gameObject.name = "Flower_" + index;
    }

    //POLYMORPHISM
    protected override void SetStateColor()
    {
        stateColor = new Color[3] { Color.white,
            new Color(0.6f, 0.6f, 0.1f),
            new Color(0.1f, 0.1f, 0.1f) };
    }


    //POLYMORPHISM
    protected override void SetFertilizerNeed()
    {
        sliderHandler.SetSlider(0, 1);
    }
}
