using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Spaghetti_Labeling
{
    public class Image
    {
        private List<List<int>> matrix;

        public Image() {
            // Default image for testing
            //this.matrix = TestImages.BinaryImage1();
        }

        public Image(string path) {
            // TODO: Load the image and convert it to BinaryImage
        }

        public Image(List<List<int>> matrix) {
            this.matrix = matrix;
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
            for (int i = 0; i < matrix.Count; i++) {
                if (matrix[i].Count != otherMatrix[i].Count) {
                    return false;
                }
                for (int j = 0; j < matrix.Count; j++) {
                    if (matrix[i][j] != otherMatrix[i][j]) {
                        return false;
                    }
                }
            }
            return true;
        }
        
        public override int GetHashCode() {
            return base.GetHashCode();
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
                    "....xxx.........",
                    "x...xxxx....xxx.",
                    ".x..x..xx...x.x.",
                    "x.......x...xx.."
                }));
            }

            public static Image LabeledImage1() {
                List<List<int>> labeled = new List<List<int>>();
                labeled.Add(new List<int> {0,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0});
                labeled.Add(new List<int> {2,0,0,0,1,1,1,1,0,0,0,0,3,3,3,0});
                labeled.Add(new List<int> {0,2,0,0,1,0,0,1,1,0,0,0,3,0,3,0});
                labeled.Add(new List<int> {2,0,0,0,0,0,0,0,1,0,0,0,3,3,0,0});
                return new Image(labeled);
            }

            public static Image BinaryImage2() {
                return new Image(StringifiedToBinary(new List<string> {
                    "x.x.x.x.x.x.x",
                    ".x...x.x...x.",
                    "..x.x...x.x..",
                    "x..x........."
                }));
            }

            public static Image LabeledImage2() {
                List<List<int>> labeled = new List<List<int>>();
                labeled.Add(new List<int> {1,0,1,0,1,0,1,0,1,0,2,0,2});
                labeled.Add(new List<int> {0,1,0,0,0,1,0,1,0,0,0,2,0});
                labeled.Add(new List<int> {0,0,1,0,1,0,0,0,1,0,2,0,0});
                labeled.Add(new List<int> {3,0,0,1,0,0,0,0,0,0,0,0,0});
                return new Image(labeled);
            }
        }
    }
}