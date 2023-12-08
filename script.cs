using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("switchtoMenu", 5);
    }

    void switchtoMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
