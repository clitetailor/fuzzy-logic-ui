using System;
using System.Collections;

namespace FuzzyLogic {
    class Defuzzification {
        public static double Defuzzificate(ArrayList inference_result) {
            double speed_total = 0d;
            double muy_total = 0d;
            foreach (var item in inference_result) {
                Object[] l = (Object[]) item;
                ArrayList coeficients = (ArrayList) l[0];
                String label = (String) l[1];
                double muy = (double) l[2];
                Func<double, double> f1 = FunctionToIntegrate(1, coeficients, label, muy);
                Func<double, double> f2 = FunctionToIntegrate(2, coeficients, label, muy);
                double numerator = Integrate(f1, 0, 2, 1500);
                double denominator = Integrate(f2, 0, 2, 1500);
                // Console.Write("Case: ");
                // foreach (var s in coeficients) {
                //     Console.Write("{0}, ", s);
                // }
                // Console.WriteLine(" {0} - {1} - {2} - {3}", label, muy, numerator, denominator);
                speed_total += (denominator != 0)? muy * (numerator/denominator): 0;
                muy_total += muy;
            }          
            return (double) speed_total / muy_total;
        }

        private static double Integrate(Func<double,double> f, double x_low, double x_high, int N_steps) {
            double h = (x_high - x_low) / N_steps;
            double res = (f(x_low) + f(x_high)) / 2;
            for(int i = 1; i < N_steps; i++) {
                res += f(x_low + i * h);
            }
            return h * res;
        }

        private static Func<double, double> FunctionToIntegrate(int type, ArrayList coeficients, String label, double muy) {
            Func<double, double> result;
            switch (label) {
                case Rules.Fs: {
                    result = y => {
                        if (y <= Rules.SPEED_ARGS[3]) {
                            return 0;
                        } else if (y > Rules.SPEED_ARGS[3] && y <= (double) coeficients[0]) {
                            return (type == 1)? (y * (y - Rules.SPEED_ARGS[3]) / (Rules.SPEED_ARGS[4] - Rules.SPEED_ARGS[3]))
                                                :((y - Rules.SPEED_ARGS[3]) / (Rules.SPEED_ARGS[4] - Rules.SPEED_ARGS[3]));
                        } else {
                            return (type == 1)? (muy * y)
                                                :muy;
                        }
                    };
                    break;
                }
                case Rules.Sr: {
                    result = y => {
                        if (y > Rules.SPEED_ARGS[2] && y <= (double) coeficients[0]) {
                            return (type == 1)? y * (y - Rules.SPEED_ARGS[2]) / (Rules.SPEED_ARGS[3] - Rules.SPEED_ARGS[2])
                                                : (y - Rules.SPEED_ARGS[2]) / (Rules.SPEED_ARGS[3] - Rules.SPEED_ARGS[2]);
                        } else if (y > (double) coeficients[0] && y <= (double) coeficients[1]) {
                            return (type == 1)? y * muy
                                                : muy;
                        } else if (y > (double) coeficients[1] && y <= Rules.SPEED_ARGS[4]) {
                            return (type == 1)? y * (Rules.SPEED_ARGS[4] - y) / (Rules.SPEED_ARGS[4] - Rules.SPEED_ARGS[3])
                                                : (Rules.SPEED_ARGS[4] - y) / (Rules.SPEED_ARGS[4] - Rules.SPEED_ARGS[3]);
                        } else {
                            return 0;
                        }
                    };
                    break;
                }
                case Rules.Sl: {
                    result = y => {
                        if (y > Rules.SPEED_ARGS[0] && y <= (double) coeficients[0]) {
                            return (type == 1)? y * (y - Rules.SPEED_ARGS[0]) / (Rules.SPEED_ARGS[2] - Rules.SPEED_ARGS[0])
                                                : (y - Rules.SPEED_ARGS[0]) / (Rules.SPEED_ARGS[2] - Rules.SPEED_ARGS[0]);
                        } else if (y > (double) coeficients[0] && y <= (double) coeficients[1]) {
                            return (type == 1)? y * muy
                                                : muy;
                        } else if (y > (double) coeficients[1] && y <= Rules.SPEED_ARGS[3]) {
                            return (type == 1)? y * (Rules.SPEED_ARGS[3] - y) / (Rules.SPEED_ARGS[3] - Rules.SPEED_ARGS[2])
                                                : (Rules.SPEED_ARGS[2] - y) / (Rules.SPEED_ARGS[2] - Rules.SPEED_ARGS[2]);
                        } else {
                            return 0;
                        }
                    };
                    break;
                }
                case Rules.St: {
                    result = y => {
                        return (type == 1)? y * (Rules.SPEED_ARGS[1] - y) / (Rules.SPEED_ARGS[1] - Rules.SPEED_ARGS[0])
                                            : (Rules.SPEED_ARGS[1] - y) / (Rules.SPEED_ARGS[1] - Rules.SPEED_ARGS[0]);
                    };
                    break;
                }
                default: {
                    result = y => 0;
                    break;
                }
            }
            return result;
        }
    }
}