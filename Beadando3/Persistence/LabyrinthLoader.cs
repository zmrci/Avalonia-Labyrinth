using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Platform;

namespace Beadando3.Persistence
{
    public class LabyrinthLoader
    {
        public static string[,] Load(string assetPath)  // beolvassuk a fajlt es egy sztringmatrixkent visszaadja
        {
            //android miatt kell
            using var stream = AssetLoader.Open(new Uri($"avares://Beadando3/{assetPath}"));
            using var reader = new StreamReader(stream);
            
            var lines = new List<string>();
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                lines.Add(line);
            }
            
            if (lines.Count == 0)
            {
                return new string[0, 0]; // Handle empty file
            }

            int sor = lines.Count; //sorok es oszlopok hossza elvileg megegyezik de azert ellenorizzuk
            int oszlop = lines[0].Length;

            string[,] map = new string[sor, oszlop]; //a labirintus letrehozasa

            for (int y = 0; y < sor; y++)
            {
                if (lines[y].Length != oszlop)
                    throw new LabyrinthSizeException("Minden sor ugyanannyi karakterből kell álljon!"); // nxn es matrix ellenorzese

                for (int x = 0; x < oszlop; x++) 
                {
                    map[y,x] = lines[y][x].ToString(); //stringbe konvertaljuk
                }
            }

            return map;
        }
    }
}

