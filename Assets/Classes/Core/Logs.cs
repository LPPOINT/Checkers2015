using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Assets.Classes.Cloud;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Classes.Core
{
    public class Logs : UniqueGameSystem<Logs>
    {



        public class Error : IAnalyticsEventProvider
        {

            public Error()
            {
                
            }

            public Error(string title, string description, object data, bool isFatal, object context)
            {
                Title = title;
                Description = description;
                Data = data;
                IsFatal = isFatal;
                Context = context;
            }

            public string Title { get; set; }
            public string Description { get; set; }
            public object Data { get; set; }
            public object Context { get; set; }
            public bool IsFatal { get; set; }

            public static Error Create(string description, object context)
            {
                return new Error(string.Empty, description, null, false,  context);
            }
            public static Error CreateFatal(string description, object context)
            {
                return new Error(string.Empty, description, null, true, context);
            }

            public string GetConsoleLog()
            {
                if (Context == null) return Description;
                return Context.GetType().Name + ": " + Description;
            }

            public string GetToastMessage()
            {
                return Description;
            }

            public override string ToString()
            {
                if (!string.IsNullOrEmpty(Title)) return string.Format("{0}: {1}", Title, Description);
                return Description;
            }

            public string GetAnalyticsEventName()
            {
                return "Error";
            }

            public IDictionary<string, object> GetAnalyticsEventData()
            {
                return new Dictionary<string, object>()
                       {
                           {"Title", Title},
                           {"Description", Description},
                           {"DataType", Data != null ? Data.GetType().Name : "NULL"},
                           {"ContextType",  Context != null ? Context.GetType().Name : "NULL"},
                           {"IsFatal", IsFatal}
                       };
            }
        }

        [Flags]
        public enum ErrorOutputFlags
        {
            None = 0x0,
            Toast = 0x1,
            ConsoleLog = 0x2,
            Popup = 0x4,
            Cloud = 0x8
        }


        private const float ErrorPopupShowTime = 3f;



        public void ProcessError(Error error, ErrorOutputFlags displayFlags)
        {
            if ((displayFlags & ErrorOutputFlags.Cloud) == ErrorOutputFlags.Cloud) error.SendCustomEvent();
            if ((displayFlags & ErrorOutputFlags.ConsoleLog) == ErrorOutputFlags.ConsoleLog) Debug.LogError(error.GetConsoleLog());
            if ((displayFlags & ErrorOutputFlags.Toast) == ErrorOutputFlags.Toast) UIPopups.Instance.ShowToast(error.GetToastMessage(), ErrorPopupShowTime);
            if ((displayFlags & ErrorOutputFlags.Popup) == ErrorOutputFlags.Popup) UIPopups.Instance.ShowDialog(error.Title, error.Description, new UIDialogPopup.ActionButton("Exit"));
            if ((displayFlags & ErrorOutputFlags.None) == ErrorOutputFlags.None) return;
        }

        public void ProcessError(string message, Object context)
        {
            ProcessError(new Error("error", message, null, false, context), ErrorOutputFlags.ConsoleLog );
        }

        public void ProcessError(string message)
        {
            ProcessError(message, null);
        }

        public void ProcessFatalError(string message)
        {
            ProcessError(new Error("fatalError", message, null, true, null), ErrorOutputFlags.ConsoleLog | ErrorOutputFlags.Cloud);
        }
    }
}
