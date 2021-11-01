using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Plant : MonoBehaviour
{
    public enum State { HEALTHY, NEED_FERTILIZER, DEAD}

    // ENCAPSULATION
    public int fertilizerNeed { get; protected set; }
    public int fertilizerCount{ get; protected set; }
    public State state { get; protected set; }

    public virtual void Grow()
    {

    } 
    
    public virtual void AskForFertilizer()
    {

    }

    //ABSTRACTION
    public abstract void Rename(int index);

    public void Fertilize() { 
    
    }
}
