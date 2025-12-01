using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="new volume setting", menuName ="Settings/Volume")]
public class VolumeSettings : ScriptableObject
{
    public new string name;
    [Range(0, 1)]
    public float value = 1f;

    public void SetValue(float _value)
    {
        value = _value;
    }
}
