using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Plant : MonoBehaviour
{
    public enum State { HEALTHY, NEED_FERTILIZER, DEAD }

    // ENCAPSULATION
    public int fertilizerNeed { get; protected set; }
    public int fertilizerCount { get; protected set; }
    public State state { get; protected set; }
    public Color[] stateColor
    {
        get { return new Color[3] { Color.green, Color.yellow, Color.black }; }
        protected set { }
    }

    private Renderer objRenderer;
    private float stateLength;


    public void Awake()
    {
        state = State.HEALTHY;
        objRenderer = this.GetComponent<Renderer>();
        objRenderer.material.color = stateColor[(int)state];
        stateLength = System.Enum.GetNames(typeof(State)).Length;
        StartCoroutine(StartLife());
    }


    protected IEnumerator StartLife()
    {
        while (true)
        {
            float sec = Random.Range(5, 10);
            yield return new WaitForSeconds(sec);
            int stateIndex = (int)state;
            if (stateIndex < stateLength-1)
            {
                ChangeState((State)(stateIndex + 1));
            }
        }
    }

    protected void ChangeState(State newState)
    {
        this.state = newState;
        objRenderer.material.color = stateColor[(int)state];

        if (this.state.Equals(State.HEALTHY)){
            Heal();
        }
        else if (this.state.Equals(State.NEED_FERTILIZER))
        {
            AskForFertilizer();
        }
        else{
            Die();
        }
    }

    public void Fertilize()
    {
        if (state.Equals(State.NEED_FERTILIZER))
        {
            ChangeState(State.HEALTHY);
        }
    }

    public virtual void Heal()
    {

    }

    public virtual void AskForFertilizer()
    {
      
    }

    public virtual void Die()
    {
        StopCoroutine(StartLife());
    }

    //ABSTRACTION
    public abstract void Rename(int index);


}
