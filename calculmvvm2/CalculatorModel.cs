using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calculmvvm2
{
    public class CalculatorModel
    {
            
        public double Add(double a, double b) => a + b;
        public double Subtract(double a, double b) => a - b;
        public double Multiply(double a, double b) => a * b;
        public double Divide(double a, double b) => b != 0 ? a / b : 0;
        public double CalculateRoot(double number, double degree)
        {
            if (degree == 0)
                throw new ArgumentException("Степень корня не может быть равна нулю.");
            if (number < 0 && degree % 2 == 0)
                throw new ArgumentException("Корень из отрицательного числа с четной степенью не существует.");
            return Math.Pow(number, 1.0 / degree);
        }
       
    }
}


