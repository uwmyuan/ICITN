using System;
using System.Collections.Generic;
using System.Text;

namespace ICITN
{
    class SUE
    {
        double[,] network;
        double[,] c;
        double[,] od;
        public SUE(double[,] net, double[,] capacity, double[,] odm) 
        {
            network = net;
            c = capacity;
            od = odm;
        }
        public double[,] traffic_assignment() 
        {
            int m = network.GetLength(0);
            int n = network.GetLength(1);
            //Console.WriteLine();
            //Console.WriteLine("Free stream:");
            //for (int i = 0; i < m; i++)
            //{
            //    Console.WriteLine();
            //    for (int j = 0; j < m; j++)
            //        Console.Write(network[i, j] + " ");
            //}
            //Console.WriteLine();
            //Console.WriteLine("Capacity:");
            //for (int i = 0; i < m; i++)
            //{
            //    Console.WriteLine();
            //    for (int j = 0; j < m; j++)
            //        Console.Write(c[i, j] + " ");
            //}
            //Console.WriteLine();
            //Console.WriteLine("OD:");
            //for (int i = 0; i < m; i++)
            //{
            //    Console.WriteLine();
            //    for (int j = 0; j < m; j++)
            //        Console.Write(od[i, j] + " ");
            //}

            //变量定义

            int[,] p = new int[m, n];
            int[,] odc = new int[m, n];//OD矩阵的邻接矩阵
            int[,] path = new int[1500, 200];//最短路标号存储矩阵
            List<int[,]> pathlist = new List<int[,]>();
            double[,] x = new double[m, n];//记录路段流量矩阵
            double[,] x1 = new double[m, n];//本次迭代的最终结果
            double[,] x2 = new double[m, n];//上一次迭代的结果
            double[,] x3 = new double[m, n];//本次迭代的结果
            double beta = 1;//误差精度          
            double[,] g = new double[m, n];
            System.Array.Copy(network, g, network.Length);
            int count = 1;
            //初始分配
            for (int k = 0; k < m; k++)
                for (int j = 0; j < n; j++)
                    p[k, j] = k + 1;//最短路标号矩阵
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    {
                        if ((g[i, j] == 0) && (i != j))
                            g[i, j] = double.MaxValue;
                    }
                    if (od[i, j] != 0)////列出OD矩阵的邻接矩阵
                        odc[i, j] = 1;
                    else
                        odc[i, j] = 0;
                }
            }
            //Console.WriteLine();
            //Console.WriteLine("OD connection:");
            //for (int i = 0; i < m; i++)
            //{
            //    Console.WriteLine();
            //    for (int j = 0; j < m; j++)
            //        Console.Write(odc[i, j] + " ");
            //}
            //Console.WriteLine();
            //Console.WriteLine("initial shortest path index:");
            //for (int i = 0; i < m; i++)
            //{
            //    Console.WriteLine();
            //    for (int j = 0; j < m; j++)
            //        Console.Write(p[i, j] + " ");
            //}
            for (int k = 0; k < n; k++)
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                    {
                        if (g[i, j] > g[i, k] + g[k, j])
                        {
                            g[i, j] = g[i, k] + g[k, j];
                            p[i, j] = p[k, j];
                        }
                    }
            //Console.WriteLine();
            //Console.WriteLine("OD cost:");
            //for (int i = 0; i < m; i++)
            //{
            //    Console.WriteLine();
            //    for (int j = 0; j < m; j++)
            //        Console.Write(g[i, j] + " ");
            //}
            //Console.WriteLine();
            //Console.WriteLine("shortest path index:");
            //for (int i = 0; i < m; i++)
            //{
            //    Console.WriteLine();
            //    for (int j = 0; j < m; j++)
            //        Console.Write(p[i, j] + " ");
            //}
            //最短路路段列举
            int index = 0;
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                {
                    if (i != j)
                    {
                        int k = m;
                        int nn = 2;
                        path[index, 0] = i + 1;//起点标号
                        path[index, 1] = j + 1;//终点标号
                        path[index, nn] = j + 1;
                        while ((k >= 1) && (path[index, nn] != i + 1))
                        {
                            nn++;
                            path[index, nn] = p[i, path[index, nn - 1] - 1];
                            k--;
                        }
                        index++;
                    }
                }
            int len = path.GetLength(0);
            int[] lenpath = new int[len];
            for (int k = 0; k < len; k++)
            {
                int i = 0;
                for (int np = 2; np < m; np++)
                {
                    if (path[k, np] != 0)
                        i = i + 1; //求解每条最短路径的组成路段数
                }
                lenpath[k] = i + 2;
            }
            //Console.WriteLine();
            //for (int i = 0; i < 150; i++) {
            //    Console.WriteLine();
            //    for (int j = 0; j < 20; j++) Console.Write(path[i,j]+" ");
            //}
            //Console.WriteLine();
            //Console.WriteLine("count of links on each path:");
            //for (int k = 0; k < len; k++) Console.Write(lenpath[k] + " ");
            //UE分配过程
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                {
                    if ((i != j) && (od[i, j] != 0))
                    {
                        for (int k = 0; k < len; k++)
                        {
                            if ((path[k, 0] - 1 == i) && (path[k, 1] - 1 == j))
                            {
                                for (int l = lenpath[k] - 1; l >= 3; l--)
                                {
                                    if (path[k, l] != path[k, l - 1])
                                        x1[path[k, l] - 1, path[k, l - 1] - 1] += od[i, j];//全有全无分配过程
                                }
                            }
                        }
                    }
                }
            //Console.WriteLine();
            //Console.WriteLine("initial load:");
            //for (int i = 0; i < m; i++)
            //{
            //    Console.WriteLine();
            //    for (int j = 0; j < n; j++)
            //        Console.Write(x1[i, j] + " ");
            //}
            pathlist.Add(path);
            //迭代分配
            while (beta >= 0.01)
            {
                count++;
                g = new double[m, n];//阻抗矩阵
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (c[i, j] != 0)
                            g[i, j] = network[i, j] * (1 + 0.15 * Math.Pow(x1[i, j] / c[i, j], 4));//计算路段行程时间,BPR函数
                        else
                            g[i, j] = 0;
                    }
                }
                //最短路求解
                path = new int[1500, 200];//最短路标号存储矩阵清零
                for (int i = 0; i < m; i++)
                    for (int j = 0; j < n; j++)
                    {
                        if ((g[i, j] == 0) && (i != j))
                            g[i, j] = double.MaxValue;
                    }
                for (int k = 0; k < m; k++)
                    for (int j = 0; j < n; j++)
                        p[k, j] = k + 1;//最短路标号矩阵
                for (int k = 0; k < n; k++)
                    for (int i = 0; i < m; i++)
                        for (int j = 0; j < n; j++)
                            if (g[i, j] > g[i, k] + g[k, j])
                            {
                                g[i, j] = g[i, k] + g[k, j];
                                p[i, j] = p[k, j];
                            }
                //最短路路段列举
                index = 0;
                for (int i = 0; i < m; i++)
                    for (int j = 0; j < n; j++)
                    {
                        if (i != j)
                        {
                            int k = m;
                            int nn = 2;
                            path[index, 0] = i + 1;//起点标号
                            path[index, 1] = j + 1;//终点标号
                            path[index, nn] = j + 1;
                            while ((k >= 1) && (path[index, nn] != i + 1))
                            {
                                nn++;
                                path[index, nn] = p[i, path[index, nn - 1] - 1];
                                k--;
                            }
                            index++;
                        }
                    }
                len = path.GetLength(0);
                lenpath = new int[len];
                for (int k = 0; k < len; k++)
                {
                    int i = 0;
                    for (int np = 2; np < m; np++)
                    {
                        if (path[k, np] != 0)
                            i = i + 1; //求解每条最短路径的组成路段数
                    }
                    lenpath[k] = i + 2;
                }
                //Console.WriteLine();
                //Console.WriteLine("count of links on each path:");
                //for (int k = 0; k < len; k++) Console.Write(lenpath[k] + " ");
                //UE分配过程
                x2 = new double[m, n];
                for (int i = 0; i < m; i++)
                    for (int j = 0; j < n; j++)
                    {
                        if ((i != j) && (od[i, j] != 0))
                        {
                            for (int k = 0; k < len; k++)
                            {
                                if ((path[k, 0] - 1 == i) && (path[k, 1] - 1 == j))
                                {
                                    for (int l = lenpath[k] - 1; l >= 3; l--)
                                    {
                                        if (path[k, l] != path[k, l - 1])
                                            x2[path[k, l] - 1, path[k, l - 1] - 1] += od[i, j];//全有全无分配过程

                                    }
                                }
                            }
                        }
                    }
                //Console.WriteLine();
                //Console.WriteLine(count+"times load:");
                //for (int i = 0; i < m; i++)
                //{
                //    Console.WriteLine();
                //    for (int j = 0; j < m; j++)
                //        Console.Write(x2[i, j] + " ");
                //}
                System.Array.Copy(x1, x3, x1.Length);
                for (int i = 0; i < m; i++)
                    for (int j = 0; j < n; j++)
                        x1[i, j] +=(x2[i, j] - x3[i, j]) / (double)count;
                //X为程序UE程序分配的最终结果
                double s1 = 0, s2 = 0;
                for (int i = 0; i < m; i++)
                    for (int j = 0; j < n; j++)
                    {
                        s1 += Math.Pow(x1[i, j] - x3[i, j],2);
                        s2 += x3[i, j];
                    }
                
                beta = Math.Sqrt(s1) / s2;
                //Console.WriteLine();
                //Console.WriteLine("beta:" + beta);
                //Console.WriteLine();
                //Console.WriteLine(count + " times load result:");
                //for (int i = 0; i < m; i++)
                //{
                //    Console.WriteLine();
                //    for (int j = 0; j < m; j++)
                //        Console.Write(Math.Ceiling(x1[i, j]) + " ");
                //}
                pathlist.Add(path);//记录迭代过程中所有OD之间的最短路
            }
            x = x1;
            //=========计时结束============
            //输出结果
            //Console.WriteLine();
            //Console.WriteLine("Result:");
            //for (int i = 0; i < m; i++)
            //{
            //    Console.WriteLine();
            //    for (int j = 0; j < n; j++)
            //        Console.Write(Math.Ceiling(x[i, j]) + " ");
            //}
            //Console.WriteLine();
            //Console.WriteLine(beta);
            return g;
        }
    }
}
