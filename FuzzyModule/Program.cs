using System;
using System.Collections;

namespace FuzzyLogic {
    class Program {
        static void Main(string[] args){
            double sample_distance = 141;
            double sample_lightTime = 5;
            double sample_angle = 17;
            ArrayList distance_result = Fuzzification.DistanceFuzzificate(sample_distance);
            Object[] Item0 = (Object[]) result[0];
            Console.WriteLine("Result: {0} - {1}", Item0);
        }
    }
}
