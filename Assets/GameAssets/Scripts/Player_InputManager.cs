using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_InputManager : MonoBehaviour
{
    [Header("General")]
    public bool active = true;

    [Header("Buttons")]
    public string xName = "Horizontal";
    public string yName = "Vertical";

    //public virtual Vector3 GetDirections() =>

    public class Axis
    {
        private float value;
        public void SetValue(float valueToSet)
        {
            value = valueToSet;
        }
        public float GetValue()
        {
            return value;
        }
    }

    private static readonly Dictionary<string, Axis> m_axis = new Dictionary<string, Axis>();

    public Vector3 GetDirection()
    {
        return(GetAxisDirection(xName, yName, out _));
    }

    protected Vector3 GetAxisDirection(string xAxis, string yAxis, out float magnitude)
    {
        var horizontal = GetAxis(xAxis);
        var vertical = GetAxis(yAxis);
        var direction = new Vector3(horizontal, 0, vertical);

        magnitude = direction.magnitude;
        return direction.normalized;

    }

    // Returns the value of the axis by its name
    private float GetAxis(string axisName)
    {
        float axisValue;

        if(!m_axis.ContainsKey(axisName))
        {
            m_axis.Add(axisName, new Axis());
        }

        if(m_axis[axisName].GetValue() != 0)
        {
            axisValue = m_axis[axisName].GetValue();
        }
        else
        {
            axisValue = Input.GetAxis(axisName);
        }

        return(axisValue);
    }
}
