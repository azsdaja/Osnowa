namespace Libraries.PathPlannersLib
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    //description of the format in http://www.movingai.com/benchmarks/formats.html
    public static class MapReader
    {
        public enum Terrain : byte
        {
            Passable = 0x0,
            Unpassable = 0x1,
            Tree = 0x2,
            Swamp = 0x3,
            Water = 0x4
        }

        public static Terrain[,] ReadTerrain(string fileName)
        {
            if (!File.Exists(fileName)) throw new FileNotFoundException("The file " + fileName + " could not be found.");
            if (Path.GetExtension(fileName) != ".map") throw new FormatException("The file \"" + fileName+ "\" must have a .map extension.");

            StreamReader sr = new StreamReader(fileName);
            string[] splitLine;
            int width, height;

            if (sr.ReadLine() != "type octile")
            {
                sr.Close();
                throw new FormatException("The first line of the file does not match \"type octile\". fileName=" + fileName);
            }

            splitLine = sr.ReadLine().Split();
            if (splitLine.Length == 2 && splitLine[0] == "height") height = Int32.Parse(splitLine[1]);
            else throw new FormatException("The second line of the file does not match \"height x\". fileName=" + fileName);

            splitLine = sr.ReadLine().Split();
            if (splitLine.Length == 2 && splitLine[0] == "width") width = Int32.Parse(splitLine[1]);
            else throw new FormatException("The third line of the file does not match \"width y\". fileName=" + fileName);

            if (sr.ReadLine() != "map") throw new FormatException("The fourth line of the file does not match \"map\". fileName=" + fileName);

            string line;
            Terrain[,] map = new Terrain[width, height];
            int xCount = 0, yCount = 0;
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                xCount = 0;
                foreach (char c in line)
                {
                    switch (c)
                    {
                        case '.':
                        case 'G':
                            map[xCount, yCount] = Terrain.Passable;
                            break;
                        case '@':
                        case 'O':
                            map[xCount, yCount] = Terrain.Unpassable;
                            break;
                        case 'T':
                            map[xCount, yCount] = Terrain.Tree;
                            break;
                        case 'S':
                            map[xCount, yCount] = Terrain.Swamp;
                            break;
                        case 'W':
                            map[xCount, yCount] = Terrain.Water;
                            break;
                        default:
                            throw new FormatException( "Unknown terrain type \"" + c + "\" at position (" +
                                                        xCount.ToString() + "," + yCount.ToString() + "). fileName=" + fileName); 
                    }
                    xCount++;
                }
                if (xCount != width) throw new FormatException("The length of Line " + yCount.ToString() +
                                        " does not match the width specified in the file header. fileName=" + fileName);
                yCount++;
                if (yCount == height && xCount == width) break;
            }
            if (yCount != height) throw new FormatException("The number of lines does not match the map height specified in the file header. fileName=" + fileName);

            sr.Close();
            return map;
        }

        public static void SaveTerrain( string fileName, Terrain[,] map)
        { 
            if (fileName != null) throw new ArgumentNullException("The argument \"fileName\" is null.");
            if (map != null) throw new ArgumentNullException("The argument \"map\" is null.");


            var sw = new StreamWriter(fileName, false, Encoding.ASCII);
            int width = map.GetLength(0);
            int height = map.GetLength(1);
            sw.WriteLine("type octile");
            sw.WriteLine("height {0}", height);
            sw.WriteLine("width {0}", width);
            sw.WriteLine("map");

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; x++)
                {
                    switch (map[x,y])
                    {
                        case Terrain.Passable:
                            sw.Write('G');
                            break;
                        case Terrain.Unpassable:
                            sw.Write('@');
                            break;
                        case Terrain.Tree:
                            sw.Write('T');
                            break;
                        case Terrain.Swamp:
                            sw.Write('S');
                            break;
                        case Terrain.Water:
                            sw.Write('W');
                            break;
                        default:
                            throw new Exception();
                    }
                    sw.Write('\n');
                }
            }
            
        }

        public static bool[,] ConvertTerrainToBooleanMap( Terrain[,] terrain)
        {
            int width = terrain.GetLength(0);
            int height = terrain.GetLength(1);
            var map = new bool[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    switch (terrain[x, y])
                    {
                        case Terrain.Passable:
                        case Terrain.Swamp:                            
                            break;
                        case Terrain.Unpassable:
                        case Terrain.Tree:
                        case Terrain.Water:
                            map[x, y] = true;
                            break;
                        default:
                            throw new Exception();
                    }
                }
            }
            return map;
        }

        public static List<Experiment> ReadScenario(string fileName)
        {
            if (!File.Exists(fileName)) throw new FileNotFoundException("The file " + fileName + " could not be found.");
            if (Path.GetExtension(fileName) != ".scen") throw new FormatException("The file \"fileName\" must have a .map extension.");

            var list = new List<Experiment>();
            float version;
            var sr = new StreamReader(fileName);
            
            string line = sr.ReadLine();
            string[] splitLine = line.Split();
            if (splitLine.Length >= 1 && splitLine[0] != "version")
            {
                version = 0.0f;
                sr.BaseStream.Seek(0, SeekOrigin.Begin);
            }
            else version = float.Parse(splitLine[1]);

            if (version == 0.0f)
            {
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    splitLine = line.Split();
                    if (splitLine.Length == 1) continue;
                    else if (splitLine.Length == 7)
                    {

                        list.Add(new Experiment
                        {
                            Bucket = uint.Parse(splitLine[0]),
                            MapName = splitLine[1],
                            Start = new Point(int.Parse(splitLine[2]), int.Parse(splitLine[3])),
                            Goal = new Point(int.Parse(splitLine[4]), int.Parse(splitLine[5])),
                            OptimalLength = uint.Parse(splitLine[6])
                        });
                    }
                    else throw new FormatException();
                }
            }
            else
            {
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    splitLine = line.Split();

                    if (splitLine.Length == 1) continue;
                    else if (splitLine.Length == 9)
                    {

                        list.Add(new Experiment
                        {
                            Bucket = uint.Parse(splitLine[0]),
                            MapName = splitLine[1],
                            MapWidth = int.Parse(splitLine[2]),
                            MapHeight = int.Parse(splitLine[3]),
                            Start = new Point(int.Parse(splitLine[4]), int.Parse(splitLine[5])),
                            Goal = new Point(int.Parse(splitLine[6]), int.Parse(splitLine[7])),
                            OptimalLength = float.Parse(splitLine[8])
                        });
                    }
                    else throw new FormatException();
                }
            }


            return list;
        }
    }
    public class Experiment
    {
        public uint Bucket { get; set; }
        public string MapName { get; set; }
        public int MapWidth { get; set; }
        public int MapHeight { get; set; }
        public Point Start { get; set; }
        public Point Goal { get; set; }
        public float OptimalLength { get; set; }
    }
}
