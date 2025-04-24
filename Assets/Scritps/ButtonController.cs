using Scritps;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private CutBlock _cutBlock;

    
    public void ClickButton()
    {
        _cutBlock.GetCut();
    }
    
}