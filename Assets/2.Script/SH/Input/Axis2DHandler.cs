using UnityEngine.XR;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "NewAxis2DHandler")]
public class Axis2DHandler : InputHandler
{
    public delegate void Axis2DEventHandler(Vector2 value);
    public event Axis2DEventHandler OnValueChanged;
    public Vector2 value {get; private set;}
    bool isValid;

    public override void UpdateValue()
    {
        isValid = device.isValid;
        if (isValid == false)
        {
            Debug.LogWarning("Invalid device " + device.name);
            return;
        }

        if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 _value))
        {
            if (value != _value)
            {
                value = _value;
                OnValueChanged?.Invoke(value);
            }
        }
        else
        {
            Debug.LogWarning("Failed to Get " + CommonUsages.primary2DAxis.ToString() + " value");
        }
    }

    public bool GetValue(out Vector2 _value)
    {
        _value = value;
        return isValid;
    }
}
