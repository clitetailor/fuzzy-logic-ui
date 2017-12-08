using System;
using System.Collections;

namespace FuzzyLogic {
    class Program {
        static void Main(string[] args){
            bool type = Rules.LIGHT_TYPE;
            double sample_distance = 141;
            double sample_lightTime = 5;
            double sample_angle = 17;
            Object[] light_status = new Object[2] {"Red", sample_lightTime};
            ArrayList fuzzificate = Fuzzificate(type, sample_distance, sample_angle, light_status);
            ArrayList inference = InferenceWorking(type, fuzzificate);
            foreach (var item in inference) {
                Object[] l = (Object[]) item;
                Console.WriteLine("{0} - {1} - {2}", l[0], l[1], l[2]);
            }
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
                            Object[] speed = new Object[3] {123, "Fast", 1};//(Object[]) InferenceEngine.Inference(distance, angle, light);
                            result.Add(speed);
                        }
                    } else {
                        Object[] speed = new Object[3] {12, "Slow", 0.5};//(Object[]) InferenceEngine.Inference(distance,angle);
                        result.Add(speed);
                    }
                }
            }

            return result;
        }
    }
}
