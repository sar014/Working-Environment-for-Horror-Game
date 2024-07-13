using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField]Camera camera;

    public void UpdateHealthBar(float currValue,float maxValue)
    {
        slider.value = currValue/maxValue;
    }
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.allCameras[0];
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = camera.transform.rotation;
    }
}
