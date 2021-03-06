/*
Created by Lukáš Osouch for bachelor's thesis Connected Component Labeling Using Directed Acyclic Graphs.
Masaryk University
2022
*/

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Drawing;

namespace Spaghetti_Labeling
{
    public class Image
    {
        private List<List<int>> matrix;

        public Image() {
            // Default image for testing
        }

        public Image(string path) {
            // Loads an image from a file, interpreting black pixels (RGB 0, 0, 0) as background
            // and non-black pixels as foreground.

            /*
            Note: using Bitmap requires the installation of System.Drawing.Common package
            (dotnet add package System.Drawing.Common).
            Note 2: Bitmap is only supported on Windows.
            */

            Bitmap img = new Bitmap(path);
            List<List<int>> matrix = new List<List<int>>();
            for (int y = 0; y < img.Height; y++) {
                List<int> row = new List<int>();
                for (int x = 0; x < img.Width; x++) {
                    Color pixel = img.GetPixel(x, y);
                    if (pixel.R == 0 && pixel.G == 0 && pixel.B == 0) {
                        // Pixel is background
                        row.Add(0);
                    } else {
                        // Pixel is foreground
                        row.Add(1);
                    }
                }
                matrix.Add(row);
            } 
            this.matrix = matrix;
        }

        public Image(List<List<int>> matrix) {
            this.matrix = matrix;
        }

