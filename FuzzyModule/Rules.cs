using System;

namespace FuzzyLogic {
    class Rules {
        public static double[] DISTANCE_ARGS = new double[4] {0d, 25d, 50d, 75d}; // 100(m)
        public static double[] LIGHT_RED_ARGS = new double[3] {0d, 3d, 6d}; //10(s)
        public static double[] LIGHT_YELLOW_ARGS = new double[2] {0d, 2d}; //5(s)
        public static double[] LIGHT_GREEN_ARGS = new double[3] {0d, 3d, 6d}; //10(s)
        public static double[] ANGLE_ARGS = new double[4] {0d, 22d, 45d, 67d}; //90(*)
        public static double[] SPEED_ARGS = new double[5] {0d, 10d, 50d, 100d, 150d}; //150(mph)
        // type
        public const bool LIGHT_TYPE = true;
        public const bool OBSTACLE_TYPE = false;
        public const String NONE = "None";
        //Distance
        public const String N = "Near";
        public const String MD = "Medium";
        public const String Fr = "Far";
        //LightStatus
        public const String R = "Red";
        public const String LR = "Less Red";
        public const String Y = "Yello";
        public const String LG = "Less Green";
        public const String G = "Green";
        //Angle
        public const String Sm = "Small";
        public const String MA = "Medium";
        public const String B = "Big";
        // Speed
        public const String St = "Stop";
        public const String Sl = "Slow";
        public const String Sr = "Slower";
        public const String Fs = "Fast";
        private static readonly String[,] LIGHT_RULES = new String[45, 4] {
            {N, G, Sm, Fs},
            {N, G, MD, Sr},
            {N, G, B, Sl},
            {MD, G, Sm, Fs},
            {MD, G, MA, Sr},
            {MD, G, B, Sl},
            {Fr, G, Sm, Fs},
            {Fr, G, MA, Sr},
            {Fr, G, B, Sl},
            {N, LG, Sm, Sl},
            {N, LG, MA, Sl},
            {N, LG, B, Sl},
            {MD, LG, Sm, Fs},
            {MD, LG, MA, Sr},
            {MD, LG, B, Sl},
            {Fr, LG, Sm, Fs},
            {Fr, LG, MA, Sr},
            {Fr, LG, B, Sl},
            {N, Y, Sm, St},
            {N, Y, MA, St},
            {N, Y, B, St},
            {MD, Y, Sm, Sr},
            {MD, Y, MA, Sr},
            {MD, Y, B, Sl},
            {Fr, Y, Sm, Fs},
            {Fr, Y, MA, Sr},
            {Fr, Y, B, Sl},
            {N, R, Sm, St},
            {N, R, MA, St},
            {N, R, B, St},
            {MD, R, Sm, Fs},
            {MD, R, MA, Sr},
            {MD, R, B, Sl},
            {Fr, R, Sm, Fs},
            {Fr, R, MA, Sr},
            {Fr, R, B, Sl},
            {N, LR, Sm, St},
            {N, LR, MA, St},
            {N, LR, B, St},
            {MD, LR, Sm, Sr},
            {MD, LR, MA, Sr},
            {MD, LR, B, Sl},
            {Fr, LR, Sm, Fs},
            {Fr, LR, MA, Sr},
            {Fr, LR, B, Sl}            
        };

        private static readonly String[,] OBSTACLE_RULES = new String[9, 3] {
            {N, Sm, St},
            {N, MA, St},
            {N, B, St},
            {MD, Sm, Fs},
            {MD, MA, Sr},
            {MD, B, Sl},
            {Fr, Sm, Fs},
            {Fr, MA, Sr},
            {Fr, B, Sl}
        };

        public static Object[] checkRule(String distance, String angle, String light_status = NONE) {
            if (light_status == NONE) {
                for (int i = 0, l = LIGHT_RULES.Length; i < l; i++) {
                    if (LIGHT_RULES[i, 0] == distance && LIGHT_RULES[i, 1] == light_status && LIGHT_RULES[i, 2] == angle) {
                        return new Object[4]{LIGHT_RULES[i, 0], LIGHT_RULES[i, 1], LIGHT_RULES[i, 2], LIGHT_RULES[i,3]};
                    }
                }
            } else { // OBSTACLE_TYPE
                for (int i = 0, l = OBSTACLE_RULES.Length; i < l; i++) {
                    if (OBSTACLE_RULES[i, 0] == distance && OBSTACLE_RULES[i, 1] == angle) {
                        return new Object[3]{OBSTACLE_RULES[i, 0], OBSTACLE_RULES[i, 1], OBSTACLE_RULES[i, 2]};
                    }
                }
            }
            return null;
        }
    }
}