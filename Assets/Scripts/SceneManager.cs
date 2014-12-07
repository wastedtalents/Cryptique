using UnityEngine;
using System.Collections;

public class SceneManager : Singleton<SceneManager>
{
    public HUDManager HUDManager { get; private set; }
    public WeaponsManager WeaponManager;

    void Awake()
    {
        HUDManager = GameObject.FindGameObjectWithTag("HUDManager").GetComponent<HUDManager>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
