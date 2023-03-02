using UnityEngine;

public class Doggo_Sounds : MonoBehaviour
{

    [SerializeField]
    private AudioClip[] StepClips; 
    [SerializeField]
    private AudioClip[] BarkClips;
    [SerializeField]
    private AudioClip[] WhineClips;
    [SerializeField]
    private AudioClip sniffDown;
    [SerializeField]
    private AudioClip sniffUp;
    [SerializeField]
    private AudioClip scratch; 
    [SerializeField]
    private AudioClip eat; 
    

    private AudioSource audioSource;
    private Animator animation;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animation = GetComponent<Animator>();
    }

   /* public void Step()
    {
        if (animation.GetFloat("Speed_f") > 0.1 && animation.GetFloat("Speed_f") < 0.6)
        {
            AudioClip clip = GetRandomClip(StepClips);
            audioSource.PlayOneShot(clip);
        }
        
    }

    public void StepRun()
    {
        if (animation.GetFloat("Speed_f") >= 0.6)
        {
            AudioClip clip = GetRandomClip(StepClips);
            audioSource.PlayOneShot(clip);
        }
    }*/
    
    public void Bark()
    {
        AudioClip clip = AudioUtilities.GetRandomClip(BarkClips);
        audioSource.PlayOneShot(clip);
    }
    

    public void Whining()
    {
        AudioClip clip = AudioUtilities.GetRandomClip(WhineClips);
        audioSource.PlayOneShot(clip);
    }
    
    public void SniffDown()
    {
        audioSource.PlayOneShot(sniffDown);
    }
    public void SniffUp()
    {
        audioSource.PlayOneShot(sniffUp);
    }
    public void Scratching()
    {
        audioSource.PlayOneShot(scratch);
    }
    public void Eating()
    {
        audioSource.PlayOneShot(eat);
    }

}
