using Assets.Classes.Core;
using UnityEngine;

namespace Assets.Classes.Implementation
{
    public class BoardEntity : RoseEntity
    {
        public int X { get; set; }
        public int Y { get; set; }


        public virtual Vector3 Offset
        {
            get { return Vector3.zero; }
        }

        public Vector2 Size
        {
            get { return renderer.bounds.size; }
        }

        public Vector3 CalculateWorldPositionAt(int x, int y)
        {
            var p = Board.Instance.CalculateCellCenterPosition(x, y);
            return new Vector3(p.x, p.y, Board.Instance.transform.position.z - Offset.z);
        }
        public Vector3 CalculateWorldPosition()
        {
            return CalculateWorldPositionAt(X, Y);
        }
        protected void PositionateSprite()
        {
            transform.position = CalculateWorldPosition();

        }

        protected override void Awake()
        {
            transform.parent = Board.Instance.transform;
            base.Awake();
        }

    }
}
