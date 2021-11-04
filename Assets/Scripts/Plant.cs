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
    public Color[] stateColor { get; protected set; }

    protected SliderHandler sliderHandler;
    private Renderer objRenderer;
    private float stateLength;


    public void Awake()
    {
        GameObject sliderObj = this.gameObject.transform.GetChild(0).GetChild(0).gameObject;
        sliderHandler = sliderObj.GetComponent<SliderHandler>();
        objRenderer = this.GetComponent<Renderer>();
        stateLength = System.Enum.GetNames(typeof(State)).Length;

        SetStateColor();
        SetFertilizerNeed();
        ChangeState((int)State.HEALTHY);
    }


    protected IEnumerator StartLife()
    {
        while (true)
        {
            float sec = Random.Range(10, 15);
            yield return new WaitForSeconds(sec);
            int stateIndex = (int)state;
            if (stateIndex < stateLength - 1)
            {
                ChangeState((State)(stateIndex + 1));
            }
        }
    }

    protected IEnumerator ShowSlider(bool enable, float sec)
    {
        yield return new WaitForSeconds(sec);
        sliderHandler.Show(enable);
    }

    protected void ChangeState(State newState)
    {
        this.state = newState;
        objRenderer.material.color = stateColor[(int)state];

        if (this.state.Equals(State.HEALTHY))
        {
            Heal();
        }
        else if (this.state.Equals(State.NEED_FERTILIZER))
        {
            AskForFertilizer();
        }
        else
        {
            Die();
        }
    }

    public void Fertilize()
    {
        if (state.Equals(State.NEED_FERTILIZER))
        {
            sliderHandler.SetValue(sliderHandler.GetValue() + 1f);
        }
    }

    protected virtual void SetStateColor()
    {
        stateColor = new Color[3] { Color.white, Color.yellow, Color.black };
    }

    protected virtual void SetFertilizerNeed()
    {
        sliderHandler.SetSlider(0, 1);
        sliderHandler.SetDelegate((obj)=> {
            if (sliderHandler.IsSliderFull())
            {
                ChangeState(State.HEALTHY);
            }
        });
        sliderHandler.Show(false);
    }

    protected virtual void Heal()
    {
        StopCoroutine(StartLife());
        StartCoroutine(ShowSlider(false, 1f));
        StartCoroutine(StartLife());
    }

    public virtual void AskForFertilizer()
    {
        sliderHandler.SetValue(sliderHandler.GetMinValue());
        StartCoroutine(ShowSlider(true, 0f));
    }

    public virtual void Die()
    {
        StopCoroutine(StartLife());
    }

    //ABSTRACTION
    public abstract void Rename(int index);


}
