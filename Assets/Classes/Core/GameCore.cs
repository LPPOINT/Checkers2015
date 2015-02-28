using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Classes.Cloud;
using Assets.Classes.Effects;
using DG.Tweening;
using Soomla.Store;
using UnityEngine;

namespace Assets.Classes.Core
{
    public class GameCore : SingletonEntity<GameCore>
    {

        public Version CoreVersion { get { return new Version(1, 0);} }
        public virtual Version GameVersion { get {return new Version();} }
        public virtual string Name {get { return "UndefinedGame"; }}

        public virtual string AdvertsingProjectId
        {
            get { return "0"; }
        }
        public virtual string UnityAnalyticsProjectId { get { return "0"; } }

        public virtual bool IsPromotionBannerSupported { get { return false; } }
        public virtual bool IsAdsSupported { get { return true; } }
        public virtual bool IsStateBased { get { return true; } }


        public IGameDatabase Database { get; private set; }
        public List<IGameSystem> GameSystems { get; private set; } 

        protected virtual IGameDatabase InitializeDatabase()
        {
            return new PPGameDatabase();
        }
        protected virtual IEnumerable<IGameSystem> InitializeCustomGameSystems()
        {
            yield return new Advertisements();
        }

        private IEnumerable<IGameSystem> InitializeBuiltinGameSystems()
        {
            yield return new Logs();
            yield return new Tweenings();
            yield return new Sessions();
            yield return new Analytics();
            yield return new GameEffects();
            yield return new FPSMonitoring();

            if(IsStateBased) yield return new GameStates();
            if(IsPromotionBannerSupported) yield return new PromotionBanners();
            if(IsAdsSupported) yield return new Advertisements();


        }

        public T GetGameSystem<T>() where T : class, IGameSystem
        {
            var s =  GameSystems.FirstOrDefault(system => system.GetType() == typeof (T)) as T;
            return s;
        }



        public const string GameEntryEvent = "GameEntry";
        public const string GameQuitEvent = "GameQuit";

        protected virtual void OnGameInitializationStarted()
        {
            Debug.Log("GameCore.Awake()");
        }
        protected virtual void OnGameInitializationComplete()
        {
            
        }
        protected virtual void OnGameLaunchingStarted()
        {
            
        }
        protected virtual void OnGameLaunchingComplete()
        {
            
        }


        private void ForEachGameSystem(Action<IGameSystem> gs)
        {
            foreach (var s in GameSystems)
            {
                gs(s);
            }
        }

        protected override void Awake()
        {
            OnGameInitializationStarted();


            Database = InitializeDatabase();

            GameSystems = new List<IGameSystem>();
            GameSystems.AddRange(InitializeBuiltinGameSystems());
            GameSystems.AddRange(InitializeCustomGameSystems());

            ForEachGameSystem(system => system.PreLoad());
            ForEachGameSystem(system => system.Load());
            ForEachGameSystem(system => system.PostLoad());


            GameMessenger.Broadcast(GameEntryEvent);
            OnGameInitializationComplete();

        }
        private void Start()
        {
            OnGameLaunchingStarted();

            ForEachGameSystem(system => system.PreStartup());
            ForEachGameSystem(system => system.Startup());
            ForEachGameSystem(system => system.PostStarup());

            OnGameLaunchingComplete();
        }

        private void Update()
        {
            ForEachGameSystem(system => system.PreUpdate());
            ForEachGameSystem(system => system.Update());
        }
        private void LateUpdate()
        {
            ForEachGameSystem(system => system.PostUpdate());
        }
        private void FixedUpdate()
        {
            ForEachGameSystem(system => system.FixedUpdate());
        }
        private void OnApplicationQuit()
        {

            ForEachGameSystem(system => system.PreShutdown());
            ForEachGameSystem(system => system.Shutdown());
            ForEachGameSystem(system => system.PostShutdown());

            GameMessenger.Broadcast(GameQuitEvent);
        }
    }
}
