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
    [SerializeField]
    private AudioClip yawn; 
    

    private AudioSource _audioSource;
    private Animator _animation;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _animation = GetComponent<Animator>();
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
        _audioSource.PlayOneShot(clip);
    }
    

    public void Whining()
    {
        AudioClip clip = AudioUtilities.GetRandomClip(WhineClips);
        _audioSource.PlayOneShot(clip);
    }
    
    public void SniffDown()
    {
        _audioSource.PlayOneShot(sniffDown);
    }
    public void SniffUp()
    {
        _audioSource.PlayOneShot(sniffUp);
    }
    public void Scratching()
    {
        _audioSource.PlayOneShot(scratch);
    }
    public void Eating()
    {
        _audioSource.PlayOneShot(eat);
    }
    public void Yawn()
    {
        _audioSource.PlayOneShot(yawn);
    }

}
