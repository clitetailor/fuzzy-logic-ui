using System;

namespace FuzzyLogic {
    class Program {
        static void Main(string[] args){
            double sample_distance = 141;
            double sample_lightTime = 5;
            double sample_angle = 17;
            Object[,] result = Fuzzification.DistanceFuzzificate(sample_distance) as Object[,];
            Console.WriteLine("Result: {0} - {1} - {2} - {3}, ", result[0,0], result[0, 1], result[1, 0], result[1, 1]);
        }
    }
}
