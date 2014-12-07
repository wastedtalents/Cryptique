using UnityEngine;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

public enum SKeyCode
{
    Left,
    Right,
    Up,
    Down
}

[Serializable]
public class ComboData  {
    public string name;
    public List<SKeyCode> sequence;
    public ComboAction action;

    private string _stringRep;

    public string ComboString
    {
        get
        {
            if (String.IsNullOrEmpty(_stringRep)) {
                var stb = new StringBuilder();
                sequence.ForEach(sk => stb.Append(ComboTracker._codeMappers[sk]));
                _stringRep = stb.ToString();
            }
            return _stringRep;
        }
    }
}
