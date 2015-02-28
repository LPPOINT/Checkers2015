using System.Linq;
using UnityEngine;

namespace Assets.Classes.Core
{
    public class GameStateBase : RoseEntity
    {

        public bool IsEnabled
        {
            get { return GameStates.Instance.CurrentState == this; }
        }



        public virtual void OnTranslationToStateBegin()
        {
            
        }

        public virtual void OnTranslationFromStateBegin()
        {

        }

        public virtual void OnStateEnter(object model)
        {
            
        }

        public virtual void OnStateLeave()
        {
            
        }

        public static T GetStateInstance<T>() where T : GameStateBase
        {
            return GameStates.Instance.States.FirstOrDefault(controller => controller.GetType() == typeof (T)) as T;
        }

    }
}
