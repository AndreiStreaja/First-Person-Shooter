using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class makeRadarObject : MonoBehaviour
{
    public Image image;

    // Start is called before the first frame update
    void Start()
    {
        radar.registerRadarObject(this.gameObject, image);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        radar.removeRadarObject(this.gameObject);
    }
}
