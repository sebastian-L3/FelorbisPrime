using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownBar : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    private float maxValue;
    private float valueNow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void addValue(float value)
    {
        valueNow += value;
        updateValue();
    }

    public void replaceValue(float value)
    {
        valueNow = value;
        updateValue();
    }

    public void setValueToMax()
    {
        valueNow = maxValue;
        updateValue();
    }

    public void updateValue()
    {
        slider.value = valueNow;
    }

    public float getMaxValue()
    {
        return maxValue;
    }

    public void setMaxValue(float value)
    {
        maxValue = value;
    
    }

    public void setValueNow(float value)
    {
        valueNow = value;
    }

    public float getValueNow()
    {
        return valueNow;
    }
}
