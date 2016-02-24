using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace geneticAlgo
{
    class indiv
    {
        double evalValue;
        double data;

        public indiv(double data)
        {
            this.data = data;
            evalValue = 0;
        }
        public void eval()
        {
            evalValue = (1 - Math.Sqrt(Math.Pow(data - 0.5, 2)));
        }

        public void setData(double data)
        {
            this.data = data;
        }
        public double getData()
        {
            return data;
        }
        public double getEvalValue()
        {
            return evalValue;
        }
        public override string ToString()
        {
            return("data : "+data.ToString()+", evalValue : "+evalValue.ToString());
        }
    }
}
