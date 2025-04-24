using Scritps;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private CutBlock _cutBlock;
    
    public void OnClick()
    {
        _cutBlock.GetCut();
    }
}