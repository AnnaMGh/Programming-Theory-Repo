using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : Plant  // INHERITANCE
{
    //ABSTRACTION
    public override void Rename(int index)
    {
        this.gameObject.name = "Bush_" + index;
    }

    //POLYMORPHISM
    protected override void SetStateColor()
    {
        stateColor = new Color[3] { Color.white,
            new Color(0.5f, 0.3f, 0.2f), 
            new Color(0.1f, 0.1f, 0.1f) };
    }

    //POLYMORPHISM
    protected override void SetFertilizerNeed()
    {
        base.sliderHandler.SetSlider(0, 2);
    }
}
