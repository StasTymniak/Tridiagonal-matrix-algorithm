using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tridiagonal_matrix_algorithm
{
    class Program
    {


        static double[] TriadiagonalMatrixAlgo(double[] A,double[] B, double[] C, double[] Y,List<double> F,int n)
        {
            double[] ksi = new double[n-1];
            double[] eta = new double[n-1];
            ksi[n - 2] = (-1) * A[n - 2] / C[n - 1];
            eta[n - 2] = F[n-1] / C[n - 1];
            for(int i = n - 2; i >= 1; i--)
            {
                double dillenn = (C[i] + (B[i] * ksi[i]));
                ksi[i-1] = ((-1) * A[i-1]) /dillenn ;

                eta[i-1] = (F[i] - (B[i] * eta[i])) / (C[i]+(B[i]*ksi[i]));
            }
            Y[0] = (F[0] - B[0] * eta[1]) / (C[0] + B[0] * ksi[1]);
            for(int i = 0; i < n-1; i++)
            {
                Y[i+1]=ksi[i]*Y[i] + eta[i];
            }
            return Y;
        }

        static void PrintVector(double[] vector)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                Console.Write($"{vector[i]} ");
            }
        }

        static double VectorNorm1(double[] vector)
        {
            double norm = 0;
            for (int i = 0; i < vector.Length; i++)
            {
                vector[i] = Math.Abs(vector[i]);
                norm = vector.Max();
            }
            return norm;
        }

        static double VectorNorm2(double[] vector)
        {
            double norm = 0;
            for (int i = 0; i < vector.Length; i++)
            {
                vector[i] = Math.Abs(vector[i]);
                norm += vector[i];
            }
            return norm;
        }
        static void Main(string[] args)
        {

            int n,userInput;
            double h;
            bool isTrue = true;
            Console.WriteLine(@"Enter:
1 to solve your matrix
2 to create matrix by algorithm ");
            userInput = Convert.ToInt32(Console.ReadLine());
            
            List<double> temp = new List<double>();
            switch (userInput)
            {
                case 1:
                    Console.WriteLine("Enter n (Length of X-vector): ");
                    n = Convert.ToInt32(Console.ReadLine());
                    int count_corr = 0;
                    
                    double[] A = new double[n-1];
                    double[] B = new double[n-1];
                    double[] C = new double[n];
                    double[] Y = new double[n];
                    List<double> F = new List<double>();
                    //Read Matrix
                    using (StreamReader sr = new StreamReader("Matrix2.txt"))
                    {


                        for (int i = 0; i <= n; i++)
                        {
                            string temp_str = sr.ReadLine();

                            if (i == 0)
                            {
                                for (int j = 0; j < temp_str.Split(' ').Count(); j++)
                                {
                                    A[j] = Convert.ToDouble(temp_str.Split(' ')[j]);
                                }
                            }

                            if (i == 1)
                            {
                                for (int j = 0; j < temp_str.Split(' ').Count(); j++)
                                {
                                    B[j] = Convert.ToDouble(temp_str.Split(' ')[j]);
                                }
                            }
                            if (i == 2)
                            {
                                for (int j = 0; j < temp_str.Split(' ').Count(); j++)
                                {
                                    C[j] = Convert.ToDouble(temp_str.Split(' ')[j]);
                                }
                            }
                            if (i == 3)
                            {
                                for (int j = 0; j < temp_str.Split(' ').Count(); j++)
                                {
                                    F.Add(Convert.ToDouble(temp_str.Split(' ')[j]));
                                }
                            }
                        }
                    }
                    //Cheak if matrix is correct
                    if (Math.Abs(C[0]) <= Math.Abs(B[0]))
                        throw new Exception("Matrix in not in correct form ");
                    if (Math.Abs(C[0]) > Math.Abs(B[0]))
                        count_corr++;

                    if (Math.Abs(C[n - 1]) > Math.Abs(A[n - 2]))
                        count_corr++;
                    if (Math.Abs(C[n - 1]) <= Math.Abs(A[n - 2]))
                        throw new Exception("Matrix in not in correct form ");

                    bool is_bigger = true;
                    for (int i = 1; i < n-2; i++)
                    {
                        if (Math.Abs(C[i]) <= (Math.Abs(A[i])+Math.Abs(B[i])))
                            throw new Exception("Matrix in not in correct form ");
                        if (Math.Abs(C[i]) > (Math.Abs(A[i]) + Math.Abs(B[i])))
                            is_bigger = true;
                        else
                            is_bigger = false;

                    }
                    if (is_bigger)
                        count_corr++;
                    for (int i = 0; i < n - 1; i++)
                    {
                        if (Math.Abs(C[i]) < 0)
                            throw new Exception("Matrix in not in correct form ");
                    }
                    for (int i = 1; i < n - 2; i++)
                    {
                        if (Math.Abs(A[i]) < 0)
                            throw new Exception("Matrix in not in correct form ");
                    }
                    for (int i = 0; i < n - 3; i++)
                    {
                        if (Math.Abs(B[i]) < 0)
                            throw new Exception("Matrix in not in correct form ");
                    }

                    Console.WriteLine("A: ");
                    PrintVector(A);
                    Console.WriteLine("\nB: ");
                    PrintVector(B);
                    Console.WriteLine("\nC: ");
                    PrintVector(C);
                    Console.WriteLine("\nF: ");
                    for(int i=0; i < n; i++) { Console.Write($"{F[i]} "); }

                    Y = TriadiagonalMatrixAlgo(A, B, C, Y, F, n);

                    Console.WriteLine("\nY: ");
                    PrintVector(Y);

                    temp.Add(C[0] * Y[0] + B[0] * Y[1]);
                    //Cheak res
                    for (int i = 2; i <= n; i++)
                    {
                        double temp_res = 0;
                        if (i != n)
                        {
                            temp_res = A[i-2] * Y[i - 2] + C[i-1] * Y[i-1] + B[i-2] * Y[i];
                            temp.Add(temp_res);
                        }

                        if (i == n)
                        {
                            temp_res = A[n-2] * Y[n - 2] + C[n-1] * Y[n-1];
                            temp.Add(temp_res);
                        }
                    }
                    for(int i = 0; i < n; i++)
                    {
                        if (temp[i] == F[i])
                            isTrue = true;
                        else
                            isTrue = false;
                    }
                    Console.WriteLine();
                    if (isTrue)
                    {
                        if (count_corr > 0)
                            Console.WriteLine("Solve is correct");
                        else
                            Console.WriteLine("Error");
                    }
                    
                    break;
                case 2:
                    Console.WriteLine("Enter n: ");
                    n = Convert.ToInt32(Console.ReadLine());

                    double[] A_algo = new double[n];
                    double[] B_algo = new double[n];
                    double[] C_algo = new double[n+1];
                    double[] Y_algo = new double[n+1];
                    List<double> F_algo = new List<double>();
                    for(int i = 0; i <= n; i++)
                    {
                        h = 1.0 / n;

                        if (i == 0)
                        {
                            A_algo[0] = 1;
                            B_algo[0] = 0;
                            C_algo[0] = 1;
                            F_algo.Add(1);
                        }
                        if(i<n && i > 0)
                        {
                            A_algo[i] = 1;
                            B_algo[i] = 1;
                            C_algo[i] = (-2) - (1 + i * h) * Math.Pow(h, 2);
                            F_algo.Add(Math.Pow(h, 2) * (4 - (1 + i * h) * ((2 * Math.Pow(i, 2) * Math.Pow(h, 2) + 1))));
                        }
                        if (i == n)
                        {
                            A_algo[i - 1] = 0;
                            C_algo[i] = 1;
                            F_algo.Add(3);
                        }

                    }
                    Console.Write("A: ");
                    PrintVector(A_algo);
                    Console.Write("\nB: ");
                    PrintVector(B_algo);
                    Console.Write("\nC: ");
                    PrintVector(C_algo);
                    Console.Write("\nF: ");
                    for (int i = 0; i <= n; i++)
                    {
                        Console.Write($"{F_algo[i]} ");
                    }

                    Y_algo = TriadiagonalMatrixAlgo(A_algo, B_algo, C_algo, Y_algo,F_algo, n+1);
                    Console.Write("\nY: ");
                    PrintVector(Y_algo);
                    double[] YY_vector = new double[n + 1];
                    double[] Vector_norm = new double[n + 1];
                    h = 1.0 / n;
                    for (int i = 0; i <= n; i++)
                    {        
                        YY_vector[i] = (2 * Math.Pow(i, 2) * Math.Pow(h, 2) + 1);
                    }
                    for (int i = 0; i <= n; i++)
                    {
                        Vector_norm[i] = Y_algo[i] - YY_vector[i];
                    }

                    Console.WriteLine($"\n||y-y*|| = {VectorNorm1(Vector_norm)}");
                    Console.WriteLine($"\n||y-y*|| = {VectorNorm2(Vector_norm)}");
                    break;
                default:
                    Console.WriteLine("GGNB");
                    break;
            }        
        }
    }
}



