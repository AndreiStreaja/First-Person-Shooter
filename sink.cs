using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class sink : MonoBehaviour
{
    public float sinkDelay = 8;
    float destroyHeight;


    // Start is called before the first frame update
    void Start()
    {
        if (this.gameObject.tag == "Ragdoll")
        {
            Invoke("StartSink", 5f);
        }
    }

    public void StartSink()
    {
        destroyHeight = Terrain.activeTerrain.SampleHeight(this.transform.position) - 5;
        Collider[] colList = this.transform.GetComponentsInChildren<Collider>();
        foreach (Collider c in colList)
        {
            Destroy(c);
        }
        InvokeRepeating("sinkIntoGround", sinkDelay, 0.1f);
    }
    void sinkIntoGround()
    {
        this.transform.Translate(0, -0.001f, 0);
        if (this.transform.position.y < destroyHeight)
        {
            Destroy(this.gameObject);

        }


    }

}
