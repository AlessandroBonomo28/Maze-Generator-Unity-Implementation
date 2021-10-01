using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MazeGenerator
{
    public class Cell
    {
        private int x;
        private int y;
        private bool wallUp;
        private bool wallDown;
        private bool wallRight;
        private bool wallLeft;
        private bool visited;


        public bool WallDown { get => wallDown; set => wallDown = value; }
        public bool WallUp { get => wallUp; set => wallUp = value; }
        public bool WallRight { get => wallRight; set => wallRight = value; }
        public bool WallLeft { get => wallLeft; set => wallLeft = value; }
        public bool IsVisited()
        {
            return visited;
        }
        public void SetVisited()
        {
            visited = true;
        }
        public int CountWalls()
        {
            int n = 0;
            if (wallUp) n++;
            if (wallDown) n++;
            if (wallLeft) n++;
            if (WallRight) n++;
            return n;
        }
        public int GetX { get => x; }
        public int GetY { get => y; }

        private Cell()
        {

        }
        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
            wallUp = true;
            wallDown = true;
            wallRight = true;
            wallLeft = true;
            visited = false;
        }

    }
    public class MazeDFS
    {
        private Stack stack;
        private Cell[,] grid;
        private Cell currentCell;
        private int rows;
        private int cols;
        private System.Random rand;

        public int Rows { get => rows; }
        public int Cols { get => cols; }

        private MazeDFS()
        {

        }
        public MazeDFS(int rows,int cols)
        {
            this.rows = Math.Max(1,rows);
            this.cols = Math.Max(1,cols);
            grid = new Cell[this.cols,this.rows];
            rand = new System.Random(Guid.NewGuid().GetHashCode());
        }
        public Cell[,] Generate()
        {
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    grid[j, i] = new Cell(j, i);

            currentCell = grid[rand.Next(Cols),rand.Next(Rows)];
            stack = new Stack();
            currentCell.SetVisited();
            stack.Push(currentCell);
            
            while(true)
            {
                if (stack.Count == 0)
                {
                    Cell c = PickUnvisitedCell();
                    if (c == null)
                    {
                        return grid;
                    }
                    else
                    {
                        stack.Push(c);
                        c.SetVisited();
                    }
                }
                currentCell = (Cell)stack.Pop();
                Cell neighbour = removeWallUnvisitedNeighbour(currentCell);
                if (neighbour != null)
                {
                    stack.Push(currentCell);
                    neighbour.SetVisited();
                    stack.Push(neighbour);


                }
            }
        }
        

        
        private Cell removeWallUnvisitedNeighbour(Cell c)
        {
            int[] l = { 0, 1, 2, 3 };
            l = l.OrderBy(x => rand.Next()).ToArray();
            for (int i=0;i<4;i++)
            {
                Cell neighbour;
                if(l[i]==0)
                {
                    neighbour = GetLeftCell(c);
                    if (neighbour != null && !neighbour.IsVisited() && c.WallLeft)
                    {
                        RemoveWallLeft(c);
                        return neighbour;
                    }
                    continue;
                }
                if (l[i] == 1)
                {
                    neighbour = GetRightCell(c);
                    if (neighbour != null && !neighbour.IsVisited() && c.WallRight)
                    {
                        RemoveWallRight(c);
                        return neighbour;
                    }
                    continue;
                }
                if (l[i] == 2)
                {
                    neighbour = GetUpCell(c);
                    if (neighbour != null && !neighbour.IsVisited() && c.WallUp)
                    {
                        RemoveWallUp(c);
                        return neighbour;
                    }
                    continue;
                }
                if (l[i] == 3)
                {
                    neighbour = GetDownCell(c);
                    if (neighbour != null && !neighbour.IsVisited() && c.WallDown)
                    {
                        RemoveWallDown(c);
                        return neighbour;
                    }
                    continue;
                }
                return null;
            }
            return null;
        }
        private Cell PickUnvisitedCell()
        {
            for (var i = 0; i < rows; i++)
                for (var j = 0; j < cols; j++)
                    if (!grid[j,i].IsVisited()) return grid[j,i];
            return null;
        }
        private Cell GetLeftCell(Cell c)
        {
            if (c.GetX <= 0) return null;
            else return grid[c.GetX - 1, c.GetY];
        }
        private Cell GetRightCell(Cell c)
        {
            if (c.GetX >= (cols - 1)) return null;
            else return grid[c.GetX + 1, c.GetY];
        }
        private Cell GetUpCell(Cell c)
        {
            if (c.GetY >= (rows - 1)) return null;
            else return grid[c.GetX, c.GetY + 1];
        }
        private Cell GetDownCell(Cell c)
        {
            if (c.GetY <= 0) return null;
            else return grid[c.GetX, c.GetY - 1];
        }

        private void RemoveWallUp(Cell c)
        {
            if (c.WallUp == false) return;
            c.WallUp = false;
            Cell cellUp = GetUpCell(c);
            if (cellUp != null) RemoveWallDown(cellUp);
        }
        private void RemoveWallDown(Cell c)
        {
            if (c.WallDown == false) return;
            c.WallDown = false;
            Cell cellDown = GetDownCell(c);
            if (cellDown != null) RemoveWallUp(cellDown);
        }
        private void RemoveWallLeft(Cell c)
        {
            if (c.WallLeft == false) return;
            c.WallLeft = false;
            Cell cellLeft = GetLeftCell(c);
            if (cellLeft != null) RemoveWallRight(cellLeft);
        }
        private void RemoveWallRight(Cell c)
        {
            if (c.WallRight == false) return;
            c.WallRight = false;
            Cell cellRight = GetRightCell(c);
            if (cellRight != null) RemoveWallLeft(cellRight);
        }
    }
    
}

