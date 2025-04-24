using UnityEngine;

namespace Scritps
{
    public class PauseFunc : MonoBehaviour
    {
        public void Pause()
        {
            Time.timeScale = 0;
        }

        public void OnPause()
        {
            Time.timeScale = 1;
        }
    }
}