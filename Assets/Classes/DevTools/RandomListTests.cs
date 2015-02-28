using System.Collections.Generic;
using System.Diagnostics;
using Assets.Classes.Core;
using Assets.Classes.Foundation.Extensions;
using Debug = UnityEngine.Debug;

namespace Assets.Classes.DevTools
{
    [EntryState]
    public class RandomListTests : GameStateBase
    {
        public override void OnStateEnter(object model)
        {
            var list = new List<int>();
            var results = new List<int>();
            var sw = new Stopwatch();

            sw.Start();

            for (var i = 0; i < 30; i++) list.Add(i);

            UIPopups.Instance.ShowDialog("Random list tests",
                                         "Test random list of 30 elements.",
                                         new UIDialogPopup.ActionButton("Retry", () => GameStates.Instance.EnableState<RandomListTests>()));

            var withErrors = false;

            for (var j = 0; j < 30; j++)
            {
                var randomElement = list.Random();

                if (results.Contains(randomElement))
                {
                    ProcessError(randomElement.ToString(), Logs.ErrorOutputFlags.ConsoleLog);
                    withErrors = true;
                    continue;
                }
                else
                {
                    Log(randomElement.ToString());
                }

                results.Add(randomElement);
            }

            if (withErrors) ProcessError("Random list doesnt work!", Logs.ErrorOutputFlags.ConsoleLog | Logs.ErrorOutputFlags.Toast);
            else UIPopups.Instance.ShowToast("Random list work!", 4);

            sw.Stop();
            UnityEngine.Debug.Log("Time: " + sw.Elapsed);

        }
    }
}
