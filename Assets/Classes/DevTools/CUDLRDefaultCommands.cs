using CUDLR;
using UnityEngine;

namespace Assets.Classes.DevTools
{
    public static class CUDLRDefaultCommands
    {
        [CUDLRCommand()]
        public static void Exit()
        {
            Debug.Log("Exit called");
            Application.Quit();
        }

        [CUDLRCommand("log")]
        public static void LogToConsole(string[] args)
        {
            foreach (var s in args)
            {
                Debug.Log(s);
            }
        }
    }
}
