using System;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{
   private Dictionary<CellNeighbour, Cell> _neighbours;

   private Cell[] _cells;

   public event Action<Cell> OnCellClicked;

   private void Awake()
   {
      _cells = FindObjectsOfType<Cell>();
      _neighbours = new Dictionary<CellNeighbour, Cell>(_cells.Length * 8);
      var positions = Array.ConvertAll(_cells, t => t.transform.position);
      var distance = 0f;

      for (int i = 0, iMax = _cells.Length; i < iMax; i++)
      {
         _cells[i].OnPointerClickEvent += HandleCellClicked;

         //#if UNITY_EDITOR
         //         _cells[i].OnPointerClickEvent += DebugOnPointerClick;
         //#endif

         for (int j = 0, jMax = _cells.Length; j < jMax; j++)
         {
            if (i == j)
            {
               continue;
            }

            var source = positions[i];
            var destination = positions[j];

            var forward = destination.z.CompareTo(source.z);
            var right = destination.x.CompareTo(source.x);

            var type = (forward, right) switch
            {
               (1, 1) => NeighbourType.ForwardRight,
               (1, 0) => NeighbourType.Forward,
               (1, -1) => NeighbourType.ForwardLeft,
               (0, 1) => NeighbourType.Right,
               (0, -1) => NeighbourType.Left,
               (-1, 1) => NeighbourType.BackwardRight,
               (-1, 0) => NeighbourType.Backward,
               (-1, -1) => NeighbourType.BackwardLeft,
               _ => default
            };

            var key = new CellNeighbour(type, _cells[i]);

            var check = _neighbours.TryGetValue(key, out var cell)
               ? Vector3.Distance(source, cell.transform.position)
               : float.MaxValue;

            distance = Vector3.Distance(source, destination);

            if (distance < check)
            {
               _neighbours[key] = _cells[j];
            }
         }
      }

      var units = FindObjectsOfType<Unit>();
      
      for (int i = 0; i < units.Length; i++)
      {
         var unit = units[i];
         Cell closestCell = null;
         var closestSqr = float.MaxValue;

         for (int j = 0; j < _cells.Length; j++)
         {
            var delta = _cells[j].transform.position - unit.transform.position;
            var sqr = delta.x * delta.x + delta.z * delta.z;
            
            if (sqr < closestSqr)
            {
               closestSqr = sqr;
               closestCell = _cells[j];
            }
         }
         if (closestCell == null)
         {
            continue;
         }
         unit.Cell = closestCell;
         closestCell.Unit = unit;
      }
   }

   private void HandleCellClicked(Cell cell)
   {
      OnCellClicked?.Invoke(cell);
   }

   private void OnDestroy()
   {
      if (_cells == null)
      {
         return;
      }

      for (int i = 0; i < _cells.Length; i++)
      {
         if (_cells[i] != null)
         {
            _cells[i].OnPointerClickEvent -= HandleCellClicked;
         }
      }
   }

}
