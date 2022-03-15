using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Spaghetti_Labeling
{
    public static class ImageProcessor
    {
        private static (Image, Dictionary<int, HashSet<int>>) SpaghettiAssignLabels(Image input) {
            List<List<int>> inputMatrix = input.GetMatrix();

            GraphManager gmFirst = new GraphManager(GraphManager.GraphType.FirstRow);
            GraphManager gmMiddle = new GraphManager(GraphManager.GraphType.MiddleRows);
            GraphManager gmLast = new GraphManager(GraphManager.GraphType.LastRow);
            // ^ For better performance, GraphManager could be created once at the start of the program and then 
            // passed as an argument

            int width = inputMatrix[0].Count;
            int height = inputMatrix.Count;
            Image output = new Image(InitMatrixWithZeroes(width, height));
            Dictionary<int, HashSet<int>> equivalentLabels = new Dictionary<int, HashSet<int>>();
            ActionPerformer ap = new ActionPerformer(input, output, equivalentLabels);
            //Console.WriteLine("Initiated spaghetti labeling");

            for (int y = 0; y < height; y += 2) {
                if (y == 0) {
                    // First row
                    //Console.WriteLine("Labeling blocks in first row.");
                    SpaghettiLabelBlocks(y, gmFirst, input, ap);
                } 
                else if (y != height - 1) {
                    // Middle rows
                    SpaghettiLabelBlocks(y, gmMiddle, input, ap);
                }
                else {
                    // Last row (only used in images with an odd number of rows)
                    //Console.WriteLine("Labeling blocks in last row.");
                    SpaghettiLabelBlocks(y, gmLast, input, ap);
                }
            }

            return (output, equivalentLabels);
        }

        private static void SpaghettiLabelBlocks(int y, GraphManager gm, Image input, ActionPerformer ap) {
            // row y means that pixels at positions y and y+1 will be labeled
            List<List<int>> inputMatrix = input.GetMatrix();
            int width = inputMatrix[0].Count;
            int nextTreeIndex = gm.GetStartTreeIndex();
            int action = -1;
            for (int x = 0; x < width; x += 2) {
                //Console.Write("Block in column {0}. ", x);
                if (x < width - 2) {
                    // Use main tree
                    //Console.Write("Using main tree with index {0}. ", nextTreeIndex);
                    (action, nextTreeIndex) = GetActionAndNextTreeIndex(gm.AdjustIndexAndGetRootInMainGraph(nextTreeIndex), input, x, y);
                         
                } else {
                    if (x == width - 2) {
                        // Use even tree
                        //Console.Write("Using even end tree with index {0}. ", nextTreeIndex);
                        action = GetAction(gm.AdjustIndexAndGetRootInEndGraphEven(nextTreeIndex), input, x, y);
                        //gm.AdjustIndexAndGetRootInEndGraphEven(nextTreeIndex).InfoDFS();
                    } else {
                        // Use odd tree
                        //Console.Write("Using odd end tree with index {0}. ", nextTreeIndex);
                        action = GetAction(gm.AdjustIndexAndGetRootInEndGraphOdd(nextTreeIndex), input, x, y);
                        //gm.AdjustIndexAndGetRootInEndGraphOdd(nextTreeIndex).InfoDFS();
                    }
                    
                }
                //Console.WriteLine("Chosen action: {0}", action);
                ap.Perform(action, x, y);
            }
        }

        private static (int, int) GetActionAndNextTreeIndex(AbstractNode root, Image input, int x, int y) {
            // Traverses the tree from a given root node based on the "input" matrix and returns a tuple containing the 
            // action to be performed and next tree index
            List<List<int>> inputMatrix = input.GetMatrix();
            AbstractNode curNode = root;
            while (curNode is Node) {
                if (ConditionToPixelValue(((Node) curNode).GetCondition(), x, y, input) == 0) {
                    curNode = ((Node) curNode).GetLeft();
                } else {
                    curNode = ((Node) curNode).GetRight();
                }
            }
            // curNode now contains the correct leaf based on the pixels in surrounding blocks
            int action = GetAnyHashsetElement(((Leaf) curNode).GetActions());
            int nextTreeIndex = ((Leaf) curNode).GetNextTreeIndex();

            return (action, nextTreeIndex);
        }

        private static int GetAction(AbstractNode root, Image input, int x, int y) {
            // Traverses the tree from a given end root node based on the "input" matrix and returns the action to be performed.
            // This method is only used for end trees, as no next tree index is returned.
            List<List<int>> inputMatrix = input.GetMatrix();
            AbstractNode curNode = root;
            while (curNode is Node) {
                if (ConditionToPixelValue(((Node) curNode).GetCondition(), x, y, input) == 0) {
                    curNode = ((Node) curNode).GetLeft();
                } else {
                    curNode = ((Node) curNode).GetRight();
                }
            }
            // curNode now contains the correct leaf based on the pixels in surrounding blocks
            return GetAnyHashsetElement(((Leaf) curNode).GetActions());
        }

        private static T GetAnyHashsetElement<T>(HashSet<T> set) {
            // This is probably super dumb but I couldn't find a built-in method that does this
            foreach (T elem in set) {
                return elem;
            }
            throw new IndexOutOfRangeException("Set must not be empty!");
        }

        private static int ConditionToPixelValue(char condition, int x, int y, Image input) {
            // Takes a node condition (given by a char in range a-t) and the current position of a block (corresponding to the 'o' char
            // of the block) and returns the value of the pixel at the position specified by the condition.
            List<List<int>> inputMatrix = input.GetMatrix();
            switch (condition) {
                case 'a':
                    return inputMatrix[y - 2][x - 2];
                case 'b':
                    return inputMatrix[y - 2][x - 1];
                case 'c':
                    return inputMatrix[y - 2][x];
                case 'd':
                    return inputMatrix[y - 2][x + 1];
                case 'e':
                    return inputMatrix[y - 2][x + 2];
                case 'f':
                    return inputMatrix[y - 2][x + 3];
                case 'g':
                    return inputMatrix[y - 1][x - 2];
                case 'h':
                    return inputMatrix[y - 1][x - 1];
                case 'i':
                    return inputMatrix[y - 1][x];
                case 'j':
                    return inputMatrix[y - 1][x + 1];
                case 'k':
                    return inputMatrix[y - 1][x + 2];
                case 'l':
                    return inputMatrix[y - 1][x + 3];
                case 'm':
                    return inputMatrix[y][x - 2];
                case 'n':
                    return inputMatrix[y][x - 1];
                case 'o':
                    return inputMatrix[y][x];
                case 'p':
                    return inputMatrix[y][x + 1];
                case 'q':
                    return inputMatrix[y + 1][x - 2];
                case 'r':
                    return inputMatrix[y + 1][x - 1];
                case 's':
                    return inputMatrix[y + 1][x];
                case 't':
                    return inputMatrix[y + 1][x + 1];
                default:
                    throw new NotSupportedException("Critical error: condition not found.");
            }
        }

        public static Image SpaghettiCCL(Image input) {
            return CCL(input, SpaghettiAssignLabels);
        }

        public static Image ClassicCCL(Image input) {
            return CCL(input, ClassicCCL_AssignLabels);
        }

        private static Image CCL(Image input, Func<Image, (Image, Dictionary<int, HashSet<int>>)> assignLabels) {
            (Image output, Dictionary<int, HashSet<int>> equivalentLabels) = assignLabels(input);
            ResolveLabelEquivalencies(output, equivalentLabels);
            return output;
        }

        private static (Image, Dictionary<int, HashSet<int>>) ClassicCCL_AssignLabels(Image input) {
            List<List<int>> inputMatrix = input.GetMatrix();
            int width = inputMatrix[0].Count;
            int height = inputMatrix.Count;
            Image output = new Image(InitMatrixWithZeroes(width, height));
            Dictionary<int, HashSet<int>> equivalentLabels = new Dictionary<int, HashSet<int>>();
            int highestLabel = 0;
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    if (inputMatrix[y][x] == 1) {
                        // Assign label if current pixel is foreground and track equivalent labels
                        highestLabel = ClassicCCL_LabelPixel(output, x, y, highestLabel, equivalentLabels);
                    }
                }
            }

            return (output, equivalentLabels);
        }

        private static int ClassicCCL_LabelPixel(Image image, int x, int y, int highestLabel, Dictionary<int, HashSet<int>> equivalentLabels) {
            List<List<int>> imageMatrix = image.GetMatrix();
            int width = imageMatrix[0].Count;
            HashSet<int> neighboringLabels = new HashSet<int>(); 
            if (x - 1 >= 0 && y - 1 >= 0 && imageMatrix[y - 1][x - 1] != 0) {
                neighboringLabels.Add(imageMatrix[y - 1][x - 1]);
            } 
            if (y - 1 >= 0 && imageMatrix[y - 1][x] != 0) {
                neighboringLabels.Add(imageMatrix[y - 1][x]);
            } 
            if (x + 1 < width && y - 1 >= 0 && imageMatrix[y - 1][x + 1] != 0) {
                neighboringLabels.Add(imageMatrix[y - 1][x + 1]);
            } 
            if (x - 1 >= 0 && imageMatrix[y][x - 1] != 0) {
                neighboringLabels.Add(imageMatrix[y][x - 1]);
            }

            //Console.WriteLine("x = {0}, y = {1}, neighboringLabels.Count = {2}", x, y, neighboringLabels.Count);

            if (neighboringLabels.Count == 0) {
                // New label
                highestLabel++;
                imageMatrix[y][x] = highestLabel;
                equivalentLabels[highestLabel] = new HashSet<int> {highestLabel};
                //Console.WriteLine("Label {0} is created at x|y {1}|{2}", highestLabel, x, y);
            } else if (neighboringLabels.Count == 1) {
                // Assign label
                imageMatrix[y][x] = GetAnyHashsetElement(neighboringLabels);
            } else {
                // Merge labels
                //Console.WriteLine("x = {0}, y = {1}, neighboringLabels.Count = {2}", x, y, neighboringLabels.Count);
                List<int> labels = new List<int>(neighboringLabels);
                imageMatrix[y][x] = labels[0];
                for (int i = 1; i < labels.Count; i++) {
                    //equivalentLabels.Add((labels[0], labels[i]));
                    equivalentLabels[labels[0]].Add(labels[i]);
                    equivalentLabels[labels[i]].Add(labels[0]);
                }
            }

            return highestLabel;
        }

        private static void ResolveLabelEquivalencies(Image image, Dictionary<int, HashSet<int>> equivalentLabels) {
            // Initialize a lookup table for equivalent labels
            List<int> lookupTable = new List<int>();
            for (int i = 0; i < equivalentLabels.Count + 1; i++) {
                lookupTable.Add(0);
            }

            // Calculate transitive closure of equivalent labels and fill up lookupTable
            foreach(KeyValuePair<int, HashSet<int>> item in equivalentLabels) {
                // All equivalent labels will be relabeled to mainLabel
                int mainLabel = item.Key;

                // Equivalent labels associated with mainLabel are pushed into a stack
                Stack<int> labelStack = new Stack<int>();
                foreach (int otherLabel in item.Value) {
                    labelStack.Push(otherLabel);
                } 

                // Labels are popped from the stack one by one and if they aren't in the lookup table yet,
                // they are added and their associated equivalent labels are pushed into the stack.
                while (labelStack.Count != 0) {
                    int curLabel = labelStack.Pop();
                    //Console.WriteLine("Current label = {0}", curLabel);
                    if (lookupTable[curLabel] == 0) {
                        lookupTable[curLabel] = mainLabel;
                        HashSet<int> associatedSet = equivalentLabels[curLabel];
                        foreach (int otherLabel in associatedSet) {
                            labelStack.Push(otherLabel);
                        }
                    }
                }
            } 

            // Relabel the image
            RelabelImage(image, lookupTable);
        }

        private static void RelabelImage(Image image, List<int> lookupTable) {
            // Replaces preliminary labels with final labels
            List<List<int>> imageMatrix = image.GetMatrix();
            int width = imageMatrix[0].Count;
            int height = imageMatrix.Count;
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    if (imageMatrix[y][x] != 0) {
                        imageMatrix[y][x] = lookupTable[imageMatrix[y][x]];
                    }
                }
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

        public static Image FloodFillCCL(Image input) {
            // Labels an input image using bfs flood fill
            List<List<int>> inputMatrix = input.GetMatrix();
            int width = inputMatrix[0].Count;
            int height = inputMatrix.Count;
            List<List<int>> outputMatrix = InitMatrixWithZeroes(width, height);
            int highestLabel = 0;
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    // If a pixel is foreground and not labeled yet, label it and all 
                    // connected foreground pixels
                    if (inputMatrix[y][x] != 0 && outputMatrix[y][x] == 0) {
                        highestLabel++;
                        Queue<(int, int)> q = new Queue<(int, int)>();
                        q.Enqueue((y, x));
                        while (q.Count != 0) {
                            (int yFlood, int xFlood) = q.Dequeue();
                            if (xFlood >= 0 && xFlood < width && yFlood >= 0 && yFlood < height &&
                                    inputMatrix[yFlood][xFlood] != 0 && outputMatrix[yFlood][xFlood] == 0) {
                                outputMatrix[yFlood][xFlood] = highestLabel;
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
            return new Image(outputMatrix);
        }

        private static Image NormalizeLabels(Image input) {
            /* Used purely for testing, this method replaces labels in a labeled image such that
            when iterating through pixels from top left to bottom right, the first encountered 
            label is 1, second 2, third 3 etc.
            */

            // Implementation is similar to flood fill CCL
            List<List<int>> inputMatrix = input.GetMatrix();
            int width = inputMatrix[0].Count;
            int height = inputMatrix.Count;
            List<List<int>> outputMatrix = InitMatrixWithZeroes(width, height);
            int highestOutputLabel = 0;
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    if (inputMatrix[y][x] != 0 && outputMatrix[y][x] == 0) {
                        highestOutputLabel++;
                        int curInputLabel = inputMatrix[y][x];
                        Queue<(int, int)> q = new Queue<(int, int)>();
                        q.Enqueue((y, x));
                        while (q.Count != 0) {
                            (int yFlood, int xFlood) = q.Dequeue();
                            if (xFlood >= 0 && xFlood < width && yFlood >= 0 && yFlood < height &&
                                    inputMatrix[yFlood][xFlood] == curInputLabel && outputMatrix[yFlood][xFlood] == 0) {
                                outputMatrix[yFlood][xFlood] = highestOutputLabel;
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
            return new Image(outputMatrix);
        }

        public static class Tests 
        {
            public static void Run() {
                TestClassicCCL();
                TestSpaghettiCCL();
                TestFloodFillCCL();
            }
            
            private static void TestLabeledImageEquivalency(Image image1, Image image2) {
                // Asserts equivalency of two labeled images by first normalizing them,
                // then asserting their equality
                Image normalized1 = NormalizeLabels(image1);
                Image normalized2 = NormalizeLabels(image2);
                Debug.Assert(normalized1.Equals(normalized2));
            }

            private static void TestClassicCCL() {
                Image labeled1 = NormalizeLabels(ClassicCCL(Image.TestImages.BinaryImage1()));
                Image reference1 = Image.TestImages.LabeledImage1();
                Debug.Assert(labeled1.Equals(reference1));

                Image labeled2 = NormalizeLabels(ClassicCCL(Image.TestImages.BinaryImage2()));
                Image reference2 = Image.TestImages.LabeledImage2();
                Debug.Assert(labeled2.Equals(reference2));

                Image labeled3 = NormalizeLabels(ClassicCCL(Image.TestImages.BinaryImage3()));
                Image reference3 = Image.TestImages.LabeledImage3();
                Debug.Assert(labeled3.Equals(reference3));
            }

            private static void TestSpaghettiCCL() {
                Image spaghetti3 = SpaghettiCCL(Image.TestImages.BinaryImage3());
                Image classic3 = ClassicCCL(Image.TestImages.BinaryImage3());
                TestLabeledImageEquivalency(spaghetti3, classic3);

                Image spaghetti4 = SpaghettiCCL(Image.TestImages.BinaryImage4());
                Image classic4 = ClassicCCL(Image.TestImages.BinaryImage4());
                TestLabeledImageEquivalency(spaghetti4, classic4);

                Image spaghetti5 = SpaghettiCCL(Image.TestImages.BinaryImage5());
                Image classic5 = ClassicCCL(Image.TestImages.BinaryImage5());
                TestLabeledImageEquivalency(spaghetti5, classic5);

                Image spaghetti6 = SpaghettiCCL(Image.TestImages.BinaryImage6());
                Image classic6 = ClassicCCL(Image.TestImages.BinaryImage6());
                TestLabeledImageEquivalency(spaghetti6, classic6);

                Image spaghetti7 = SpaghettiCCL(Image.TestImages.BinaryImage7());
                Image classic7 = ClassicCCL(Image.TestImages.BinaryImage7());
                TestLabeledImageEquivalency(spaghetti7, classic7);

                Image spaghetti8 = SpaghettiCCL(Image.TestImages.BinaryImage8());
                Image classic8 = ClassicCCL(Image.TestImages.BinaryImage8());
                TestLabeledImageEquivalency(spaghetti8, classic8);

                Image spaghetti9 = SpaghettiCCL(Image.TestImages.BinaryImage9());
                Image classic9 = ClassicCCL(Image.TestImages.BinaryImage9());
                TestLabeledImageEquivalency(spaghetti9, classic9);

                Image spaghetti10 = SpaghettiCCL(Image.TestImages.BinaryImage10());
                Image classic10 = ClassicCCL(Image.TestImages.BinaryImage10());
                TestLabeledImageEquivalency(spaghetti10, classic10);

                Image spaghetti11 = SpaghettiCCL(Image.TestImages.BinaryImage11());
                Image classic11 = ClassicCCL(Image.TestImages.BinaryImage11());
                TestLabeledImageEquivalency(spaghetti11, classic11);

                Image spaghetti12 = SpaghettiCCL(Image.TestImages.BinaryImage12());
                Image classic12 = ClassicCCL(Image.TestImages.BinaryImage12());
                TestLabeledImageEquivalency(spaghetti12, classic12);

                Image spaghetti13 = SpaghettiCCL(Image.TestImages.BinaryImage13());
                Image classic13 = ClassicCCL(Image.TestImages.BinaryImage13());
                TestLabeledImageEquivalency(spaghetti13, classic13);

                Image spaghetti14 = SpaghettiCCL(Image.TestImages.BinaryImage14());
                Image classic14 = ClassicCCL(Image.TestImages.BinaryImage14());
                TestLabeledImageEquivalency(spaghetti14, classic14);

                Image spaghetti15 = SpaghettiCCL(Image.TestImages.BinaryImage15());
                Image classic15 = ClassicCCL(Image.TestImages.BinaryImage15());
                TestLabeledImageEquivalency(spaghetti15, classic15);

                Image spaghetti16 = SpaghettiCCL(Image.TestImages.BinaryImage16());
                Image classic16 = ClassicCCL(Image.TestImages.BinaryImage16());
                TestLabeledImageEquivalency(spaghetti16, classic16);

                Image spaghetti17 = SpaghettiCCL(Image.TestImages.BinaryImage17());
                Image classic17 = ClassicCCL(Image.TestImages.BinaryImage17());
                TestLabeledImageEquivalency(spaghetti17, classic17);

                Image spaghetti18 = SpaghettiCCL(Image.TestImages.BinaryImage18());
                Image classic18 = ClassicCCL(Image.TestImages.BinaryImage18());
                TestLabeledImageEquivalency(spaghetti18, classic18);

                
                for (int i = 1; i < 10; i++) {
                    //Image randomImageEven = Image.TestImages.GenerateRandomImage(20, 19, fileName: "testEven" + i);
                    Image randomImageEven = Image.TestImages.GenerateRandomImage(20, 19);
                    Image spaghettiRandomEven = SpaghettiCCL(randomImageEven);
                    Image classicRandomEven = ClassicCCL(randomImageEven);
                    TestLabeledImageEquivalency(spaghettiRandomEven, classicRandomEven);
                    Console.WriteLine("Random image with even number of columns passed tests. ({0})", i);
                }

                for (int i = 1; i < 10; i++) {
                    //Image randomImageOdd = Image.TestImages.GenerateRandomImage(21, 19, fileName: "testOdd" + i);
                    Image randomImageOdd = Image.TestImages.GenerateRandomImage(21, 19);
                    Image spaghettiRandomOdd = SpaghettiCCL(randomImageOdd);
                    Image classicRandomOdd = ClassicCCL(randomImageOdd);
                    TestLabeledImageEquivalency(spaghettiRandomOdd, classicRandomOdd);
                    Console.WriteLine("Random image with odd number of columns passed tests. ({0})", i);
                }
                
            } 

            private static void TestFloodFillCCL() {
                for (int i = 1; i < 10; i++) {
                    Image randomImage = Image.TestImages.GenerateRandomImage(20, 20);
                    Image classicRandom = ClassicCCL(randomImage);
                    Image floodFillRandom = FloodFillCCL(randomImage);
                    TestLabeledImageEquivalency(classicRandom, floodFillRandom);
                    Console.WriteLine("Flood fill test passed on a random image. ({0})", i);
                }
            }
        }
    }
}