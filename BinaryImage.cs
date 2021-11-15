using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Spaghetti_Labeling
{
    public class BinaryImage
    {
        private List<List<bool>> image;

        public BinaryImage() {
            // Default image for testing
            this.image = TestImages.Image1();
        }

        public BinaryImage(string path) {
            // TODO: Load the image and convert it 
        }

        public List<List<bool>> GetImage() {
            return image;
        }

        private static class TestImages
        {
            private static List<List<bool>> StringifiedToBinary(List<string> stringified) {
                List<List<bool>> binary = new List<List<bool>>();
                int firstRowLength = stringified[0].Length;                
                foreach (string row in stringified) {
                    Debug.Assert(row.Length == firstRowLength);
                    List<bool> binarRow = new List<bool>();
                    foreach (char c in row) {
                        binarRow.Add(c == '.');
                    }
                    binary.Add(binarRow);
                }
                return binary;
            }

            public static List<List<bool>> Image1() {
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