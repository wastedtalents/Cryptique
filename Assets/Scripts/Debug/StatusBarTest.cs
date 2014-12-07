using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatusBarTest : MonoBehaviour
{
    public UIStatus status;
    public Slider slider;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SlideCHanged()
    {
        status.SetState(slider.value);
    }
}
