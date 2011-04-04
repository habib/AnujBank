using System;
using System.IO;
using AnujBank;

namespace TestAnujBank
{
    public class InterestRates
    {

        private double positiveInterestRate;
        private double negativeInterestRate;
        TextReader Reader { get; set; }

        public InterestRates()
        {
            
        }
        public InterestRates(TextReader reader)
        {
            Reader = reader;
        }

        public void Configure()
        {
            parse();
        }

        private void parse()
        {
            string line;
            while((line = Reader.ReadLine()) != null)
            {
                string[] tokens = line.Split('=');
                if (tokens[0].ToUpper().Equals("POSITIVEINTERESTRATE"))
                {
                    positiveInterestRate = Double.Parse(tokens[1]);
                }
                else if(tokens[0].ToUpper().Equals("NEGATIVEINTERESTRATE"))
                {
                    negativeInterestRate = Double.Parse(tokens[1]);
                }
            }
        }

        public virtual double PositiveInterestRate()
        {
            return positiveInterestRate;
        }

        public virtual double NegativeInterestRate()
        {
            return negativeInterestRate;
        }
    }
}