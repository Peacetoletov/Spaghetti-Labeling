using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Spaghetti_Labeling
{
    public static class ImageProcessor
    {
        private static Image SpaghettiAssignLabels(Image input, List<HashSet<int>> equivalentLabels) {
            List<List<int>> inputMatrix = input.GetMatrix();

            GraphManager gmFirst = new GraphManager(GraphManager.GraphType.FirstRow);
            GraphManager gmMiddle = new GraphManager(GraphManager.GraphType.MiddleRows);
            GraphManager gmLast = new GraphManager(GraphManager.GraphType.LastRow);
            // ^ For better performance, GraphManager could be created once at the start of the program and then 
            // passed as an argument

            int width = inputMatrix[0].Count;
            int height = inputMatrix.Count;
            Image output = new Image(InitMatrixWithZeroes(width, height));
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

            /*
            Console.WriteLine("\nEach row contains a set of equivalent labels:");
            foreach (HashSet<int> labelSet in equivalentLabels) {
                foreach (int label in labelSet) {
                    Console.Write("{0} ", label);
                }
                Console.WriteLine();
            }
            */

            return output;
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

        private static Image CCL(Image input, Func<Image, List<HashSet<int>>, Image> assignLabels) {
            List<HashSet<int>> equivalentLabels = new List<HashSet<int>>();
            Image output = assignLabels(input, equivalentLabels);
            ResolveLabelEquivalencies(output, equivalentLabels);
            return output;
        }

        private static Image ClassicCCL_AssignLabels(Image input, List<HashSet<int>> equivalentLabels) {
            List<List<int>> inputMatrix = input.GetMatrix();
            int width = inputMatrix[0].Count;
            int height = inputMatrix.Count;
            Image image = new Image(InitMatrixWithZeroes(width, height));
            int highestLabel = 0;
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    if (inputMatrix[y][x] == 1) {
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
            // Note that the only pixels which can theoretically be equivalent are the 'r' pixel and
            // either the 'p' or 's' pixel.
            ClassicCCL_ManageEquivalencies(image, 'p', x, y, equivalentLabels);
            ClassicCCL_ManageEquivalencies(image, 's', x, y, equivalentLabels);
        }

        private static void ClassicCCL_ManageEquivalencies(Image image, char pixel, int x, int y, 
                                                           List<HashSet<int>> equivalentLabels) {
            // Checks if the 'r' pixel and the pixel passed as argument ('p' or 's') are equivalent,
            // and if so, updates the equivalentLabels list
            
            // Return if either of the relevant pixels is out of bounds
            List<List<int>> imageMatrix = image.GetMatrix();
            int width = imageMatrix[0].Count;
            if (x == 0 || y == 0 || x == width - 1) {
                return;
            }

            // Return if either of the relevant pixels is background, or if they both have the same label
            Debug.Assert(pixel == 'p' || pixel == 's');
            int label1 = imageMatrix[y - 1][x + 1];
            int label2 = pixel == 'p' ? imageMatrix[y - 1][x - 1] : imageMatrix[y][x - 1];
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

        public static (HashSet<int>, HashSet<int>) FindSetsWithLabels(List<HashSet<int>> equivalentLabels, int label1, int label2) {
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

            int[] setIndexToLabel = new int[equivalentLabels.Count];
            int currentHighestFinalLabel = 0;

            List<List<int>> imageMatrix = image.GetMatrix();
            int width = imageMatrix[0].Count;
            int height = imageMatrix.Count;
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    if (imageMatrix[y][x] == 0) {
                        // Skip background pixels
                        continue;
                    }
                    int setIndex = GetIndexOfSetWithLabel(imageMatrix[y][x], equivalentLabels);
                    if (setIndexToLabel[setIndex] == 0) {
                        // This set of equivalent labels wasn't used yet, needs to be added to the map
                        currentHighestFinalLabel++;
                        setIndexToLabel[setIndex] = currentHighestFinalLabel;
                    }
                    imageMatrix[y][x] = setIndexToLabel[setIndex];
                }
            }
        }

        private static int GetIndexOfSetWithLabel(int label, List<HashSet<int>> equivalentLabels) {
            for (int i = 0; i < equivalentLabels.Count; i++) {
                if (equivalentLabels[i].Contains(label)) {
                    return i;
                }
            }
            throw new NotSupportedException("Critical error: label " + label + " not found in equivalentLabels.");
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

        public static class Tests 
        {
            public static void Run() {
                TestClassicCCL();
                TestSpaghettiCCL();
                TestFloodFillCCL();
            }

            private static void TestClassicCCL() {
                Image labeled1 = ClassicCCL(Image.TestImages.BinaryImage1());
                Image reference1 = Image.TestImages.LabeledImage1();
                Debug.Assert(labeled1.Equals(reference1));

                Image labeled2 = ClassicCCL(Image.TestImages.BinaryImage2());
                Image reference2 = Image.TestImages.LabeledImage2();
                Debug.Assert(labeled2.Equals(reference2));

                Image labeled3 = ClassicCCL(Image.TestImages.BinaryImage3());
                Image reference3 = Image.TestImages.LabeledImage3();
                Debug.Assert(labeled3.Equals(reference3));
            }

            private static void TestSpaghettiCCL() {
                Image spaghetti3 = SpaghettiCCL(Image.TestImages.BinaryImage3());
                Image classic3 = ClassicCCL(Image.TestImages.BinaryImage3());
                Debug.Assert(spaghetti3.Equals(classic3));

                Image spaghetti4 = SpaghettiCCL(Image.TestImages.BinaryImage4());
                Image classic4 = ClassicCCL(Image.TestImages.BinaryImage4());
                Debug.Assert(spaghetti4.Equals(classic4));

                Image spaghetti5 = SpaghettiCCL(Image.TestImages.BinaryImage5());
                Image classic5 = ClassicCCL(Image.TestImages.BinaryImage5());
                Debug.Assert(spaghetti5.Equals(classic5));

                Image spaghetti6 = SpaghettiCCL(Image.TestImages.BinaryImage6());
                Image classic6 = ClassicCCL(Image.TestImages.BinaryImage6());
                Debug.Assert(spaghetti6.Equals(classic6));

                Image spaghetti7 = SpaghettiCCL(Image.TestImages.BinaryImage7());
                Image classic7 = ClassicCCL(Image.TestImages.BinaryImage7());
                Debug.Assert(spaghetti7.Equals(classic7));

                Image spaghetti8 = SpaghettiCCL(Image.TestImages.BinaryImage8());
                Image classic8 = ClassicCCL(Image.TestImages.BinaryImage8());
                Debug.Assert(spaghetti8.Equals(classic8));

                Image spaghetti9 = SpaghettiCCL(Image.TestImages.BinaryImage9());
                Image classic9 = ClassicCCL(Image.TestImages.BinaryImage9());
                Debug.Assert(spaghetti9.Equals(classic9));

                Image spaghetti10 = SpaghettiCCL(Image.TestImages.BinaryImage10());
                Image classic10 = ClassicCCL(Image.TestImages.BinaryImage10());
                Debug.Assert(spaghetti10.Equals(classic10));

                Image spaghetti11 = SpaghettiCCL(Image.TestImages.BinaryImage11());
                Image classic11 = ClassicCCL(Image.TestImages.BinaryImage11());
                Debug.Assert(spaghetti11.Equals(classic11));

                Image spaghetti12 = SpaghettiCCL(Image.TestImages.BinaryImage12());
                Image classic12 = ClassicCCL(Image.TestImages.BinaryImage12());
                Debug.Assert(spaghetti12.Equals(classic12));

                Image spaghetti13 = SpaghettiCCL(Image.TestImages.BinaryImage13());
                Image classic13 = ClassicCCL(Image.TestImages.BinaryImage13());
                Debug.Assert(spaghetti13.Equals(classic13));

                Image spaghetti14 = SpaghettiCCL(Image.TestImages.BinaryImage14());
                Image classic14 = ClassicCCL(Image.TestImages.BinaryImage14());
                Debug.Assert(spaghetti14.Equals(classic14));

                Image spaghetti15 = SpaghettiCCL(Image.TestImages.BinaryImage15());
                Image classic15 = ClassicCCL(Image.TestImages.BinaryImage15());
                Debug.Assert(spaghetti15.Equals(classic15));

                Image spaghetti16 = SpaghettiCCL(Image.TestImages.BinaryImage16());
                Image classic16 = ClassicCCL(Image.TestImages.BinaryImage16());
                Debug.Assert(spaghetti16.Equals(classic16));

                Image spaghetti17 = SpaghettiCCL(Image.TestImages.BinaryImage17());
                Image classic17 = ClassicCCL(Image.TestImages.BinaryImage17());
                Debug.Assert(spaghetti17.Equals(classic17));

                Image spaghetti18 = SpaghettiCCL(Image.TestImages.BinaryImage18());
                Image classic18 = ClassicCCL(Image.TestImages.BinaryImage18());
                Debug.Assert(spaghetti18.Equals(classic18));

                
                for (int i = 1; i < 10; i++) {
                    //Image randomImageEven = Image.TestImages.GenerateRandomImage(20, 19, fileName: "testEven" + i);
                    Image randomImageEven = Image.TestImages.GenerateRandomImage(20, 19);
                    Image spaghettiRandomEven = SpaghettiCCL(randomImageEven);
                    Image classicRandomEven = ClassicCCL(randomImageEven);
                    Debug.Assert(spaghettiRandomEven.Equals(classicRandomEven));
                    Console.WriteLine("Random image with even number of columns passed tests. ({0})", i);
                }

                for (int i = 1; i < 10; i++) {
                    //Image randomImageOdd = Image.TestImages.GenerateRandomImage(21, 19, fileName: "testOdd" + i);
                    Image randomImageOdd = Image.TestImages.GenerateRandomImage(21, 19);
                    Image spaghettiRandomOdd = SpaghettiCCL(randomImageOdd);
                    Image classicRandomOdd = ClassicCCL(randomImageOdd);
                    Debug.Assert(spaghettiRandomOdd.Equals(classicRandomOdd));
                    Console.WriteLine("Random image with odd number of columns passed tests. ({0})", i);
                }
                
            } 
        }

        private static void TestFloodFillCCL() {
            for (int i = 1; i < 10; i++) {
                Image randomImage = Image.TestImages.GenerateRandomImage(20, 20);
                Image classicRandom = ClassicCCL(randomImage);
                Image floodFillRandom = ClassicCCL(randomImage);
                Debug.Assert(floodFillRandom.Equals(classicRandom));
                Console.WriteLine("Flood fill test passed on a random image. ({0})", i);
            }
        }
    }
}