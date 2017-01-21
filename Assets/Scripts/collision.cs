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
    	
        if(col.gameObject.tag == "domino")
        {
        	index = Random.Range (0, audioFiles.Length);
   			_audioSource.clip = audioFiles[index];
            _audioSource.Play();
        }
    }
}