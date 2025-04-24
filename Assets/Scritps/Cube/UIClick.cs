using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class UIClick : MonoBehaviour
{
    public InputActionAsset _inputActionAsset;

    public Button uiButton;

    private InputAction buttonAction;

    private void Awake()
    {
        buttonAction = _inputActionAsset.FindActionMap("UI").FindAction("Click");
        
        // buttonAction.performed += OnButttonAc
    }
}
