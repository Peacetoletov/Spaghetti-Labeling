using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Spaghetti_Labeling
{
    public static class ImageProcessor
    {
        public static Image SpaghettiCCL(string path="") {
            // TODO: firest test classic CCL, then implement this

            List<List<int>> input = path == "" ? new Image().GetMatrix() : new Image(path).GetMatrix();

            return null;
        }

        public static Image ClassicCCL(string path="") {
            // TODO: test this

            List<List<int>> input = path == "" ? new Image().GetMatrix() : new Image(path).GetMatrix();

            List<HashSet<int>> equivalentLabels;
            List<List<int>> output = ClassicCCL_AssignLabels(input, out equivalentLabels);

            ResolveLabelEquivalencies(output, equivalentLabels);

            return new Image(output);
        }

        private static List<List<int>> ClassicCCL_AssignLabels(List<List<int>> input,  
                                                               out List<HashSet<int>> equivalentLabels) {
            equivalentLabels = new List<HashSet<int>>();
            int width = input[0].Count;
            int height = input.Count;
            List<List<int>> output = InitMatrixWithZeroes(width, height);
            int highestLabel = 0;
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    if (input[y][x] == 1) {
                        // Assign label if current pixel is foreground 
                        highestLabel = ClassicCCL_LabelPixel(output, x, y, highestLabel);

                        // Store information about equivalent labels
                        /* Note that the only pixels which can theoretically be equivalent and haven't been marked
                        as equivalent yet are the ones in the upper left and right corner. */
                        ClassicCCL_StoreEquivalencies(output, x, y, equivalentLabels);
                    }
                }
            }

            return output;
        }

        private static int ClassicCCL_LabelPixel(List<List<int>> output, int x, int y, int highestLabel) {
            int width = output[0].Count;
            if (x - 1 > 0 && y - 1 > 0 && output[y - 1][x - 1] != 0) {
                output[y][x] = output[y - 1][x - 1];
            } else if (y - 1 > 0 && output[y - 1][x] != 0) {
                output[y][x] = output[y - 1][x];
            } else if (x + 1 < width && y - 1 > 0 && output[y - 1][x + 1] != 0) {
                output[y][x] = output[y - 1][x + 1];
            } else if (x - 1 > 0 && output[y][x - 1] != 0) {
                output[y][x] = output[y][x - 1];
            } else {
                highestLabel++;
                output[y][x] = highestLabel;
            }
            return highestLabel;
        }

        private static void ClassicCCL_StoreEquivalencies(List<List<int>> output, int x, int y, 
                                                          List<HashSet<int>> equivalentLabels) {
            // Return if either of the relevant pixels is out of bounds
            int width = output[0].Count;
            if (x < 0 || y < 0 || x >= width) {
                return;
            }

            // Return if both relevant pixels have the same label
            int label1 = output[y - 1][x - 1];
            int label2 = output[y - 1][x + 1];
            if (label1 == label2) {
                return;
            }

            // Find sets containing the labels, if there are any
            HashSet<int> setWithLabel1 = null;
            HashSet<int> setWithLabel2 = null;
            foreach (HashSet<int> set in equivalentLabels) {
                if (set.Contains(label1)) {
                    setWithLabel1 = set;
                }
                if (set.Contains(label2)) {
                    setWithLabel2 = set;
                }
                if (setWithLabel1 != null && setWithLabel2 != null) {
                    break;
                }
            }

            // Perform an action based on which label belongs to which set
            if (setWithLabel1 == null && setWithLabel2 == null) {
                equivalentLabels.Add(new HashSet<int> {label1, label2});
            } else if (setWithLabel1 != null && setWithLabel2 == null) {
                setWithLabel1.Add(label2);
            } else if (setWithLabel1 == null && setWithLabel2 != null) {
                setWithLabel2.Add(label1);
            } else {
                if (setWithLabel1 != setWithLabel2) {
                    setWithLabel1.UnionWith(setWithLabel2);
                    equivalentLabels.Remove(setWithLabel2);
                }
                // Do nothing if both sets are the same
            }
        }

        private static List<List<int>> InitMatrixWithZeroes(int width, int height) {
            List<List<int>> matrix = new List<List<int>>();
            for (int row = 0; row < height; row++) {
                matrix.Add(new List<int>());
                for (int col = 0; col < width; col++) {
                    matrix[row].Add(0);
                }
            }
            return matrix;
        }

        private static void ResolveLabelEquivalencies(List<List<int>> imageMatrix, 
                                                      List<HashSet<int>> equivalentLabels) {
            int width = imageMatrix[0].Count;
            int height = imageMatrix.Count;
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    for (int i = 0; i < equivalentLabels.Count; i++) {
                        HashSet<int> set = equivalentLabels[i];
                        if (set.Contains(imageMatrix[y][x])) {
                            imageMatrix[y][x] = i;
                        }
                    }
                }
            }
        }
    }
}