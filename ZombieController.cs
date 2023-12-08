using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class ZombieController : MonoBehaviour

{
    public int shotsTaken;
    public int shotsRequired = 3;

    public AudioSource[] splatSounds;
    public float runningSpeed;
    public float walkingSpeed;
    public float damageAmount = 5;

    public GameObject ragDoll;
    public GameObject target; // my player character
    Animator zm_anim;
    NavMeshAgent agent;

    enum STATE { IDLE, WANDER, ATTACK, CHASE, DEAD};

    STATE state = STATE.IDLE;
 


    void Start()
    {
       zm_anim = this.GetComponent<Animator>();
       agent = this.GetComponent<NavMeshAgent>();


    }

    void TurnOffTriggers()
    {
        zm_anim.SetBool("walking", false);
        zm_anim.SetBool("attack", false);
        zm_anim.SetBool("running", false);
        zm_anim.SetBool("death", false);

    }
    float DistanceToPlayer()
    {
        if (gameStats.gameOver) return Mathf.Infinity;

        return Vector3.Distance(target.transform.position, this.transform.position);
    }
    bool CanSeePlayer()
    {
        if (DistanceToPlayer() < 10)
            return true;
        return false;
    }
    bool forgetPlayer()
    {
        if (DistanceToPlayer() > 20)
            return true;
        return false;   
    }
    public void killZombie()
    {
        TurnOffTriggers();
        zm_anim.SetBool("death", true);
        state = STATE.DEAD;
    }
    public void DamagePlayer()
    {
        if (target != null)
        {
            target.GetComponent<FPController>().TakeHit(damageAmount);
            splatSoundsPlay();
        }
    }


    void splatSoundsPlay()
    {
        AudioSource audioSource = new AudioSource();
        int n = UnityEngine.Random.Range(1, splatSounds.Length);

        audioSource = splatSounds[n];
        audioSource.Play();
        splatSounds[n] = splatSounds[0];
        splatSounds[0] = audioSource;

    }

    // Update is called once per frame
    void Update()
    {

        if(target == null && gameStats.gameOver == false)
        {
            target = GameObject.FindWithTag("Player");
            return;
        }
        switch(state)
        {
            case STATE.IDLE:
                if (CanSeePlayer())
                    state = STATE.CHASE;
                else if (Random.Range(0, 1000) < 5)
                    state = STATE.WANDER;
                break;
            case STATE.WANDER:
                if (!agent.hasPath)
                {
                    float newX = this.transform.position.x + Random.Range(-5, 5);
                    float newZ = this.transform.position.z + Random.Range(-5, 5);
                    float newY = Terrain.activeTerrain.SampleHeight(new Vector3(newX, 0, newZ));
                    Vector3 dest = new Vector3(newX, newY, newZ);
                    agent.SetDestination(dest);
                    agent.stoppingDistance = 0;
                    TurnOffTriggers();
                    zm_anim.SetBool("walking", true);
                    agent.speed = walkingSpeed;
                }
                if (CanSeePlayer())
                    state = STATE.CHASE;
                else if (Random.Range(0, 1000) < 5)
                {
                    state = STATE.IDLE;
                    TurnOffTriggers();
                    agent.ResetPath();
                }

                    break;
            case STATE.CHASE:
                if (gameStats.gameOver) { TurnOffTriggers(); state = STATE.WANDER; return;};
                agent.SetDestination(target.transform.position);
                agent.stoppingDistance = 5;
                TurnOffTriggers();
                zm_anim.SetBool("running", true);

                if(agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                {
                    state = STATE.ATTACK;
                }
                if (forgetPlayer())
                {
                    state = STATE.WANDER;
                    agent.ResetPath();
                }
                    agent.speed = runningSpeed;
                break;

       
            case STATE.ATTACK:
                if (gameStats.gameOver) { TurnOffTriggers(); state = STATE.WANDER; return; };
                TurnOffTriggers();
                zm_anim.SetBool("attack", true);

                this.transform.LookAt(target.transform.position);

                if (Vector3.Distance(transform.position, target.transform.position) > agent.stoppingDistance + 1)
                {
                    state = STATE.CHASE;
                }
                break;    
            
            
            case STATE.DEAD:
                Destroy(agent);
                AudioSource[] sounds = this.GetComponents<AudioSource>();
                foreach (AudioSource s in sounds)
                
                    s.volume = 0;
            
                this.GetComponent<sink>().StartSink();
                break;
        }
    }

}
