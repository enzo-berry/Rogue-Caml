using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Slider slider;
    public GameObject volumeValue;
    private TextMeshPro _volumeValue;

    //Error on the recovery of the component to modify the volume display
    // Start is called before the first frame update
    void Start()
    {
        _volumeValue = volumeValue.GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        _volumeValue.text = $"{(slider.value * 100):.} | 100";
    }
}
