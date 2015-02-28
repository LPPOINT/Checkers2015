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
            var lt = Board.Instance.CalculateCellLeftTopPosition(x, y, Offset.x, Offset.y);
            return new Vector3(lt.x + Size.x / 2, lt.y + Size.y / 2, Board.Instance.transform.position.z - Offset.z);
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
