using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIStatus : MonoBehaviour
{
    private Color _overlayColor;
    public SpriteRenderer overlay;
    public Scrollbar monsterBar;
    public Scrollbar humanBar;

    void Awake()
    {

    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    /// <summary>
    /// Takes values from -1 to 1. -1 - monster, 1 human.
    /// </summary>
    /// <remarks>Ran BEFORE we run start awake :)</remarks>
    public void SetState(float state) {               
        if (state < 0)
        {
            monsterBar.size = 0;
            humanBar.size = Mathf.Lerp(0, 0.5f, -state);
            return;
        }
        else if (state > 0) // monster
        {
            humanBar.size = 0;
            monsterBar.size = Mathf.Lerp(0, 0.5f, state);

            if(_overlayColor.r == 0)
                _overlayColor = new Color(255, 0, 0, 0);

            _overlayColor.a = Mathf.Lerp(0, 0.4f, state);
            overlay.color = _overlayColor;
            return;
        }
        monsterBar.size = 0;
        humanBar.size = 0;
    }
}
