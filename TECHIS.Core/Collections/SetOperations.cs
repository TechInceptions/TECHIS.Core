//#region Using directives

//using System;
//using System.Collections.Generic;
//using System.Text;

//#endregion

//namespace TECHIS.Core.Collections
//{
//    public sealed class SetOperations
//    {
//        private SetOperations()
//        {

//        }

//        public static System.Collections.ArrayList ElementsInAandNotInB(System.Collections.ArrayList setA, System.Collections.ArrayList setB)
//        {
//            System.Collections.ArrayList setIntersection = SetIntersection(setA, setB);
//            return Elements_Of_A_Not_In_B(setA, setIntersection);
//        }

//        public static System.Collections.ArrayList Elements_Of_A_Not_In_B(System.Collections.ArrayList setA, System.Collections.ArrayList setB)
//        {

//            int SetACount = setA.Count;

//            bool found = false;

//            System.Collections.ArrayList ResultSet = new System.Collections.ArrayList(setA.Count);

//            for (int x = 0; x < SetACount; x++)
//            {
//                found = false;
//                foreach (object B in setB)
//                {
//                    if (setA[x].ToString().Equals(B.ToString()))
//                    {
//                        found = true;
//                        break;
//                    }
//                }

//                if (!found)
//                    ResultSet.Add(setA[x]);
//            }

//            return ResultSet;
//        }

//        public static System.Collections.ArrayList SetIntersection(System.Collections.ArrayList setA, System.Collections.ArrayList setB)
//        {
//            System.Collections.ArrayList ResultSet = new System.Collections.ArrayList(setA.Count);

//            foreach (object A in setA)
//            {
//                foreach (object B in setB)
//                {
//                    if (A.ToString().Equals(B.ToString()))
//                    {
//                        ResultSet.Add(B);
//                    }
//                }
//            }

//            return ResultSet;
//        }


//        public static System.Collections.ArrayList ElementsInAandNotInB(System.Collections.IEnumerable setA, System.Collections.IEnumerable setB)
//        {
//            System.Collections.ArrayList setIntersection = SetIntersection(setA, setB);
//            return Elements_Of_A_Not_In_B(setA, setIntersection);
//        }

//        public static System.Collections.ArrayList Elements_Of_A_Not_In_B(System.Collections.IEnumerable setA, System.Collections.IEnumerable setB)
//        {
//            bool found = false;

//            System.Collections.ArrayList ResultSet = new System.Collections.ArrayList(100);

//            foreach (object A in setA)
//            {
//                found = false;
//                foreach (object B in setB)
//                {
//                    if (A.ToString().Equals(B.ToString()))
//                    {
//                        found = true;
//                        break;
//                    }
//                }

//                if (!found)
//                    ResultSet.Add(A);
//            }

//            return ResultSet;
//        }

//        public static System.Collections.ArrayList SetIntersection(System.Collections.IEnumerable setA, System.Collections.IEnumerable setB)
//        {
//            System.Collections.ArrayList ResultSet = new System.Collections.ArrayList(100);

//            foreach (object A in setA)
//            {
//                foreach (object B in setB)
//                {
//                    if (A.ToString().Equals(B.ToString()))
//                    {
//                        ResultSet.Add(B);
//                    }
//                }
//            }

//            return ResultSet;
//        }


//        public static int[] ElementsInAandNotInB(int[] setA, int[] setB)
//        {
//            int[] setIntersection = SetIntersection(setA, setB);
//            return Elements_Of_A_Not_In_B(setA, setIntersection);
//        }

//        public static int[] Elements_Of_A_Not_In_B(int[] setA, int[] setB)
//        {
//            bool found = false;

//            List<int> ResultSet = new List<int>(setA.Length);

//            foreach (int A in setA)
//            {
//                found = false;
//                foreach (int B in setB)
//                {
//                    if (A.Equals(B))
//                    {
//                        found = true;
//                        break;
//                    }
//                }

//                if (!found)
//                    ResultSet.Add(A);
//            }

//            int[] result = new int[ResultSet.Count];
//            ResultSet.CopyTo(result);

//            return result;
//        }

//        public static int[] SetIntersection(int[] setA, int[] setB)
//        {
//            List<int> ResultSet = new List<int>(setA.Length);

//            foreach (int A in setA)
//            {
//                foreach (int B in setB)
//                {
//                    if (A.Equals(B))
//                    {
//                        ResultSet.Add(B);
//                    }
//                }
//            }

//            int[] result = new int[ResultSet.Count];
//            ResultSet.CopyTo(result);

//            return result;
//        }

//    }
//}
