
using System;
using UnityEngine;
using UnityEngine.SceneManagement;


[DisallowMultipleComponent]
public class Booster : MonoBehaviour
{
    Rigidbody rigidbody;
    AudioSource audioSource;
    
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float LevelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip load;

    [SerializeField] ParticleSystem mainEnginePar;
    [SerializeField] ParticleSystem deathPar;
    [SerializeField] ParticleSystem loadPar;
    // Start is called before the first frame update

    enum State {Alive,Dying,Transcending,unkillable}
    State state = State.Alive;

    
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        NoCollission();
        if (state == State.Alive || state == State.unkillable)
        {
            NextLevel();
            Thrust();
            Rotate();
            
        } 
        
    }

    private void NoCollission() 
    {
        if (Input.GetKeyDown(KeyCode.C))
            {
            if (state == State.Alive)
            {
                state = State.unkillable;
            }
            else if (state == State.unkillable)
            {
                state = State.Alive;
            }
            }
    }
         

    private void NextLevel()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Finish();
        }
        
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
            
        }
        else
        {
            audioSource.Stop();
            mainEnginePar.Stop();
        }
    }
      

    private void ApplyThrust()
    {
        // float thrustThisFrame = mainThrust * Time.deltaTime;
        rigidbody.AddRelativeForce(Vector3.up * mainThrust);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEnginePar.Play();

    }

    private void Rotate()
    {
       
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        rigidbody.freezeRotation = true;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        rigidbody.freezeRotation = false;

    }

    void OnCollisionEnter(Collision collision)
    {
       
        if (state != State.Alive) { return; }

        switch (collision.gameObject.tag) 
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSq();
                break;
            default:
                StartDeathSq();
                break;
        }
    }

    private void StartSuccessSq()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(load);
        loadPar.Play();
        Invoke("Finish", LevelLoadDelay);
    }

    private void StartDeathSq()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        deathPar.Play();
        Invoke("Death", LevelLoadDelay);
    }

    private void Death()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void Finish()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //load next scene
        if (currentSceneIndex == 0)
        {
            int nextScene = currentSceneIndex + 1;
            SceneManager.LoadScene(nextScene);
        }
        else SceneManager.LoadScene(currentSceneIndex);
    }
}
