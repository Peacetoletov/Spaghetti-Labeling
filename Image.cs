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
            this.matrix = TestImages.BinaryImage1();
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

        private static class TestImages
        {
            private static List<List<int>> StringifiedToBinary(List<string> stringified) {
                List<List<int>> binary = new List<List<int>>();
                int firstRowLength = stringified[0].Length;                
                foreach (string row in stringified) {
                    Debug.Assert(row.Length == firstRowLength);
                    List<int> binarRow = new List<int>();
                    foreach (char c in row) {
                        binarRow.Add(c == '.' ? 1 : 0);
                    }
                    binary.Add(binarRow);
                }
                return binary;
            }

            public static List<List<int>> BinaryImage1() {
                return StringifiedToBinary(new List<string> {
                    "....xxx.........",
                    "x...xxxx....xxx.",
                    ".x..x..xx...x.x.",
                    "x.......x...xx.."
                });
            }
        }
    }
}