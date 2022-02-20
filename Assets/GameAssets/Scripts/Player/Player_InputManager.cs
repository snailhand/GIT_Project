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
    public string jump = "Jump";

    private Camera playerCamera;
    //public virtual Vector3 GetDirections() =>

    public virtual bool GetJump() => GetButton(jump);
    public virtual bool GetJumpUp() => GetButtonUp(jump);
    public virtual bool GetJumpDown() => GetButtonDown(jump);

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

    public class Button
    {
        private bool holding;
        private int pressedFrame;
        private int releasedFrame;

        public void Pressed()
        {
            if (!holding)
            {
                holding = true;
            }

            pressedFrame = Time.frameCount;
        }

        public void Released()
        {
            if (holding)
            {
                holding = false;
            }

            releasedFrame = Time.frameCount;
        }

        public bool GetButton() => holding;
        public bool GetButtonDown() => pressedFrame != 0 && pressedFrame - Time.frameCount == -1;
        public bool GetButtonUp() => releasedFrame != 0 && releasedFrame == Time.frameCount - 1;
    }

    private static readonly Dictionary<string, Axis> m_axis = new Dictionary<string, Axis>();
    private static readonly Dictionary<string, Button> m_buttons = new Dictionary<string, Button>();

    void Start()
    {
        playerCamera = Camera.main;
    }

    public virtual Vector3 GetCameraDirection(out float magnitude)
    {
        return GlobalToCameraDirection(GetFacingDirection(out magnitude));
    }

    public virtual Vector3 GetFacingDirection(out float magnitude)
    {
        return GetAxisDirection(xName, yName, out magnitude);
    }

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

        print("magnitude: " + magnitude + ", direction: " + direction);
        return direction.normalized;

    }

    protected virtual Vector3 GlobalToCameraDirection(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0)
        {
            var rotation = Quaternion.AngleAxis(playerCamera.transform.eulerAngles.y, Vector3.up);
            direction = rotation * direction;
            direction = direction.normalized;
        }

        return direction;
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

    protected virtual bool GetButton(string name)
    {
        if(!m_buttons.ContainsKey(name))
        {
            m_buttons.Add(name, new Button());
        }
        return (active && m_buttons[name].GetButton() || Input.GetButton(name));
    }

    protected virtual bool GetButtonUp(string name)
    {
        if (!m_buttons.ContainsKey(name))
        {
            m_buttons.Add(name, new Button());
        }
        return (active && m_buttons[name].GetButtonUp() || Input.GetButton(name));
    }

    protected virtual bool GetButtonDown(string name)
    {
        if (!m_buttons.ContainsKey(name))
        {
            m_buttons.Add(name, new Button());
        }
        return (active && m_buttons[name].GetButtonDown() || Input.GetButton(name));
    }


}
