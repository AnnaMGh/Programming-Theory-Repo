using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : Plant
{ 
    //ABSTRACTION
    public override void Rename(int index)
    {
        this.gameObject.name = "Flower_" +index;
    }

}
