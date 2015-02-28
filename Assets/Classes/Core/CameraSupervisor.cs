using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Classes.Foundation.Enums;
using Assets.Classes.Foundation.Extensions;
using Assets.Classes.Implementation;
using UnityEngine;

namespace Assets.Classes.Core
{
    public class CameraSupervisor : RoseEntity
    {

        #region Observers declaration
        public interface ICameraObserver<in T>
        {
            void OnCameraPropertyChanged(T oldValue, T newValue);
        }
        public interface ICameraPositionObserver : ICameraObserver<Vector3>
        {
             
        }
        public interface ICameraSizeObserver : ICameraObserver<float>
        {
            
        }
        public interface ICameraColorObserver : ICameraObserver<Color>
        {
             
        }

        #endregion

        #region Static variables

        private static Camera mainCamera;
        public static Camera MainCamera
        {
            get { return mainCamera ?? (mainCamera = Camera.main); }
        }


        private static CameraSupervisor mainCameraSupervisor;
        public static CameraSupervisor Main
        {
            get
            {
                var m =  mainCameraSupervisor ??
                       (mainCameraSupervisor =
                           FindObjectsOfType<CameraSupervisor>()
                               .FirstOrDefault(
                                   observer =>
                                       observer.CameraToObserve != null &&
                                       observer.CameraToObserve.tag == Tags.MainCamera));
                if (m == null)
                {
                    var c = Camera.main;
                    var observer = c.gameObject.AddComponent<CameraSupervisor>();
                    observer.CameraToObserve = c;
                    m = observer;
                    mainCamera = c;
                    mainCameraSupervisor = observer;

                }
                return m;
            }
        }
        #endregion

        #region Borders Collision
        public  Rect CameraViewRect { get; private set; }

        public void InvalidateViewRect()
        {
            CameraViewRect = CameraToObserve.GetCameraViewRect();
        }

        public enum BorderCollisionType
        {
            NoCollision,
            CollisionWithRightBorder,
            CollisionWithLeftBorder
        }

        private  bool CheckPointOneDimension(float point, float border, HorizontalDirection d)
        {
            if (d == HorizontalDirection.Left) return point <= border;
            return point >= border;
        }

        public  BorderCollisionType DetectBordersCollisionWith(BoxCollider2D bc2D)
        {
            return DetectBordersCollisionWith(bc2D.GetWorldRect());
        }
        public  BorderCollisionType DetectBordersCollisionWith(Rect worldRect)
        {

            var maxX = worldRect.xMax;
            var minX = worldRect.xMin;

            if (CheckPointOneDimension(minX, CameraViewRect.xMin, HorizontalDirection.Left)) return BorderCollisionType.CollisionWithLeftBorder;
            if (CheckPointOneDimension(maxX, CameraViewRect.xMax, HorizontalDirection.Right)) return BorderCollisionType.CollisionWithRightBorder;

            return BorderCollisionType.NoCollision;

        }
        public  BorderCollisionType DetectBordersCollisionWith(Vector3 point)
        {

            if (CheckPointOneDimension(point.x, CameraViewRect.xMin, HorizontalDirection.Left)) return BorderCollisionType.CollisionWithLeftBorder;
            if (CheckPointOneDimension(point.x, CameraViewRect.xMax, HorizontalDirection.Right)) return BorderCollisionType.CollisionWithRightBorder;

            return BorderCollisionType.NoCollision;
        }

        #endregion

        #region Observers
        private ArrayList Observers = new ArrayList();


        public void AddObserver<T>(ICameraObserver<T> o) 
        {
           // Observers.Add(o);
        }
        public void RemoveObserver<T>(ICameraObserver<T> o)
        {
           // Observers.Remove(o);
        }
        public IEnumerable<T> GetObservers<T>() where T : class
        {


            foreach (var observer in Observers)
            {

                if ((observer as T) != null)
                    yield return observer as T;
            }
           
        }

        public void InvokeObservers<TObsType, TValType>(TValType oldValue, TValType newValue) where TObsType : class
        {
            var obs = GetObservers<TObsType>();
            foreach (var o in obs)
            {


                var ot = o as ICameraObserver<TValType>;
                if(ot != null)
                    ot.OnCameraPropertyChanged(oldValue, newValue);
            }
        }

        #endregion

        public Camera CameraToObserve;
        private Transform cameraToObserveTransform;
        private Vector3 lastPosition;
        private float lastSize;
        private Color lastColor;


        private void CheckActivate()
        {
            //if (!enabled && Observers.Count == 0) enabled = true;
        }
        private void CheckDeactivate()
        {
           // if (enabled && Observers.Count != 0) enabled = false;
        }

        #region Unity callbacks
        protected override void Awake()
        {
            if (CameraToObserve == null)
            {
                ProcessError("CameraObserver: CameraToObserve == null");
                return;
            }

          //  Observers = new ArrayList();

            if (CameraToObserve.tag == Tags.MainCamera) // FindObjectOfType<T>() call stripping
            {
                mainCameraSupervisor = this;
                mainCamera = CameraToObserve;
            }

            cameraToObserveTransform = CameraToObserve.transform;

            lastPosition = cameraToObserveTransform.position;
            lastSize = CameraToObserve.orthographicSize;
            lastColor = CameraToObserve.backgroundColor;

            InvalidateViewRect();
            base.Awake();
        }
        private void Start()
        {


        }
        private void Update()
        {
            return;
            if (cameraToObserveTransform.position != lastPosition)
            {
                InvokeObservers<ICameraPositionObserver, Vector3>(lastPosition, cameraToObserveTransform.position);
                lastPosition = cameraToObserveTransform.position;
                InvalidateViewRect();
            }
            if (CameraToObserve.orthographicSize != lastSize)
            {
                InvokeObservers<ICameraSizeObserver, float>(lastSize, CameraToObserve.orthographicSize);
                lastSize = CameraToObserve.orthographicSize;
                InvalidateViewRect();
            }
            if (CameraToObserve.backgroundColor != lastColor)
            {
                InvokeObservers<ICameraColorObserver, Color>(lastColor, CameraToObserve.backgroundColor);
                lastColor = CameraToObserve.backgroundColor;
            }
        }
        #endregion



    }
}
