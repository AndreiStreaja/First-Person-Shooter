using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomSound : MonoBehaviour
{
    public AudioSource sound;
    public bool sound3D = true;
    public float firstPlay;
    public float randomMin;
    public float randomMax;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("PlaySound", firstPlay);
        
    }

    // Update is called once per frame
    void PlaySound()
    {
        GameObject newSound = new GameObject();
        AudioSource newnAS = newSound.AddComponent<AudioSource>();
        newnAS.clip = sound.clip;
        if (sound3D)
        {
            newnAS.spatialBlend = 1;
            newnAS.maxDistance = sound.maxDistance; // "sfera" pe care playerul o intalneste (sunetul creste direct proportional cu avansarea playerului de la marginea sferei catre centrul sferei)
            newSound.transform.SetParent(this.transform); // se creeaza un nou obiect care este atasat de obiectul parinte 
            newSound.transform.localPosition = Vector3.zero; // se seteza pozitia noului obiect pe (x, y, z) = 0, 0, 0; 
        }

        newnAS.Play();

        Invoke("PlaySound", Random.Range(randomMin, randomMax));
        Destroy(newSound, sound.clip.length);
    }
}
