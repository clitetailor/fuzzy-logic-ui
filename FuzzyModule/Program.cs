using System;
using System.Collections;

namespace FuzzyLogic {
    class Program {
        static void Main(string[] args){
            // Console.WriteLine("Start");
            bool type = Rules.LIGHT_TYPE;
            double sample_distance = 30d;
            double sample_lightTime = 5d;
            double sample_angle = 5d;
            Object[] light_status = new Object[2] {"Green", sample_lightTime};

            Console.WriteLine("Speed: {0}", CalculateSpeed(type, sample_distance, sample_angle, light_status));
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

            // Console.WriteLine("Type P:29 {0} {1} {2}", ((Object[]) distance_instances[0])[1], ((Object[]) angle_instances[0])[1], ((Object[]) light_instances[0])[1]);

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
                            // Console.WriteLine("Type P:48 {0} {1} {2}", ((Object[]) distance)[1].GetType(), ((Object[]) angle)[1].GetType(), ((Object[]) light)[1].GetType());
                            // Console.WriteLine("speed: {0} {1} {2}", (speed[0])[0], speed[1], speed[2]);
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
