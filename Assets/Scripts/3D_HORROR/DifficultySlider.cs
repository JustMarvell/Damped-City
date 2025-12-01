using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DifficultySlider : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI text;
    public DificultySettings[] dificultySettings;

    void Start()
    {
        slider.onValueChanged.AddListener(OnValueChangedCallback);
    }

    public void SetDifficulty(int value)
    {
        DifficultyManager.instance.SetDifficulty(dificultySettings[value]);
        text.text = dificultySettings[value].name;
    }
    
    public void OnValueChangedCallback(float value)
    {
        SetDifficulty((int)value);
    }
}