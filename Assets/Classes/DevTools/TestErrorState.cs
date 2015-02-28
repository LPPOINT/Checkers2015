using Assets.Classes.Core;

namespace Assets.Classes.DevTools
{
    public class TestErrorState : GameStateBase
    {
        public override void OnStateEnter(object model)
        {
            UIPopups.Instance.ShowDialog("ERROR TEST", "Click to error", new UIDialogPopup.ActionButton("Click!", () => ProcessError("TEST ERROR", Logs.ErrorOutputFlags.ConsoleLog | Logs.ErrorOutputFlags.Cloud | Logs.ErrorOutputFlags.Toast)));
        }
    }
}
