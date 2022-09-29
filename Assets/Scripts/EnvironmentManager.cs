using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour {

    public ParticleSystem fogSystem;

    public ParticleSystem leafSystem;

    public MenuToggle weatherToggler;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!this.weatherToggler.isOn)
        {
            if (this.fogSystem.isPlaying)
            {
                this.fogSystem.Stop();
            }
            if (this.leafSystem.isPlaying)
            {
                this.leafSystem.Stop();
            }

        }
        if (this.weatherToggler.isOn)
        {
            if (this.fogSystem.isStopped)
            {
                this.fogSystem.Play();
            }
            if (this.leafSystem.isStopped)
            {
                this.leafSystem.Play();
            }

        }
    }
}
