using System.Collections.Generic;
using System.Linq;
using Assets.Classes.Core;
using UnityEngine;

namespace Assets.Classes.Effects
{
    public class GameEffects : UniqueGameSystem<GameEffects>
    {

        private static List<GameEffect> DefinedEffects;


        public static T GetEffectInstanceInInstance<T>() where T : GameEffect
        {
            return DefinedEffects.FirstOrDefault(effect => effect.GetType() == typeof (T)) as T;
        }

        public override void Load()
        {
            DefinedEffects = new List<GameEffect>(Object.FindObjectsOfType<GameEffect>());
        }
    }
}
