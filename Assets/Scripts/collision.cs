using UnityEngine;
using System.Collections;

public class collision : MonoBehaviour
{
	public AudioClip[] audioFiles;
	int index;
	private AudioSource _audioSource;

	void Awake()
	{
		_audioSource = GetComponent<AudioSource>();
	}


    void OnCollisionEnter (Collision col)
    {
    	Debug.Log("Fired");
        if(col.gameObject.tag == "Domino")
        {
        	index = Random.Range (0, audioFiles.Length);
   			_audioSource.clip = audioFiles[index];
            _audioSource.Play();
            Debug.Log("Fired");
            
        }
    }
}