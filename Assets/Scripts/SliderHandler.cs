using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderHandler : MonoBehaviour
{
    private Slider slider;
    private Image bgImg;
    private Image fillImg;
    private float fillToValue;

    private readonly Color minColor = Color.red;
    private readonly Color midColor = Color.yellow;
    private readonly Color maxColor = Color.green;

    private bool isSliderValueDoneUpdating;
    private bool isSliderColorDoneUpdating;

    private Deleagates.ObjectDelegate objDelegate;


    // Update is called once per frame
    void Update()
    {
        //change slider value
        UpdateSliderValue();
        //change slider color
        UpdateSliderColor();
    }

    //ABSTRACTION
    private void UpdateSliderValue() {
        if (!isSliderValueDoneUpdating)
        {
            if (slider.value < fillToValue)
            {
                slider.value += 0.1f;
                if (slider.value > fillToValue)
                {
                    slider.value = fillToValue;
                }
            }
            else if (slider.value > fillToValue)
            {
                slider.value -= 0.1f;
                if (slider.value < fillToValue)
                {
                    slider.value = fillToValue;
                }
            }
            else
            {
                if (objDelegate != null)
                {
                    objDelegate.Invoke(IsSliderFull());
                }
                isSliderValueDoneUpdating = true;
                isSliderColorDoneUpdating = false;
            }
        }
      
    }

    //ABSTRACTION
    private void UpdateSliderColor()
    {
        if (!isSliderColorDoneUpdating)
        {
            if (slider.value == slider.minValue)
            {
                bgImg.color = minColor;
                fillImg.color = minColor;       
            }
            else if (slider.value < slider.maxValue)
            {
                bgImg.color = midColor;
                fillImg.color = midColor;
            }
            else
            {
                bgImg.color = maxColor;
                fillImg.color = maxColor;
            }
            isSliderColorDoneUpdating = true;
        }
    }

    // ENCAPSULATION
    public void SetSlider(float minValue, float maxValue)
    {
        if (slider == null)
        {
            slider = this.gameObject.GetComponent<Slider>();
            bgImg = this.gameObject.transform.GetChild(0).GetComponent<Image>();
            fillImg = this.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Image>();
           
        }
        slider.minValue = minValue;
        slider.maxValue = maxValue;
    }

    // ENCAPSULATION
    public void SetDelegate(Deleagates.ObjectDelegate objectDelegate)
    {
        objDelegate = objectDelegate;
    }

    // ENCAPSULATION
    public float GetMinValue()
    {
        return slider.minValue;
    }

    // ENCAPSULATION
    public void SetValue(float value)
    {
        if (value > slider.maxValue)
        {
            value = slider.maxValue;
        }
        else if (value < slider.minValue)
        {
            value = slider.minValue;
        }
        fillToValue = value;
        isSliderValueDoneUpdating = false;
    }

    // ENCAPSULATION
    public float GetValue()
    {
        return slider.value;
    }  

    // ENCAPSULATION
    public bool IsSliderFull()
    {
        return slider.value == fillToValue;
    }

    

    public void Show(bool show)
    {
        slider.gameObject.SetActive(show);
    }
}
