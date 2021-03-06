﻿using System.Collections.Generic;
using System.Linq;
using Assets.Classes.Core;
using Assets.Classes.Foundation.Enums;
using Assets.Classes.Foundation.Extensions;
using Rotorz.Tile;
using UnityEngine;

namespace Assets.Classes.Implementation
{
    public class Board : SingletonEntity<Board>
    {

        public TileSystem TileSystem;

        public Vector2 CellSize { get; private set; }
        public Vector3 LeftTop { get; private set; }
        public Vector2 CheckerOffset;

        public Checker WhiteCheckerPrefab;
        public Checker BlackCheckerPrefab;
        public Highlighting HighlightingPrefab;

        public Checker GetCheckerPrefab(GameColor checkerColor)
        {
            return checkerColor == GameColor.Black ? BlackCheckerPrefab : WhiteCheckerPrefab;
        }
        public Checker CreateCheckerInstance(GameColor checkerColor)
        {
            var prefab = GetCheckerPrefab(checkerColor);
            if (prefab == null) return null;
            var i = (Instantiate(prefab.gameObject) as GameObject).GetComponent<Checker>();
            Checkers.Add(i);
            return i;
        }
        public Checker CreateCheckerInstance(GameColor checkerColor, int x, int y)
        {
            var instance = CreateCheckerInstance(checkerColor);
            if (instance == null) return null;
            instance.PositionateAt(x, y);

            return instance;
        }

        public Highlighting CreateHighlightingInstance(Color hgColor)
        {
            var instance = (Instantiate(HighlightingPrefab.gameObject) as GameObject).GetComponent<Highlighting>();
            instance.HighlightingColor = hgColor;

            Highlightings.Add(instance);

            return instance;
        }

        public Highlighting CreateHighlightingInstance(Color hgColor, int x, int y)
        {
            var instance = CreateHighlightingInstance(hgColor);
            if (instance == null) return null;
            instance.X = x;
            instance.Y = y;

            return instance;
        }

        public void PositionateCheckersByDefault()
        {

            if (!isBoardIsDirty )
            {
                Debug.Log("Ignore PositionateCheckersByDefault");
                return;
            }

            foreach (var checker in Checkers)
            {
                Destroy(checker.gameObject);
            }
            Checkers.Clear();

            for (var x = 0; x < 8; x++)
            {
                for (var y = 0; y < 8; y++)
                {
                    var color = GameColor.Black;

                    if(y > 4) color = GameColor.White;


                    if (y < 3 || y > 4)
                    {
                        if((y == 0 && x%2 != 0 )|| (y == 1 && x%2 == 0) || (y == 2 && x%2 != 0)
                         || (y == 5 && x % 2 == 0) || (y == 6 && x % 2 != 0) || (y == 7 && x % 2 == 0))
                            CreateCheckerInstance(color, x, y);
                    }

                }
            }

            isBoardIsDirty = false;

        }

        private bool isBoardIsDirty;

        public List<Checker> Checkers { get; private set; }
        public List<Highlighting> Highlightings { get; private set; }

        public VerticalDirection GetCheckersSideByColor(GameColor color)
        {
            return color == GameColor.White ? VerticalDirection.Bottom : VerticalDirection.Top;
        }

        public Checker GetCheckerAt(int x, int y)
        {
            return Checkers.FirstOrDefault(checker => checker.X == x && checker.Y == y);
        }

        public Vector3 CalculateCellLeftTopPosition(int cellX, int cellY, float offsetX, float offsetY)
        {


            return TileSystem.WorldPositionFromTileIndex(cellY, cellX, false);
        }

        public Vector2 CalculateCellCenterPosition(int cellX, int cellY, float offsetX, float offsetY)
        {
            return TileSystem.WorldPositionFromTileIndex(cellY, cellX, true);
        }

        public Vector2 CalculateCellCenterPosition(int cellX, int cellY)
        {
            return CalculateCellCenterPosition(cellX, cellY, 0, 0);
        }

        public Vector3 CalculateCellLeftTopPosition(int cellX, int cellY)
        {
            return CalculateCellLeftTopPosition(cellX, cellY, 0, 0);
        }

        public Vector2? GetNearestCellPosition(Vector3 worldPosition)
        {
            var ti = TileSystem.ClosestTileIndexFromWorld(worldPosition);
            return new Vector2(ti.column, ti.row);
        }

        public void Move(CheckerMove move)
        {
            isBoardIsDirty = true;
            var c = GetCheckerAt(move.OldX, move.OldY);
            if (c == null)
            {
                Debug.Log("Checker to move not found!");
                return;
            }
            c.Move(move);
        }
 
        public void KillChecker(int x, int y)
        {
            KillChecker(GetCheckerAt(x, y));
        }
        public void KillChecker(Checker checker)
        {
            Checkers.Remove(checker);
            checker.KillSelf();
        }

        public void HighlightCell(int x, int y, Color color)
        {
            var h = GetHighlightingForCell(x, y);
            if (h != null)
            {
                h.HighlightingColor = color;
                h.SetDirty();
            }
            else
            {
                CreateHighlightingInstance(color, x, y);
            }
        }
        public void RemoveHighlightingFromCell(int x, int y)
        {
            var h = GetHighlightingForCell(x, y);
            if(h == null) return;
            h.Dispose();
        }
        public bool IsCellHighlighted(int x, int y)
        {
            return Highlightings.Any(h => h.X == x && h.Y == y);
        }

        public void RemoveAllHighlightings()
        {
            foreach (var highlighting in Highlightings)
            {
               highlighting.DisposeImmediateWithoutEvent();
            }

            Highlightings.Clear();

        }

        public Highlighting GetHighlightingForCell(int x, int y)
        {
            return Highlightings.FirstOrDefault(h => h.X == x && h.Y == y);
        }

        private void OnHighlightingDisposed(Highlighting hg)
        {
            Highlightings.Remove(hg);
        }

        protected override void Awake()
        {
            if (TileSystem == null)
            {
                ProcessError("TileSystem not initialized");
                return;
            }

            if (spriteRenderer == null)
            {
                ProcessError("Board should have SpriteRenderer!");
                return;
            }

            Checkers = new List<Checker>();
            Highlightings = new List<Highlighting>();

            GameMessenger.AddListener<Highlighting>(Highlighting.HighlightingDisposedEventName, OnHighlightingDisposed);

            isBoardIsDirty = true;
            PositionateCheckersByDefault();

            CellSize = new Vector2(TileSystem.CellSize.x, TileSystem.CellSize.y);
            LeftTop = new Vector3(transform.position.x - spriteRenderer.bounds.size.x/2, transform.position.y + spriteRenderer.bounds.size.y/2, transform.position.z);

        }

    }
}