        public void Save(string path) {
            // Saves this image as a file
            int width = matrix[0].Count;
            int height = matrix.Count;
            Bitmap bm = new Bitmap(width, height);
            int numberOfLabels = NormalizeLabels();
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    int adjustedLabel = GetAdjustedLabel(matrix[y][x], numberOfLabels);
                    int r = adjustedLabel % 256;
                    int g = (adjustedLabel / 256) % 256;
                    int b = (adjustedLabel / (256*256)) % 256;
                    Color c = Color.FromArgb(0, r, g, b);
                    bm.SetPixel(x, y, c);
                }
            }
            bm.Save(path, System.Drawing.Imaging.ImageFormat.Bmp);
        }

        private int GetAdjustedLabel(int curLabel, int numberOfLabels) {
            // Adjusts a label to look nicer when saved as an RGB image
            if (curLabel == 0) {
                return 0;
            }
            const int maxLabel = 256*256*256 - 1;
            if (numberOfLabels > maxLabel) {
                throw new ApplicationException("Too many labels");
            }
            int adjustmentCoefficient = maxLabel / numberOfLabels;
            return curLabel * adjustmentCoefficient;
        }

        public int NormalizeLabels() {
            /* Replaces labels in a labeled image such that when iterating through pixels 
            from top left to bottom right, the first encountered label is 1, second 2, 
            third 3 etc. Returns the total number of components.
            */

            // Implementation is similar to flood fill CCL
            int width = matrix[0].Count;
            int height = matrix.Count;
            List<List<int>> newMatrix = ImageProcessor.InitMatrixWithZeroes(width, height);
            int newHighestLabel = 0;
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    if (matrix[y][x] != 0 && newMatrix[y][x] == 0) {
                        newHighestLabel++;
                        int curInputLabel = matrix[y][x];
                        Queue<(int, int)> q = new Queue<(int, int)>();
                        q.Enqueue((y, x));
                        while (q.Count != 0) {
                            (int yFlood, int xFlood) = q.Dequeue();
                            if (xFlood >= 0 && xFlood < width && yFlood >= 0 && yFlood < height &&
                                    matrix[yFlood][xFlood] == curInputLabel && newMatrix[yFlood][xFlood] == 0) {
                                newMatrix[yFlood][xFlood] = newHighestLabel;
                                q.Enqueue((yFlood - 1, xFlood - 1));
                                q.Enqueue((yFlood - 1, xFlood));
                                q.Enqueue((yFlood - 1, xFlood + 1));
                                q.Enqueue((yFlood, xFlood - 1));
                                q.Enqueue((yFlood, xFlood + 1));
                                q.Enqueue((yFlood + 1, xFlood - 1));
                                q.Enqueue((yFlood + 1, xFlood));
                                q.Enqueue((yFlood + 1, xFlood + 1));
                            }
                        }
                    }
                }
            }
            this.matrix = newMatrix;
            return newHighestLabel;
        }
        public void SaveAsBinary(string path) {
            // Saves this image as a binary image
            int width = matrix[0].Count;
            int height = matrix.Count;
            Bitmap bm = new Bitmap(width, height);
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    int r = 0;
                    int g = 0;
                    int b = 0;
                    if (matrix[y][x] != 0) {
                        r = 255;
                        g = 255;
                        b = 255;
                    }
                    Color c = Color.FromArgb(0, r, g, b);
                    bm.SetPixel(x, y, c);
                }
            }
            bm.Save(path, System.Drawing.Imaging.ImageFormat.Bmp);
        }

        public List<List<int>> GetMatrix() {
            return matrix;
        }

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType()) {
                return false;
            }

            List<List<int>> otherMatrix = ((Image) obj).GetMatrix();
            if (matrix.Count != otherMatrix.Count) {
                return false;
            }
            for (int y = 0; y < matrix.Count; y++) {
                if (matrix[y].Count != otherMatrix[y].Count) {
                    return false;
                }
                for (int x = 0; x < matrix[y].Count; x++) {
                    if (matrix[y][x] != otherMatrix[y][x]) {
                        return false;
                    }
                }
            }
            return true;
        }
        
        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public void Print(bool debug=false) {
            // Console.WriteLine("Printing image:");
            for (int i = 0; i < matrix.Count; i++) {
                List<int> row = matrix[i];
                foreach (int pixel in row) {
                    Console.Write(pixel);
                }
                if (debug) {
                    Console.WriteLine(" (row {0})", i);
                } else {
                    Console.WriteLine();
                }
            }
        }

        public static class TestImages
        {
            private static List<List<int>> StringifiedToBinary(List<string> stringified) {
                List<List<int>> binary = new List<List<int>>();
                int firstRowLength = stringified[0].Length;                
                foreach (string row in stringified) {
                    Debug.Assert(row.Length == firstRowLength);
                    List<int> binarRow = new List<int>();
                    foreach (char c in row) {
                        binarRow.Add(c == 'x' ? 1 : 0);
                    }
                    binary.Add(binarRow);
                }
                return binary;
            }

            public static Image BinaryImage1() {
                return new Image(StringifiedToBinary(new List<string> {
                    "................",
                    "................",
                    "....xxx.........",
                    "x...xxxx....xxx.",
                    ".x..x..xx...x.x.",
                    "x.......x...xx.."
                }));
            }

            public static Image LabeledImage1() {
                List<List<int>> labeled = new List<List<int>>();
                labeled.Add(new List<int> {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0});
                labeled.Add(new List<int> {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0});
                labeled.Add(new List<int> {0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0});
                labeled.Add(new List<int> {2,0,0,0,1,1,1,1,0,0,0,0,3,3,3,0});
                labeled.Add(new List<int> {0,2,0,0,1,0,0,1,1,0,0,0,3,0,3,0});
                labeled.Add(new List<int> {2,0,0,0,0,0,0,0,1,0,0,0,3,3,0,0});
                return new Image(labeled);
            }

            public static Image BinaryImage2() {
                return new Image(StringifiedToBinary(new List<string> {
                    "..............",
                    "..............",
                    "x.x.x.x.x...x.",
                    ".x...x.x..xx..",
                    "..x.x...x.....",
                    "x..x.........."
                }));
            }

            public static Image LabeledImage2() {
                List<List<int>> labeled = new List<List<int>>();
                labeled.Add(new List<int> {0,0,0,0,0,0,0,0,0,0,0,0,0,0});
                labeled.Add(new List<int> {0,0,0,0,0,0,0,0,0,0,0,0,0,0});
                labeled.Add(new List<int> {1,0,1,0,1,0,1,0,1,0,0,0,2,0});
                labeled.Add(new List<int> {0,1,0,0,0,1,0,1,0,0,2,2,0,0});
                labeled.Add(new List<int> {0,0,1,0,1,0,0,0,1,0,0,0,0,0});
                labeled.Add(new List<int> {3,0,0,1,0,0,0,0,0,0,0,0,0,0});
                return new Image(labeled);
            }

            public static Image BinaryImage3() {
                // This image contains all possible action results
                return new Image(StringifiedToBinary(new List<string> {
                    "........",     // skip the first 2 rows
                    "........",
                    "xxx.....",     // 2 - new label, 6 - assign x = s, 1 - no action
                    "xx......",
                    "..xx....",     // 3 - assign x = p
                    "..xx....",     
                    ".x.x....",     // 5 - assign x = r, 4 - assign x = q
                    ".x.x....",
                    "........",
                    ".x..x...",
                    "..xx....",     // 8 - merge x = p + r
                    "........",
                    "....x...", 
                    "....x...",
                    "...x....",     // 12 - merge x = r + s 
                    "xxx....."                   
                }));
            }

            public static Image LabeledImage3() {
                List<List<int>> labeled = new List<List<int>>();
                labeled.Add(new List<int> {0,0,0,0,0,0,0,0});
                labeled.Add(new List<int> {0,0,0,0,0,0,0,0});
                labeled.Add(new List<int> {1,1,1,0,0,0,0,0});
                labeled.Add(new List<int> {1,1,0,0,0,0,0,0});
                labeled.Add(new List<int> {0,0,1,1,0,0,0,0});
                labeled.Add(new List<int> {0,0,1,1,0,0,0,0});
                labeled.Add(new List<int> {0,1,0,1,0,0,0,0});
                labeled.Add(new List<int> {0,1,0,1,0,0,0,0});
                labeled.Add(new List<int> {0,0,0,0,0,0,0,0});
                labeled.Add(new List<int> {0,2,0,0,2,0,0,0});
                labeled.Add(new List<int> {0,0,2,2,0,0,0,0});
                labeled.Add(new List<int> {0,0,0,0,0,0,0,0});
                labeled.Add(new List<int> {0,0,0,0,3,0,0,0});
                labeled.Add(new List<int> {0,0,0,0,3,0,0,0});
                labeled.Add(new List<int> {0,0,0,3,0,0,0,0});
                labeled.Add(new List<int> {3,3,3,0,0,0,0,0});
                return new Image(labeled);
            }

            public static Image BinaryImage4() {
                // This image contains all possible action results in EndForestEven
                return new Image(StringifiedToBinary(new List<string> {
                    "....",     // skip the first 2 rows
                    "....",
                    "..xx",     // 2 - new label              
                    "...x",
                    "...x",     // 4 - assign x = q
                    "....",
                    ".xxx",     // 6 - assign x = s
                    "....",
                    "....",     // 1 - no action
                    ".x..",
                    "..x.",     // 3 - assign x = p
                    "...."
                }));
            }

            public static Image BinaryImage5() {
                // This image contains some possible action results in the first row 
                return new Image(StringifiedToBinary(new List<string> {
                    "..x..x.",      // Main trees: 1 - no action, 2 - new label, 6 - assign x = s
                    "...xx.x"      // End tree odd: 6 - assign x = s
                }));
            }

            public static Image BinaryImage6() {
                return new Image(StringifiedToBinary(new List<string> {
                    ".......",
                    ".......",
                    "..x..x.",      // Main trees: 1 - no action, 2 - new label, 6 - assign x = s
                    "...xx.x"       // End tree odd: 6 - assign x = s
                }));
            }

            public static Image BinaryImage7() {
                return new Image(StringifiedToBinary(new List<string> {
                    ".......",
                    ".......",
                    "..x...x",
                    "...xxxx" 
                }));
            }

            public static Image BinaryImage8() {
                return new Image(StringifiedToBinary(new List<string> {
                    ".......",
                    ".......",
                    "..x...x",
                    "...xx.x" 
                }));
            }

            public static Image BinaryImage9() {
                return new Image(StringifiedToBinary(new List<string> {
                    ".......",
                    ".......",
                    "..x....",
                    "...xxx." 
                }));
            }

            public static Image BinaryImage10() {
                return new Image(StringifiedToBinary(new List<string> {
                    "........",
                    "........",
                    "...x..x.",      // Main trees: 1 - no action, 2 - new label, 6 - assign x = s
                    "....xx.x"       // End tree even: 6 - assign x = s
                }));
            }

            public static Image BinaryImage11() {
                return new Image(StringifiedToBinary(new List<string> {
                    "........",
                    "........",
                    "...x...x",
                    "....xxxx" 
                }));
            }

            public static Image BinaryImage12() {
                return new Image(StringifiedToBinary(new List<string> {
                    "........",
                    "........",
                    "...x...x",
                    "....xx.x" 
                }));
            }

            public static Image BinaryImage13() {
                return new Image(StringifiedToBinary(new List<string> {
                    "........",
                    "........",
                    "...x....",
                    "....xxx." 
                }));
            }

            public static Image BinaryImage14() {
                // This image contains all possible action results in the last row using main forest
                return new Image(StringifiedToBinary(new List<string> {
                    ".....................",
                    ".....x.x....xx..x...x",
                    "xxxx.x..x..x..xx.xxx."   
                //   | | | | | | | | | | |   ... each line visualizes the start of a block
                    // 2 - new label, 6 - assign x = s, 4 - assign x = q,
                    // 1 - no action, 3 - assign x = p, 5 - assign x = r,
                    // 8 - merge x = p + r, 12 - merge x = s + r 
                }));
            }

            public static Image BinaryImage15() {
                // This image was randomly generated and was labeled incorrectly in the bottom right corner.
                return new Image(StringifiedToBinary(new List<string> {
                    "x.....x..x",
                    "........xx",
                    "xx.xxx.x..",
                    "xxx.......",
                    ".x.....x..",
                    "..xx......",
                    ".x.....x..",
                    ".....x.x.x",
                    "....x..xxx",
                    "..x....xxx"
                }));
            }

            public static Image BinaryImage16() {
                // Simplified version of image 15
                return new Image(StringifiedToBinary(new List<string> {
                    ".x.....x..",
                    ".....x.x.x",
                    "....x..xxx",
                    "..x....xxx"
                }));
            }

            public static Image BinaryImage17() {
                // This image was randomly generated and was labeled incorrectly in the top right corner.
                return new Image(StringifiedToBinary(new List<string> {
                    ".......xxx",
                    "......x.x.",
                    "...xx...xx",
                    "x....xx...",
                    "..x.x..xx.",
                    "....x.x...",
                    "x.x..xx.x.",
                    "x......x..",
                    "x........x",
                    "x..x.xxx.x"
                }));
            }

            public static Image BinaryImage18() {
                // Simplified version of image 17
                return new Image(StringifiedToBinary(new List<string> {
                    ".xxx..",
                    "x.x..."
                }));
            }

            

            public static Image GenerateRandomImage(int width, int height, double fgProb=0.3, string fileName="") {
                /* Generates a random image with a given width and height. Probability of a pixel being in the foreground
                is given by fgProb. If fileName is not empty, the generated image is also written to a file.
                */

                Random r = new Random();
                List<string> lines = new List<string>();
                for (int y = 0; y < height; y++) {
                    string line = "";
                    for (int x = 0; x < width; x++) {
                        if (r.NextDouble() < fgProb) {
                            line += "x";
                        } else {
                            line += ".";
                        }
                    }
                    lines.Add(line);
                }
                if (fileName != "") {
                    File.WriteAllLines("Test images/" + fileName, lines);
                }
                return new Image(StringifiedToBinary(lines));
            }
        }
    }
}