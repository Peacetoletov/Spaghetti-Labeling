using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Spaghetti_Labeling
{
    public static class ImageProcessor
    {
        private static Image SpaghettiAssignLabels(List<List<int>> input,  
                                                   List<HashSet<int>> equivalentLabels) {
            /* URGENT TODO:
            When creating the main forest, I add constraints for the first column after all other constraints. This means
            that I don't know which index corresponds to the tree that should be first used.
            I tried to swap the order to have it at the first index, therefore it would always remain at the first index
            even during tree duplicate removal etc., but doing so broke all tests.
            I need to find out how to move it to the first index and not break everything, which will probably involve adjusting
            some of the tests.
            */

            // TODO: this

            /* NOTE: Due to the lack of special forests for the first and last row, all images labeled with the
            Spaghetti algortihm are required to have first 2 rows filled with background pixels and the number
            of rows must be even.
            Once the special trees are implemented, this condition will disappear.
            */
            Debug.Assert(input.Count % 2 == 0);

            List<Tree> mainForest = ForestCreator.MainForest(ODTree.GetTree);
            (List<Tree> endForestEvenTrees, List<List<int>> endForestEvenIndices) = SplitList(ForestCreator.EndForest(mainForest, true));
            (List<Tree> endForestOddTrees, List<List<int>> endForestOddIndices) = SplitList(ForestCreator.EndForest(mainForest, false));

            MainGraph mainGraph = new MainGraph(mainForest, endForestEvenIndices, endForestOddIndices);
            Graph endGraphEven = new Graph(endForestEvenTrees);
            Graph endGraphOdd = new Graph(endForestOddTrees);


            int width = input[0].Count;
            int height = input.Count;
            Image output = new Image(InitMatrixWithZeroes(width, height));

            for (int y = 0; y < height; y += 2) {
                
                    if (y == 0) {
                        // First row
                        // currently does nothing. First implement middle rows, then this.
                    } 
                    else if (y != height - 1) {
                        // Middle rows
                        // TODO: 
                        //SpaghettiLabelBlocksInRow()
                        
                        /*
                        if (not last column) {
                            use main tree
                        } else {
                            use corresponding end tree
                        }
                        */

                    }
                    else {
                        // Last row
                        // currently does nothing. First implement middle rows and first row, then this.
                    }
            }

            return null;
        }

        private static (List<Tree>, List<List<int>>) SplitList(List<(Tree, List<int>)> endForest) {
            List<Tree> trees = new List<Tree>();
            List<List<int>> indicesList = new List<List<int>>();
            foreach ((Tree tree, List<int> indices) in endForest) {
                trees.Add(tree);
                indicesList.Add(indices);
            }
            return (trees, indicesList);
        }

        private static void SpaghettiLabelBlocksInMiddleRow(Image image, int y) {
            // row y means that pixels at positions y and y+1 will be labeled
            int width = image.GetMatrix()[0].Count;
            for (int x = 0; x < width; x += 2) {
                SpaghettiLabelMiddleBlock(666, x, y, width);
            }
        }

        private static int SpaghettiLabelMiddleBlock(int treeIndex, int x, int y, int width) {
            // Labels the 2x2 block, with x and y being the upper left corner of the block.
            // Returns the index of the next tree to be used.

            return 0;
        }

        public static Image SpaghettiCCL(List<List<int>> binaryImage) {
            return CCL(binaryImage, SpaghettiAssignLabels);
        }

        // TODO: override SpaghettiCCL and ClassicCCL methods to work with a path to an image as the argument

        public static Image ClassicCCL(List<List<int>> binaryImage) {
            return CCL(binaryImage, ClassicCCL_AssignLabels);
        }

        private static Image CCL(List<List<int>> binaryImage, Func<List<List<int>>, List<HashSet<int>>, Image> assignLabels) {
            List<HashSet<int>> equivalentLabels = new List<HashSet<int>>();
            Image output = assignLabels(binaryImage, equivalentLabels);
            //ResolveLabelEquivalencies(output, equivalentLabels);      // TEMPORARILY COMMENTED OUT
            return output;
        }

        private static Image ClassicCCL_AssignLabels(List<List<int>> input,  
                                                     List<HashSet<int>> equivalentLabels) {
            int width = input[0].Count;
            int height = input.Count;
            //List<List<int>> output = ;
            Image image = new Image(InitMatrixWithZeroes(width, height));
            int highestLabel = 0;
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    if (input[y][x] == 1) {
                        // Assign label if current pixel is foreground 
                        highestLabel = ClassicCCL_LabelPixel(image, x, y, highestLabel, equivalentLabels);

                        // Store information about equivalent labels
                        ClassicCCL_ManageEquivalencies(image, x, y, equivalentLabels);
                    }
                }
            }

            return image;
        }

        private static int ClassicCCL_LabelPixel(Image image, int x, int y, int highestLabel, List<HashSet<int>> equivalentLabels) {
            List<List<int>> imageMatrix = image.GetMatrix();
            int width = imageMatrix[0].Count;
            if (x - 1 >= 0 && y - 1 >= 0 && imageMatrix[y - 1][x - 1] != 0) {
                imageMatrix[y][x] = imageMatrix[y - 1][x - 1];
            } else if (y - 1 >= 0 && imageMatrix[y - 1][x] != 0) {
                imageMatrix[y][x] = imageMatrix[y - 1][x];
            } else if (x + 1 < width && y - 1 >= 0 && imageMatrix[y - 1][x + 1] != 0) {
                imageMatrix[y][x] = imageMatrix[y - 1][x + 1];
            } else if (x - 1 >= 0 && imageMatrix[y][x - 1] != 0) {
                imageMatrix[y][x] = imageMatrix[y][x - 1];
            } else {
                highestLabel++;
                imageMatrix[y][x] = highestLabel;
                equivalentLabels.Add(new HashSet<int> {highestLabel});
            }
            return highestLabel;
        }

        private static void ClassicCCL_ManageEquivalencies(Image image, int x, int y, 
                                                           List<HashSet<int>> equivalentLabels) {
            /* Note that the only pixels which can theoretically be equivalent and haven't been marked
            as equivalent yet are the ones in the upper left and right corner. */
            // Return if either of the relevant pixels is out of bounds
            List<List<int>> imageMatrix = image.GetMatrix();
            int width = imageMatrix[0].Count;
            if (x == 0 || y == 0 || x == width - 1) {
                return;
            }

            // Return if either of the relevant pixels is background, or if they both have the same label
            int label1 = imageMatrix[y - 1][x - 1];
            int label2 = imageMatrix[y - 1][x + 1];
            if (label1 == 0 || label2 == 0 || label1 == label2) {
                return;
            }

            // If both labels are in different sets, join the sets together
            (HashSet<int> setWithLabel1, HashSet<int> setWithLabel2) = FindSetsWithLabels(equivalentLabels, label1, label2);
            if (setWithLabel1 != setWithLabel2) {
                setWithLabel1.UnionWith(setWithLabel2);
                equivalentLabels.Remove(setWithLabel2);
            }
        }

        private static (HashSet<int>, HashSet<int>) FindSetsWithLabels(List<HashSet<int>> equivalentLabels, int label1, int label2) {
            HashSet<int> setWithLabel1 = null;
            HashSet<int> setWithLabel2 = null;
            foreach (HashSet<int> set in equivalentLabels) {
                if (set.Contains(label1)) {
                    setWithLabel1 = set;
                }
                if (set.Contains(label2)) {
                    setWithLabel2 = set;
                }
            }
            return (setWithLabel1, setWithLabel2);
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

        private static void ResolveLabelEquivalencies(Image image, List<HashSet<int>> equivalentLabels) {
            List<List<int>> imageMatrix = image.GetMatrix();
            int width = imageMatrix[0].Count;
            int height = imageMatrix.Count;
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    for (int i = 0; i < equivalentLabels.Count; i++) {
                        HashSet<int> set = equivalentLabels[i];
                        if (set.Contains(imageMatrix[y][x])) {
                            imageMatrix[y][x] = i + 1;
                        }
                    }
                }
            }
        }

        public static class Tests 
        {
            public static void Run() {
                //TestClassicCCL();
            }

            private static void TestClassicCCL() {
                Image labeled1 = ClassicCCL(Image.TestImages.BinaryImage1().GetMatrix());
                Image reference1 = Image.TestImages.LabeledImage1();
                Debug.Assert(labeled1.Equals(reference1));

                Image labeled2 = ClassicCCL(Image.TestImages.BinaryImage2().GetMatrix());
                Image reference2 = Image.TestImages.LabeledImage2();
                Debug.Assert(labeled2.Equals(reference2));

                /*
                List<List<int>> labeledMatrix = labeled2.GetMatrix();
                foreach (List<int> row in labeledMatrix) {
                    foreach (int label in row) {
                        Console.Write(label);
                    }
                    Console.WriteLine();
                }
                */                

                //Console.WriteLine("Classic CCL works");
            }
        }
    }
}