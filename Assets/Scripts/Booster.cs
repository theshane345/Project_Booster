
using UnityEngine;
using UnityEngine.SceneManagement;

public class Booster : MonoBehaviour
{
    Rigidbody rigidbody;
    AudioSource audioSource;
    
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    // Start is called before the first frame update


    enum State {Alive,Dying,Transcending}
    State state = State.Alive;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();


    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }else audioSource.Stop();

    }

    
    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
           // float thrustThisFrame = mainThrust * Time.deltaTime;
            rigidbody.AddRelativeForce(Vector3.up*mainThrust);
            if (!audioSource.isPlaying)
            {

                audioSource.Play();
            }
        }
        else audioSource.Stop();
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
        switch (collision.gameObject.tag) 
        {
            case "Friendly":
                print("friendly");
                break;

            case "Finish":
                state = State.Transcending;
                print("Congrats you win");
                Invoke("Finish", 1f);
                break;
            default:
                state = State.Dying;
                print("dead");
                Invoke("Death", 1f);
                break;
        }
        
    }

    private void Death()
    {
        SceneManager.LoadScene(0);
    }

    private void Finish()
    {
        //load next scene
        
        SceneManager.LoadScene(1);
    }
}
