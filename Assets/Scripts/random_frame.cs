using UnityEngine;
using System.Collections;

public class random_frame : MonoBehaviour {

	// Use this for initialization
	void Start () {

            AnimationState a = null;
            //Find current animation that is playing
            foreach (AnimationState ac in animation)
            {
                if (ac.enabled)
                {
                    a = ac;
                    break;
                }
            }

            //If animation is found set it to a random time
            if (a != null)
                a.time = Random.Range(0, a.length);
        }
	
	}


