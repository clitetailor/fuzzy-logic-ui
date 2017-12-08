using System;
using System.Collections;

namespace FuzzyLogic {
    class Fuzzification {
        public static ArrayList DistanceFuzzificate(double distance) {
            ArrayList result = new ArrayList();
            double value;
            if (distance >= Rules.DISTANCE_ARGS[0] && distance < Rules.DISTANCE_ARGS[1]) {
                result.Add(new Object[2] {Rules.N, 1});
            } else if (distance >= Rules.DISTANCE_ARGS[1] && distance < Rules.DISTANCE_ARGS[2]) {
                value = (Rules.DISTANCE_ARGS[2] - distance) / (Rules.DISTANCE_ARGS[2] - Rules.DISTANCE_ARGS[1]);
                if (value > 0) {
                    result.Add(new Object[2] {Rules.N, value});
                }
                value = (distance - Rules.DISTANCE_ARGS[1]) / (Rules.DISTANCE_ARGS[2] - Rules.DISTANCE_ARGS[1]);
                if (value > 0) {
                    result.Add(new Object[2] {Rules.MD, value});
                }
            } else if (distance >= Rules.DISTANCE_ARGS[2] && distance < Rules.DISTANCE_ARGS[3]) {
                value = (Rules.DISTANCE_ARGS[3] - distance) / (Rules.DISTANCE_ARGS[3] - Rules.DISTANCE_ARGS[2]);
                if (value > 0) {
                    result.Add(new Object[2] {Rules.MD, value});
                }
                value = (distance - Rules.DISTANCE_ARGS[2]) / (Rules.DISTANCE_ARGS[3] - Rules.DISTANCE_ARGS[2]);
                result.Add(new Object[2] {Rules.Fr, value});
            } else if (distance >= Rules.DISTANCE_ARGS[4]) {
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
                    if (time >= Rules.LIGHT_RED_ARGS[0] && time < Rules.LIGHT_RED_ARGS[1]) {
                        result.Add(new Object[2] {Rules.LR, 1});
                    } else if (time >= Rules.LIGHT_RED_ARGS[1] && time < Rules.LIGHT_RED_ARGS[2]) {
                        value = (Rules.LIGHT_RED_ARGS[2] - time) / (Rules.LIGHT_RED_ARGS[2] - Rules.LIGHT_RED_ARGS[1]);
                        if (value > 0) {
                            result.Add(new Object[2] {Rules.LR, value});
                        }
                        value = (time - Rules.LIGHT_RED_ARGS[1]) / (Rules.LIGHT_RED_ARGS[2] - Rules.LIGHT_RED_ARGS[1]);
                        if (value > 0) {
                            result.Add(new Object[2] {Rules.R, value});
                        }
                    } else if (time >= Rules.LIGHT_RED_ARGS[2]) {
                        result.Add(new Object[2] {Rules.R, 1});
                    }
                    break;
                }
                case Rules.Y: {
                    if (time >= Rules.LIGHT_YELLOW_ARGS[0] && time < Rules.LIGHT_YELLOW_ARGS[1]) {
                        value = (time - Rules.LIGHT_YELLOW_ARGS[0]) / (Rules.LIGHT_YELLOW_ARGS[1] - Rules.LIGHT_YELLOW_ARGS[0]);
                        if (value > 0) {
                            result.Add(new Object[2] {Rules.Y, value});
                        }
                    } else if (time >= Rules.LIGHT_YELLOW_ARGS[1] && time < Rules.LIGHT_YELLOW_ARGS[2]) {
                        value = (Rules.LIGHT_YELLOW_ARGS[2] - time) / (Rules.LIGHT_YELLOW_ARGS[2] - Rules.LIGHT_YELLOW_ARGS[1]);
                        if (value > 0) {
                            result.Add(new Object[2] {Rules.Y, value});
                        }
                    }
                    break;
                }
                case Rules.G: {
                    if (time >= Rules.LIGHT_GREEN_ARGS[0] && time < Rules.LIGHT_GREEN_ARGS[1]) {
                        result.Add(new Object[2] {Rules.LG, 1});
                    } else if (time >= Rules.LIGHT_GREEN_ARGS[1] && time < Rules.LIGHT_GREEN_ARGS[2]) {
                        value = (Rules.LIGHT_GREEN_ARGS[2] - time) / (Rules.LIGHT_GREEN_ARGS[2] - Rules.LIGHT_GREEN_ARGS[1]);
                        if (value > 0) {
                            result.Add(new Object[2] {Rules.LG, value});
                        }
                        value = (time - Rules.LIGHT_GREEN_ARGS[1]) / (Rules.LIGHT_GREEN_ARGS[2] - Rules.LIGHT_GREEN_ARGS[1]);
                        if (value > 0) {
                            result.Add(new Object[2] {Rules.G, value});
                        }
                    } else if (time >= Rules.LIGHT_GREEN_ARGS[2]) {
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
            if (angle >= Rules.ANGLE_ARGS[0] && angle < Rules.ANGLE_ARGS[1]) {
                result.Add(new Object[2] {Rules.Sm, 1});
            } else if (angle >= Rules.ANGLE_ARGS[1] && angle < Rules.ANGLE_ARGS[2]) {
                value = (Rules.ANGLE_ARGS[2] - angle) / (Rules.ANGLE_ARGS[2] - Rules.ANGLE_ARGS[1]);
                if (value > 0) {
                    result.Add(new Object[2] {Rules.Sm, value});
                }
                value = (angle - Rules.ANGLE_ARGS[1]) / (Rules.ANGLE_ARGS[2] - Rules.ANGLE_ARGS[1]);
                if (value > 0) {
                    result.Add(new Object[2] {Rules.MA, value});
                }
            } else if (angle >= Rules.ANGLE_ARGS[2] && angle < Rules.ANGLE_ARGS[3]) {
                value = (Rules.ANGLE_ARGS[3] - angle) / (Rules.ANGLE_ARGS[3] - Rules.ANGLE_ARGS[2]);
                if (value > 0) {
                    result.Add(new Object[2] {Rules.MA, value});
                }
                value = (angle - Rules.ANGLE_ARGS[2]) / (Rules.ANGLE_ARGS[3] - Rules.ANGLE_ARGS[2]);
                if (value > 0) {
                    result.Add(new Object[2] {Rules.B, value});
                }
            } else if (angle >= Rules.ANGLE_ARGS[4]) {
                result.Add(new Object[2] {Rules.B, 1});
            }
            return result;
        }
     }
}