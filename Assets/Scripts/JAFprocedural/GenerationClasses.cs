using System;
using System.Collections.Generic;

namespace JAFprocedural
{
    [Serializable] public class Coord {
        public int x;
        public int y;
        public int z;

        public Coord(int xV = 0, int yV = 0, int zV = 0) {
            x = xV;
            y = yV;
            z = zV;
        }

        public void SetAs(int xV, int yV, int zV = 0) {
            x = xV;
            y = yV;
            z = zV;
        }
        public void SetAs(Coord newCoord) {
            x = newCoord.x;
            y = newCoord.y;
            z = newCoord.z;
        }
    }

    public class Cell
    {
        public int value;

        public Cell(int v = 0)
        {
            value = v;
        }
    }

    public class Space2D
    {
        public int width;
        public int height;
        public Coord worldOrigin;
        protected List<List<Cell>> grid = new List<List<Cell>> { };

        public Space2D(int x = 1, int y = 1)
        {
            width = x;
            height = y;

            for (int i = 0; i < height; i++)
            {
                List<Cell> newRow = new List<Cell> { };
                for (int j = 0; j < width; j++)
                {
                    Cell newCell = new Cell();
                    newRow.Add(newCell);
                }

                grid.Add(newRow);
            }

            worldOrigin = new Coord(0, 0);
        }

        public int area()
        {
            return (width * height);
        }
        public Cell GetCellObj(Coord location)
        {
            return grid[location.y][location.x];
        }
        public Cell GetCellObj(int x, int y)
        {
            return grid[y][x];
        }
        public int GetCell(Coord location)
        {
            return grid[location.y][location.x].value;
        }
        public int GetCell(int x, int y)
        {
            return grid[y][x].value;
        }

        public bool FindFirstInstance(int cellValue, out Coord location)
        {
            for(int i = 0; i < height; i++)
            {
                for(int j = 0; j < width; j++)
                {
                    if(GetCell(j, i) == cellValue)
                    {
                        location = new Coord(j, i);
                        return true;
                    }
                }
            }

            location = new Coord(-1, -1);
            return false;
        }

        public void SetCell(Coord location, Cell val)
        {
            grid[location.y][location.x] = val;
        }
        public void SetCellVal(Coord location, int val)
        {
            grid[location.y][location.x].value = val;
        }

        public void AddRow(int rows = 1)
        {
            for (int i = 0; i < rows; i++)
            {
                List<Cell> newRow = new List<Cell> { };
                for (int j = 0; j < width; j++)
                {
                    newRow.Add(new Cell());
                }
                grid.Add(newRow);
            }

            height += rows;
        }
        public void AddCol(int cols = 1)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    grid[i].Add(new Cell());
                }
            }

            width += cols;
        }
        public void DropRow(int rows = 1)
        {
            if (rows >= grid.Count) rows = grid.Count - 1;

            for(int i = 0; i < rows; i++)
            {
                grid.RemoveAt(grid.Count - 1);
                height--;
            }
        }
        public void DropCol(int cols = 1)
        {
            if (cols >= width) cols = grid[0].Count - 1;

            for(int i = 0; i < height; i++)
            {
                for(int j = 0; j < cols; j++)
                {
                    grid[i].RemoveAt(grid[i].Count - 1);

                    if (i == 0) width--;
                }

            }
        }
        public void SetDimensions(Coord dim)
        {
            if (dim.y > height) AddRow(dim.y - height);
            else if (dim.y < width) DropRow(height - dim.y);

            if (dim.x > width) AddCol(dim.x - width);
            else if (dim.x < width) DropCol(width - dim.x);
        }
    }
}
