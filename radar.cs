
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarOBJ
{
    public Image icon { get; set; }
    public GameObject owner { get; set; }

}
public class radar : MonoBehaviour
{
    public Transform playerPosition;
    public float mapScale = 2.0f;

    public static List<RadarOBJ> radObjects = new List<RadarOBJ>();

    public static void registerRadarObject(GameObject o, Image i)
    {
        Image image = Instantiate(i);
        radObjects.Add(new RadarOBJ() { owner = o, icon = image});
    }
    public static void removeRadarObject(GameObject o)
    {
        List<RadarOBJ> newList = new List<RadarOBJ>();
        for(int i = 0; i< radObjects.Count; i++)
        {
            if (radObjects[i].owner == o)
            {
                Destroy(radObjects[i].icon);
                continue;
            }
            else
                newList.Add(radObjects[i]);
        }
        radObjects.RemoveRange(0, radObjects.Count);
        radObjects.AddRange(newList);
    }
    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        if (playerPosition == null) return;
        foreach(RadarOBJ ro in radObjects)
        {
            Vector3 radPos = ro.owner.transform.position - playerPosition.position;
            float distToObject = Vector3.Distance(playerPosition.position, ro.owner.transform.position) * mapScale;

            float deltay = Mathf.Atan2(radPos.x, radPos.z) * Mathf.Rad2Deg - 270 - playerPosition.eulerAngles.y;
            radPos.x = distToObject * Mathf.Cos(deltay * Mathf.Deg2Rad) * -1;
            radPos.z = distToObject * Mathf.Sin(deltay * Mathf.Deg2Rad);

            ro.icon.transform.SetParent(this.transform);
            RectTransform rt = this.GetComponent<RectTransform>();
            ro.icon.transform.position = new Vector3(radPos.x + rt.pivot.x, radPos.z + rt.pivot.y, 0) + this.transform.position;
        }
    }
}


