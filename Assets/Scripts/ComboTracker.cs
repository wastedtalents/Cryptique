using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.Linq;

public class ComboTracker : MonoBehaviour {

    public static Dictionary<SKeyCode, char> _codeMappers;
    public List<ComboData> combos;
    public float comboMoveSpanTime;

    private StringBuilder _comboChars;
    private float _lastComboStepTime;
    private bool _comboStarted;

    static ComboTracker()
    {
        _codeMappers = new Dictionary<SKeyCode, char>()
        {
            { SKeyCode.Down, 'D' },
            { SKeyCode.Up, 'U' },
            { SKeyCode.Left, 'L' },
            { SKeyCode.Right, 'R' }
        };
    }

    void Awake()
    {
        _comboChars = new StringBuilder();
    }

    // Use this for initialization
    void Start () {
	   
	}
	
	// Update is called once per frame
	void Update () {
        if (_comboStarted && (Time.time - _lastComboStepTime > comboMoveSpanTime))
        {
            _comboStarted = false;
            CheckCombo(_comboChars); // check if we have a combo.
        }
	}

    /// <summary>
    /// Registers key pressed.
    /// </summary>
    /// <param name="left"></param>
    internal void Register(SKeyCode code)
    {
        _lastComboStepTime = Time.time;
        if (!_comboStarted)
        {
            _comboStarted = true;
            _comboChars.Length = 0;
            _comboChars.Append(_codeMappers[code]);
        }
        else
            _comboChars.Append(_codeMappers[code]);
    }

    /// <summary>
    /// Check for combo.
    /// </summary>
    /// <param name="_comboChars"></param>
    private void CheckCombo(StringBuilder comboChars)
    {
        var chars = comboChars.ToString();
        var combo = combos.SingleOrDefault(com => com.ComboString.Equals(chars));
        if (combo != null) // we've found a combo!.
        {
            ActionService.Instance.Execute(combo.action);
            _comboStarted = false;
        }
    }

}
