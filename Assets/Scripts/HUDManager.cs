using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDManager : MonoBehaviour
{
    public Image arrowContainer;
    public Sprite upArrow;
    public Sprite downArrow;
    public Sprite rightArrow;
    public Sprite leftArrow;

    public GameObject inventoryPanel;

    public float arrowDissolveSpeed = 2;

    private Dictionary<SKeyCode, Sprite> _codeSprites;
    private Color _tempCol;

    void Awake()
    {
        _tempCol = arrowContainer.color;

        _codeSprites = new Dictionary<SKeyCode, Sprite>()
        {
            { SKeyCode.Up, upArrow},
            { SKeyCode.Down, downArrow},
            { SKeyCode.Right, rightArrow},
            { SKeyCode.Left, leftArrow}
        };
    }

    void Update()
    {
        if (InputService.Instance.InventoryPressed)
        {
            ShowInventory();
        }
        else if (InputService.Instance.InventoryUp)
        {
            HideInventory();
        }
    }

    void Start()
    {
        ShowArrow(SKeyCode.Up);
    }

    public void ShowArrow(SKeyCode code)
    {
        StopCoroutine("ArrowPressed");
        StartCoroutine("ArrowPressed", code);
    }

    private IEnumerator ArrowPressed(SKeyCode code)
    {
        var elapsed = 0f;
        _tempCol.a = 1;

        arrowContainer.sprite = _codeSprites[code];
        arrowContainer.color = _tempCol;
        while (_tempCol.a > 0)
        {
            _tempCol.a = Mathf.Lerp(1, 0, elapsed / arrowDissolveSpeed);
            arrowContainer.color = _tempCol;
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    public void ShowInventory()
    {
        inventoryPanel.SetActive(true);
    }

    public void HideInventory()
    {
        inventoryPanel.SetActive(false);
    }
}
