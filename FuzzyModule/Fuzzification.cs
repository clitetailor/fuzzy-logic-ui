using System;

namespace FuzzyLogic {
    class Fuzzification {
        private static readonly int[] DISTANCE_ARGS = new int[4] {0, 70, 140, 210}; //(m)
        private static readonly int[] LIGHT_ARGS = new int[3] {0, 3, 6}; //(s)
        private static readonly int[] ANGLE_ARGS = new int[4] {0, 7, 14, 21}; //(*)

        public static Object DistanceFuzzificate(double distance) {
            Object result;
            if (distance >= DISTANCE_ARGS[0] && distance <= DISTANCE_ARGS[1]) {
                result = new Object[2] {Rules.N, 1};
            } else if (distance > DISTANCE_ARGS[1] && distance <= DISTANCE_ARGS[2]) {
                result = new Object[2, 2] {
                    {Rules.N, (DISTANCE_ARGS[2] - distance) / (DISTANCE_ARGS[2] - DISTANCE_ARGS[1])},
                    {Rules.MD, (distance - DISTANCE_ARGS[1]) / (DISTANCE_ARGS[2] - DISTANCE_ARGS[1])}
                };
            } else if (distance > DISTANCE_ARGS[2] && distance <= DISTANCE_ARGS[3]) {
                result = new Object[2, 2] {
                    {Rules.MD, (DISTANCE_ARGS[3] - distance) / (DISTANCE_ARGS[3] - DISTANCE_ARGS[2])},
                    {Rules.Fr, (distance - DISTANCE_ARGS[2]) / (DISTANCE_ARGS[3] - DISTANCE_ARGS[2])}
                };
            } else if (distance > DISTANCE_ARGS[4]) {
                result = new Object[2] {Rules.Fr, 1};
            } else {
                result = null;
            }
            return result;
        }
        public static Object LightFuzzificate(double lightTime) {
            return new Object[2] {"Green", 0.76};
        }
        public static Object AngleFuzzificate(double angle) {
            return new Object[2] {"Big", 0.54};
        }
     }
}