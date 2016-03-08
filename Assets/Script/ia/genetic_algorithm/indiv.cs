using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace geneticAlgo
{
    class indiv
    {
        List<double> data;
        double evalValue;

        public indiv()
        {
            data = new List<double>();
            evalValue = 0;
        }
        public indiv(List<double> data)
        {
            this.data = new List<double>(data);
            evalValue = 0;
        }
        public indiv(indiv I)
        {
            this.data = new List<double>(I.getData());
            evalValue = I.getEvalValue();
        }
        public indiv(List<double> data, double evalValue)
        {
            this.data = new List<double>(data);
            this.evalValue = evalValue;
        }
        public void eval()
        {
            evalValue = 0;
            for(int i = 0; i < data.Count; i++)
            {
                evalValue += Math.Sqrt(Math.Pow(data[i] - 0.5, 2))/data.Count;
            }
            evalValue = 1 - evalValue;
            //Console.WriteLine("data size : {0}", data.Count);
        }

        public void addDoubleToData(double d)
        {
            for(int i = 0; i < data.Count; i++)
            {
                data[i] += d;
            }
        }
        public void addData(double d)
        {
            data.Add(d);
        }
        public void setDataAtPos(int pos, double newData)
        {
            data[pos] = newData;
        }
        public void setData(List<double> data)
        {
            this.data = data;
        }
        public List<double> getData()
        {
            return data;
        }
        public int getSize()
        {
            return data.Count;
        }
        public double getEvalValue()
        {
            return evalValue;
        }
        public override string ToString()
        {
            string dataString="";
            for(int i = 0; i < data.Count; i++)
            {
                dataString += " | "+data[i].ToString();
            }
            return("data : "+dataString+", evalValue : "+evalValue.ToString());
        }
    }
}
