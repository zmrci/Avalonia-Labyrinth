using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Beadando3.Models
{
    public class Motor : IMotor
    {
        public event EventHandler? GameChange; //jatekos lepett
        public event EventHandler? GameOver; // kijutott

        public CellType[,] labyrinth { get; private set; }
        public bool[,] VisibleCells { get; private set; } // a lathato cellak
        public Position playerPosition { get; private set; }
        public bool isActive { get; private set; }

        public Motor(string[,] stringMap)
        {
            int rows = stringMap.GetLength(0); // sorhossz
            int cols = stringMap.GetLength(1); // oszlophossz
            labyrinth = new CellType[rows, cols]; //maga a labirintus
            VisibleCells = new bool[rows, cols];
            Position? start = null; //start kezdetnek null, tudom hogy eleve adott mert csak bal alul lehet kezdo de igy tud olyat is kezelni aminek nem ott van es a berakott mapokat pedig ugy csinaltam hogy a feltetlenek megfeleljenek

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    switch (stringMap[y, x])
                    {
                        case "#":
                            labyrinth[y, x] = CellType.Wall;
                            break;
                        case ".":
                            labyrinth[y, x] = CellType.Road;
                            break;
                        case "S":
                            labyrinth[y, x] = CellType.Road; //a starton is lehet menni
                            start = new Position(x, y);
                            break;
                        case "E":
                            labyrinth[y, x] = CellType.Exit;
                            break;
                        default:
                            throw new UnknownCellTypeException($"Unknown cell type '{stringMap[y, x]}' at position ({x},{y})."); //nem ismeri fel dobja a hibat
                    }
                }
            }

            if (start == null)
            {
                throw new InvalidLabyrinthexception("Start position 'S' not found in the labyrinth map."); // nincsen start akkor nem jo a palya
            }

            playerPosition = start;
            UpdateVisibleCells();
        }

        public void Start() // jatek elinditasa aktiv lesz a jatek es jelzi hogy valtozott a jatek allapot
        {
            isActive = true;
            GameChange?.Invoke(this, EventArgs.Empty);
        }


        public void Pause() // megall
        {
            isActive = false;
        } 
        public void Resume() //elindul
        {
            isActive = true;
        }

        public void Move(int dx, int dy)
        {
            if (!isActive) return;

            Position newPos = playerPosition.Move(dx, dy); //uj pozicio kalkulalasa

            // határon belül van?
            if (newPos.x < 0 || newPos.y < 0 || newPos.x >= labyrinth.GetLength(1) || newPos.y >= labyrinth.GetLength(0))
                return;

            // falra ne lépjen
            if (labyrinth[newPos.y, newPos.x] == CellType.Wall) 
                return;

            // lépés
            playerPosition = newPos;
            UpdateVisibleCells(); // ha tenyleg lepett akkor frissitjuk a lampa fenyet
            GameChange?.Invoke(this, EventArgs.Empty); //frissitem az allapotot

            // kijárat?
            if (labyrinth[newPos.y, newPos.x] == CellType.Exit)
                GameOver?.Invoke(this, EventArgs.Empty);
        }

        public void UpdateVisibleCells() // lmpa fenye
        {
            int rows = labyrinth.GetLength(0);
            int cols = labyrinth.GetLength(1);
            VisibleCells = new bool[rows, cols];
            var queue = new Queue<(int x, int y, int distance)>();

            queue.Enqueue((playerPosition.x, playerPosition.y, 0));
            VisibleCells[playerPosition.y, playerPosition.x] = true;

            while (queue.Count > 0)
            {
                var (x, y, distance) = queue.Dequeue();

                if (distance >= 2 || labyrinth[y, x] == CellType.Wall)
                {
                    continue;
                }

                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        if (dx == 0 && dy == 0) continue;

                        int nextX = x + dx;
                        int nextY = y + dy;

                        if (nextX >= 0 && nextX < cols && nextY >= 0 && nextY < rows && !VisibleCells[nextY, nextX])
                        {
                            VisibleCells[nextY, nextX] = true;
                            queue.Enqueue((nextX, nextY, distance + 1));
                        }
                    }
                }
            }
        }
    }
}
