using UnityEngine;
using System.Collections;

public class InputService : Singleton<InputService> {
    public bool UpPressed { get { return Input.GetKeyDown(KeyCode.W); } }
    public bool DownPressed { get { return Input.GetKeyDown(KeyCode.S); } }
    public bool LeftPressed { get { return Input.GetKeyDown(KeyCode.A); } }
    public bool RightPressed { get { return Input.GetKeyDown(KeyCode.D); } }

    public bool ComboUpPressed { get { return Input.GetKeyDown(KeyCode.UpArrow); } }
    public bool ComboDownPressed { get { return Input.GetKeyDown(KeyCode.DownArrow); } }
    public bool ComboLeftPressed { get { return Input.GetKeyDown(KeyCode.LeftArrow); } }
    public bool ComboRightPressed { get { return Input.GetKeyDown(KeyCode.RightArrow); } }     

    public bool UpHeld { get { return Input.GetKey(KeyCode.W); } }
    public bool DownHeld { get { return Input.GetKey(KeyCode.S); } }
    public bool LeftHeld { get { return Input.GetKey(KeyCode.A); } }
    public bool RightHeld{ get { return Input.GetKey(KeyCode.D); } }

    public bool InventoryPressed { get { return Input.GetKeyDown(KeyCode.Tab); } }
    public bool InventoryUp { get { return Input.GetKeyUp(KeyCode.Tab); } }

}
