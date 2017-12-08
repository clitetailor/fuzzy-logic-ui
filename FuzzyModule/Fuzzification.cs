using System;
using System.Collections;

namespace FuzzyLogic {
    class Fuzzification {
        private static readonly int[] DISTANCE_ARGS = new int[4] {0, 70, 140, 210}; //(m)
        private static readonly int[] LIGHT_RED_ARGS = new int[3] {0, 3, 6}; //(s)
        private static readonly int[] LIGHT_YELLOW_ARGS = new int[3] {0, 3, 6}; //(s)
        private static readonly int[] LIGHT_GREEN_ARGS = new int[3] {0, 3, 6};
        private static readonly int[] ANGLE_ARGS = new int[4] {0, 7, 14, 21}; //(*)

        public static ArrayList DistanceFuzzificate(double distance) {
            ArrayList result = new ArrayList();
            double value;
            if (distance >= DISTANCE_ARGS[0] && distance < DISTANCE_ARGS[1]) {
                result.Add(new Object[2] {Rules.N, 1});
            } else if (distance >= DISTANCE_ARGS[1] && distance < DISTANCE_ARGS[2]) {
                value = (DISTANCE_ARGS[2] - distance) / (DISTANCE_ARGS[2] - DISTANCE_ARGS[1]);
                if (value > 0) {
                    result.Add(new Object[2] {Rules.N, value});
                }
                value = (distance - DISTANCE_ARGS[1]) / (DISTANCE_ARGS[2] - DISTANCE_ARGS[1]);
                if (value > 0) {
                    result.Add(new Object[2] {Rules.MD, value});
                }
            } else if (distance >= DISTANCE_ARGS[2] && distance < DISTANCE_ARGS[3]) {
                value = (DISTANCE_ARGS[3] - distance) / (DISTANCE_ARGS[3] - DISTANCE_ARGS[2]);
                if (value > 0) {
                    result.Add(new Object[2] {Rules.MD, value});
                }
                value = (distance - DISTANCE_ARGS[2]) / (DISTANCE_ARGS[3] - DISTANCE_ARGS[2]);
                result.Add(new Object[2] {Rules.Fr, value});
            } else if (distance >= DISTANCE_ARGS[4]) {
                result.Add(new Object[2] {Rules.Fr, 1});
            }
            return result;
        }
        public static ArrayList LightFuzzificate(Object[] light_status) {
            String color = (String) light_status[0];
            double time = (double) light_status[1];
            ArrayList result = new ArrayList();
            double value;
            switch (color) {
                case Rules.R: {
                    if (time >= LIGHT_RED_ARGS[0] && time < LIGHT_RED_ARGS[1]) {
                        result.Add(new Object[2] {Rules.LR, 1});
                    } else if (time >= LIGHT_RED_ARGS[1] && time < LIGHT_RED_ARGS[2]) {
                        value = (LIGHT_RED_ARGS[2] - time) / (LIGHT_RED_ARGS[2] - LIGHT_RED_ARGS[1]);
                        if (value > 0) {
                            result.Add(new Object[2] {Rules.LR, value});
                        }
                        value = (time - LIGHT_RED_ARGS[1]) / (LIGHT_RED_ARGS[2] - LIGHT_RED_ARGS[1]);
                        if (value > 0) {
                            result.Add(new Object[2] {Rules.R, value});
                        }
                    } else if (time >= LIGHT_RED_ARGS[2]) {
                        result.Add(new Object[2] {Rules.R, 1});
                    }
                    break;
                }
                case Rules.Y: {
                    if (time >= LIGHT_YELLOW_ARGS[0] && time < LIGHT_YELLOW_ARGS[1]) {
                        value = (time - LIGHT_YELLOW_ARGS[0]) / (LIGHT_YELLOW_ARGS[1] - LIGHT_YELLOW_ARGS[0]);
                        if (value > 0) {
                            result.Add(new Object[2] {Rules.Y, value});
                        }
                    } else if (time >= LIGHT_YELLOW_ARGS[1] && time < LIGHT_YELLOW_ARGS[2]) {
                        value = (LIGHT_YELLOW_ARGS[2] - time) / (LIGHT_YELLOW_ARGS[2] - LIGHT_YELLOW_ARGS[1]);
                        if (value > 0) {
                            result.Add(new Object[2] {Rules.Y, value});
                        }
                    }
                    break;
                }
                case Rules.G: {
                    if (time >= LIGHT_GREEN_ARGS[0] && time < LIGHT_GREEN_ARGS[1]) {
                        result.Add(new Object[2] {Rules.LG, 1});
                    } else if (time >= LIGHT_GREEN_ARGS[1] && time < LIGHT_GREEN_ARGS[2]) {
                        value = (LIGHT_GREEN_ARGS[2] - time) / (LIGHT_GREEN_ARGS[2] - LIGHT_GREEN_ARGS[1]);
                        if (value > 0) {
                            result.Add(new Object[2] {Rules.LG, value});
                        }
                        value = (time - LIGHT_GREEN_ARGS[1]) / (LIGHT_GREEN_ARGS[2] - LIGHT_GREEN_ARGS[1]);
                        if (value > 0) {
                            result.Add(new Object[2] {Rules.G, value});
                        }
                    } else if (time >= LIGHT_GREEN_ARGS[2]) {
                        result.Add(new Object[2] {Rules.G, 1});
                    }
                    break;
                }
            }
            return result;
        }
        public static ArrayList AngleFuzzificate(double angle) {
            ArrayList result = new ArrayList();
            double value;
            if (angle >= ANGLE_ARGS[0] && angle < ANGLE_ARGS[1]) {
                result.Add(new Object[2] {Rules.Sm, 1});
            } else if (angle >= ANGLE_ARGS[1] && angle < ANGLE_ARGS[2]) {
                value = (ANGLE_ARGS[2] - angle) / (ANGLE_ARGS[2] - ANGLE_ARGS[1]);
                if (value > 0) {
                    result.Add(new Object[2] {Rules.Sm, value});
                }
                value = (angle - ANGLE_ARGS[1]) / (ANGLE_ARGS[2] - ANGLE_ARGS[1]);
                if (value > 0) {
                    result.Add(new Object[2] {Rules.MA, value});
                }
            } else if (angle >= ANGLE_ARGS[2] && angle < ANGLE_ARGS[3]) {
                value = (ANGLE_ARGS[3] - angle) / (ANGLE_ARGS[3] - ANGLE_ARGS[2]);
                if (value > 0) {
                    result.Add(new Object[2] {Rules.MA, value});
                }
                value = (angle - ANGLE_ARGS[2]) / (ANGLE_ARGS[3] - ANGLE_ARGS[2]);
                if (value > 0) {
                    result.Add(new Object[2] {Rules.B, value});
                }
            } else if (angle >= ANGLE_ARGS[4]) {
                result.Add(new Object[2] {Rules.B, 1});
            }
            return result;
        }
     }
}