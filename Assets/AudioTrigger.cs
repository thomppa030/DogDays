using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    [Tooltip("If true, the audio clip will be played only once. If false, the audio clip will be played every time the player enters the trigger.")]
    [field: SerializeField]
    private bool IsOneShot { get; set; } = true;
    
    [field: SerializeField] private AudioClip SoundClip { get; set; }
    
    private AudioSource _audioSource;
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(AudioUtilities.PlaySound(SoundClip));
            if (IsOneShot)
            {
                Destroy(this);
            }
        }
    }
}