using System;
using System.Collections;

namespace FuzzyLogic {
    class InferenceEngine {
        public static Object[] Inference(Object[] distance, Object[] angle, Object[] light_status = null) {
            Object[] rule;
            double muy = 0d;
            String label;
            ArrayList speed;
            if (light_status != null) {
                rule = Rules.checkRule((String)distance[0], (String) angle[0], (String) light_status[0]);
                // Not sure how to parse. Type changing on input!
                muy = Math.Min((double) distance[1], Math.Min((double) angle[1],(double) light_status[1]));
            } else {
                rule = Rules.checkRule((String)distance[0], (String) angle[0]);
                // same
                muy = Math.Min((double) distance[1], (double) angle[1]);
            }
            label = (String) rule[rule.Length - 1];
            speed = CalculateSpeedCoefficient(label, muy);
            return new Object[3] {speed, label, muy};
        }

        private static ArrayList CalculateSpeedCoefficient(String label, double muy) {
            ArrayList result = new ArrayList();
            switch (label) {
                case Rules.Fs: {
                    result.Add((Rules.SPEED_ARGS[4] - Rules.SPEED_ARGS[3]) * muy + Rules.SPEED_ARGS[3]);
                    break;
                }
                case Rules.Sr: {
                    result.Add(Rules.SPEED_ARGS[4] - (Rules.SPEED_ARGS[4] - Rules.SPEED_ARGS[3]) * muy);
                    result.Add((Rules.SPEED_ARGS[3] - Rules.SPEED_ARGS[2]) * muy + Rules.SPEED_ARGS[2]);
                    break;
                }
                case Rules.Sl: {
                    result.Add(Rules.SPEED_ARGS[3] - (Rules.SPEED_ARGS[3] - Rules.SPEED_ARGS[2]) * muy);
                    result.Add((Rules.SPEED_ARGS[2] - Rules.SPEED_ARGS[0]) * muy + Rules.SPEED_ARGS[0]);
                    break;
                }
                case Rules.St: {
                    result.Add(Rules.SPEED_ARGS[1] - (Rules.SPEED_ARGS[1] - Rules.SPEED_ARGS[0]) * muy);
                    break;
                }
                default:
                    break;
            }
            return result;
        }
    }
}