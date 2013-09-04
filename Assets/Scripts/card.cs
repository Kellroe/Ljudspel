using UnityEngine;
using System.Collections;

public class card : MonoBehaviour {
    //public AudioClip sound = null;
    
    void Start()
    {
        animation["idle"].speed = Random.Range(0.8f, 1.2f);
        

       // Debug.Log(card.transform.FindChild("card mesh").animation["idle"].speed);
        
    }
    void Update()
    {
        if (animation.clip.name == "turn" && animation["turn"].time >= animation.clip.length*Random.Range(0.7f,0.9f))
        {
            animation.CrossFade("idle",3.8f);
        }
    }

    public void PlaySound() {
        audio.Play();
    }
    
}
