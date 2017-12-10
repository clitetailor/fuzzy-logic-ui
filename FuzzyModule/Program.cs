using System;
using System.Collections;
using System.IO;

namespace FuzzyLogic {
    class Program {
        static void Main(string[] args){
            ArrayList distance = new ArrayList();
            ArrayList angle = new ArrayList();
            ArrayList light = new ArrayList();
            ArrayList speed = new ArrayList();
            double i = 0;
            for (int d = 0; d <= 100; d++) {
                for (int a = 0; a <= 90; a++) {
                    // RED
                    for (int l = 0; l <= 5; l++) {
                        Console.Write("{0}-", i++);
                        bool type = Rules.LIGHT_TYPE;
                        double sample_distance = (double) d;
                        double sample_lightTime = (double) l;
                        double sample_angle = (double) a;
                        Object[] light_status = new Object[2] {"Yellow", sample_lightTime};
                        double spd = CalculateSpeed(type, sample_distance, sample_angle, light_status);
                        distance.Add(sample_distance);
                        angle.Add(sample_angle);
                        light.Add(sample_lightTime);
                        speed.Add(spd);
                        if (Double.IsNaN(spd)) {
                            Console.WriteLine("NaN: {0} {1} {2}", sample_distance, sample_angle, sample_lightTime);
                            return;
                        }
                    }
                }
            }
            
            using(StreamWriter sr = new StreamWriter("distance-yellow.txt"))
            {
                foreach(var item in distance)
                {
                    sr.Write("{0} ", item);
                }
            }
            using(StreamWriter sr = new StreamWriter("angle-yellow.txt"))
            {
                foreach(var item in angle)
                {
                    sr.Write("{0} ", item);
                }
            }
            using(StreamWriter sr = new StreamWriter("light-yellow.txt"))
            {
                foreach(var item in light)
                {
                    sr.Write("{0} ", item);
                }
            }
            using(StreamWriter sr = new StreamWriter("speed-yellow.txt"))
            {
                foreach(var item in speed)
                {
                    sr.Write("{0} ", item);
                }
            }
        }

        public static double CalculateSpeed(bool type, double distance, double angle, Object[] light_status) {
            ArrayList fuzzificate = Fuzzificate(type, distance, angle, light_status);
            ArrayList inference = InferenceWorking(type, fuzzificate);
            double speed = Defuzzification.Defuzzificate(inference);
            return (double) speed;
        }

        private static ArrayList Fuzzificate(bool isLight, double distance, double angle, Object[] light_status) {
            ArrayList distance_instances = Fuzzification.DistanceFuzzificate(distance);
            ArrayList angle_instances = Fuzzification.AngleFuzzificate(angle);
            ArrayList light_instances;
            if (isLight) {
                light_instances = Fuzzification.LightFuzzificate(light_status);
            } else {
                light_instances = null;
            }

            ArrayList result = new ArrayList();
            result.Add(distance_instances);
            result.Add(angle_instances);
            result.Add(light_instances);

            return result;
        }

        private static ArrayList InferenceWorking(bool isLight, ArrayList detail) {
            ArrayList distance_instances = (ArrayList) detail[0];
            ArrayList angle_instances = (ArrayList) detail[1];
            ArrayList light_instances = (isLight)? (ArrayList) detail[2]: null;
            ArrayList result = new ArrayList();

            foreach (var distance in distance_instances) {
                foreach (var angle in angle_instances) {
                    if (light_instances != null) {
                        foreach (var light in light_instances) {
                            Object[] speed = (Object[]) InferenceEngine.Inference((Object[]) distance, (Object[]) angle, (Object[]) light);
                            result.Add(speed);
                        }
                    } else {
                        Object[] speed = (Object[]) InferenceEngine.Inference((Object[]) distance, (Object[]) angle);
                        result.Add(speed);
                    }
                }
            }

            return result;
        }
    }
}
