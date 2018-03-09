using System;
using System.Collections.Generic;
using System.Text;
namespace ICITN
{
    #region 染色体类
    class GAChromosome : List<int>
    {
        int n, p, r;
        double[,] weighted_distance;
        double f1;
        public GAChromosome(Parameter Par) 
        {
            n = Par.m;
            p = Par.p;
            r = Par.r;
            weighted_distance = Par.weighted_distance;
            f1 = Par.obj_ex();
        }
        public void FitnessCalculate()
        {
            
            int count = this.Count;
            int k = 0;
            int[] index = new int[p-r];
            for (int i = 0; i < p; i++)
            {
                if (this[i] == 0) { index[k] = i; k++; }
            }
            double f2 = 0;
            double temp;
            for (int i = 0; i < n; i++)
            {
                temp = double.MaxValue;
                for (int j = 0; j < p-r; j++)
                {                                                                                                                                                                                       
                    temp = Math.Min(temp, weighted_distance[i, index[j]]);
                }
                f2 = f2 + temp;
            }
            double fitness = f2 - f1;
            Fitness = fitness;
        }
        public double Fitness;
    }
    #endregion
    #region 遗传算法类
    class GA
    {
        #region 变量定义
        int n, p,r;
        Parameter par;
        public GA(Parameter Par)
        {
            n = Par.m;
            p = Par.p;
            r = Par.r;
            par = Par;
        }
        public int PopulationSize;
        private void PopulationSizeCalculate()
        {
            int d = (int)Math.Round((double)n / p);
            double s = 1;
            int i, j;
            for (i = n; i >= n - p; i--)
            {
                s = s * i;
            }
            for (j = p; j >= 1; j--)
            {
                s = s / j;
            }
            this.PopulationSize = Math.Max(2, (int)Math.Round(n / 100 * (Math.Log(s) / d))) * d;
        }
        public enum SelectionType
        {
            Tournment,
            Roullette,
        };
        public SelectionType Selection = SelectionType.Roullette;
        public double MutationRate;//变异率
        public double CrossRate;//交叉率
        public System.Random m_Random = new System.Random();
        public List<GAChromosome> m_thisGeneration = new List<GAChromosome>();//这一代种群
        public List<GAChromosome> m_nextGeneration = new List<GAChromosome>();//下一代种群
        public Boolean ApplyElitism = false;
        int GenerationNum = 0;//迭代次数
        public int GenerationCount=1000;//总迭代次数
        public int Crosstype = 1;
        public System.Collections.ArrayList TotalFitness = new System.Collections.ArrayList();
        #endregion
        #region 方法定义
        public void Initialize(Parameter Par)
        {
            //PopulationSizeCalculate();
            PopulationSize = 100;
            if (PopulationSize >= 50) ApplyElitism = true;
            for (int i = 0; i < PopulationSize; i++)//PopulationSize为染色体数    
            {
                GAChromosome newParent = new GAChromosome(Par);
                this.Initializer(ref newParent);
                newParent.FitnessCalculate();
                m_thisGeneration.Add(newParent);
            }
            TotalFitness.Clear();
            RankPopulation(m_thisGeneration);
            TotalFitnessRecord();
        }//初始化种群
        public class GAComparer : IComparer<GAChromosome>
        {
            public int Compare(GAChromosome x, GAChromosome y)
            {
                if (x.Fitness > y.Fitness) return -1;
                else if (x.Fitness == y.Fitness) return 0;
                else return 1;
            }
        }//染色体比较器
        public void RankPopulation(List<GAChromosome> GASet)
        {
            GAComparer GAComp = new GAComparer();
            GASet.Sort(GAComp);
        }//对染色体按适应度降序排序
        public void CreateNextGeneration(Parameter Par)
        {
            m_nextGeneration.Clear();
            GAChromosome bestChromo = null;
            if (this.ApplyElitism) //最优秀适应度最好是否直接选择加入到新一代群体
            {
                bestChromo = m_thisGeneration[0];
            }//取出最优染色体(已排序)
            for (int i = 0; i < this.PopulationSize; i += 2)
            {
                //Step1选择
                int iDadParent = 0;
                int iMumParent = 0;
                if (this.Selection == SelectionType.Tournment)//竞争法
                {
                    iDadParent = TournamentSelection();
                    iMumParent = TournamentSelection();
                }
                else if (this.Selection == SelectionType.Roullette)//轮赌法
                {
                    iDadParent = RoulletteSelection();
                    iMumParent = RoulletteSelection();
                }
                GAChromosome Dad = (GAChromosome)m_thisGeneration[iDadParent];
                GAChromosome Mum = (GAChromosome)m_thisGeneration[iMumParent];
                GAChromosome child1 = new GAChromosome(Par);
                GAChromosome child2 = new GAChromosome(Par);
                //Step2交叉
                if (m_Random.NextDouble() < this.CrossRate)
                {
                    CrossOver(Dad, Mum, ref child1, ref child2);
                }
                else
                {
                    child1 = Dad;
                    child2 = Mum;
                }
                //Step3变异
                if (m_Random.NextDouble() < this.MutationRate)
                {
                    this.Mutation(ref child1);
                    this.Mutation(ref child2);
                }
                //Step4计算适应度
                child1.FitnessCalculate();
                child2.FitnessCalculate();
                m_nextGeneration.Add(Dad);
                m_nextGeneration.Add(Mum);
                m_nextGeneration.Add(child1);
                m_nextGeneration.Add(child2);
            }
            if (null != bestChromo)
                m_nextGeneration.Add(bestChromo);//最优的染色体插入子代群体中
            RankPopulation(m_nextGeneration);//按照适应度对染色体排序
            m_thisGeneration.Clear();//用新一代替换当前群体
            for (int j = 0; j < this.PopulationSize; j++)
            {
                m_thisGeneration.Add(m_nextGeneration[j]);
            }
            TotalFitnessRecord();

            //using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\BestFitness_RUE.txt", true))
            //{
            //    file.WriteLine(m_thisGeneration[0].Fitness.ToString());
            //} 
            this.GenerationNum++;//进化代计数
        }//产生下一代种群
        private int RoulletteSelection()
        {            
            double randomFitness = m_Random.NextDouble() * (double)TotalFitness[GenerationNum];
            //在当代群体中找到适应度与randomFitness相接近的染色体
            int index = -1;
            int i = 0;
            double sumofpart = 0;
            while (i < PopulationSize)
            {
                sumofpart = sumofpart + this.m_thisGeneration[i].Fitness;
                if (randomFitness <= sumofpart)
                {
                    index = i;
                    break;
                }
                i++;
            }
            if (index == -1)
            {
                index = PopulationSize - 1;
            }
            return index;
        }//轮赌法
        private int TournamentSelection()
        {
            int Count = 1;
            if (this.PopulationSize >= 50)
                Count = 8;
            else if (this.PopulationSize >= 30)
                Count = 6;
            else if (this.PopulationSize >= 10)
                Count = 3;
            else if (this.PopulationSize >= 2)
                Count = 2;
            int finalindex = 0;
            double dMaxFit = 0;
            for (int i = 0; i < Count; i++)
            {
                int sellndex = m_Random.Next(0, this.PopulationSize);
                double fitness = m_thisGeneration[sellndex].Fitness;
                if (fitness > dMaxFit)
                {
                    finalindex = i;
                    dMaxFit = fitness;
                }
            }
            return finalindex;

        }//竞赛法
        private void TotalFitnessRecord() 
        {
            double m_dTotalFitness = 0;
            for (int j = 0; j < PopulationSize; j++)
            {
                m_dTotalFitness += m_thisGeneration[j].Fitness;
            }
            TotalFitness.Add(m_dTotalFitness);
            //using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\TotalFitness_RUE.txt", true))
            //{
            //    file.WriteLine(m_dTotalFitness.ToString());
            //} 
        }
        public GAChromosome GetBestChromosome()
        {
            GAChromosome bestChromosome = m_thisGeneration[0];
            return bestChromosome;
        }//获得当前种群的最优适应度
        public void Initializer(ref GAChromosome GAChro)
        {
            int flag1 = 0;
            int flag2 = 1;
            double flag3;
            while (flag2 == 1)
            {
                for (int i = 0; i < p; i++)
                {
                    flag3 = m_Random.NextDouble();
                    if (flag3 > 0.5 && flag1 != r)
                    {
                        GAChro.Add(1);
                        flag1++;
                    }
                    else GAChro.Add(0);

                }
                if (flag1 == r) flag2 = 0;
                else
                {
                    GAChro.Clear();
                    flag1 = 0;
                }
            }
        }//染色体初始化
        public void Initializer1(ref GAChromosome GAChro) 
        {
            List<int> ones=new List<int>();
            ones.Clear();            
            while (ones.Count<p)
            {
                int temp=m_Random.Next(0, n);
                bool flag = true;
                for (int i = 0; i < ones.Count; i++)
                {
                    if (temp == ones[i]) 
                    {
                        flag = false;
                        continue;
                    }
                }
                if (flag) ones.Add(temp);
            }
            GAChro.Clear();
            for(int i=0;i<n;i++)
            {
                bool flag = true;
                for(int j=0;j<ones.Count;j++)
                {
                    if (ones[j] == i) 
                    {
                        GAChro.Add(1);
                        ones.Remove(i);
                        flag = false;
                    }
                }
                if (flag)
                    GAChro.Add(0);
            }
        }
        public void CrossOver(GAChromosome Dad, GAChromosome Mum, ref GAChromosome child1, ref GAChromosome child2)
        {   
            int countDad = 0, countMum = 0;
            List<int> crosspoint = new List<int>();
            for (int i = 0; i < p; i++)
            {
                if (Dad[i] == 1) countDad++;
                if (Mum[i] == 1) countMum++;
                if (countDad == countMum) { crosspoint.Add(i); };
            }
            switch (Crosstype = 1)
            {
                //一点交叉
                case 1:
                    {
                        int count = crosspoint.Count;
                        int j = ((int)m_Random.Next(0, count));
                        int index = (int)crosspoint[j];
                        for (int k = 0; k < p; k++)
                        {
                            if (k <= index)
                            {
                                child1.Add(Dad[k]);
                                child2.Add(Mum[k]);
                            }
                            else
                            {
                                child1.Add(Mum[k]);
                                child2.Add(Dad[k]);
                            }
                        }
                        break;
                    }
                //多点交叉
                case 2:
                    {
                        for (int i = 0; i < crosspoint.Count; i++)
                        {
                            if (m_Random.NextDouble() > 0.5)
                            {
                                for (int k = crosspoint[i]; k < crosspoint[i + 1]; k++)
                                {
                                    child1.Add(Dad[k]);
                                    child2.Add(Mum[k]);
                                }
                            }
                            else
                                for (int k = crosspoint[i]; k < crosspoint[i + 1]; k++)
                                {
                                child1.Add(Mum[k]);
                                child2.Add(Dad[k]);
                                }
                        }
                        break;
                    }
            }
        }//交叉
        public void Mutation(ref GAChromosome chromose)
        {
            int index = ((int)m_Random.Next(0,p));
            int count = ((int)m_Random.Next(0,p - index));
            chromose.Reverse(index, count);
        }//变异
        public int[] output()
        {
            int[] r = new int[p];
            GAChromosome GAChr = this.GetBestChromosome();
            for (int i = 0; i < p; i++)
                r[i] = GAChr[i];
            return r;
        }
        public void optimal()
        {
            this.Initialize(par);
            while (GenerationNum < GenerationCount)
                CreateNextGeneration(par);
        }
        #endregion
    }
    #endregion
    #region 参数类
    class Parameter 
    {
        public int p,r,m;
        double[] hi;
        double[,] dij;
        public double[,] weighted_distance;
        public Parameter(double[] hi, double[,] dij, int pp, int rr, int mm) 
        {
            this.hi = hi;
            this.dij = dij;
            p = pp;
            m = mm;
            r = rr;
            weighted_distance=new double[m,m];
            hidij();
        }
        public void hidij() 
        {

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < p; j++)
                {
                    weighted_distance[i, j] = hi[i] * dij[i, j];
                }
            }
        }
        public double obj_ex()
        {
            double obj = 0;
            double temp;
            for (int i = 0; i < m; i++)
            {
                temp = double.MaxValue;
                for (int j = 0; j < p; j++)
                {
                    temp = Math.Min(temp, weighted_distance[i, j]);
                }
                obj = obj + temp;
            }
            return obj;
        }

    }
    #endregion

}