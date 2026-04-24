using System;

public readonly struct CellNeighbour : IEquatable<CellNeighbour>
{
   public readonly NeighbourType Type;

   public readonly Cell Cell;

   public CellNeighbour(NeighbourType type, Cell cell)
   {
      Type = type;
      Cell = cell;
   }

   public bool Equals(CellNeighbour other)
   {
      return Type == other.Type && Cell == other.Cell;
   }

   public override bool Equals(object obj)
   {
      return obj is CellNeighbour other && Equals(other);
   }

   public override int GetHashCode()
   {
      unchecked
      {
         return ((int)Type * 397) ^ (Cell != null ? Cell.GetHashCode() : 0);
      }
   }
}