using System;

namespace ChuuniExtension.CountdownTools
{
    public class CountdownFactory
    {
        private static CountdownConfig config;

        public static ICountdown CustomGenerate(CountdownType type, CountdownConfig config){
            return type switch{
                CountdownType.Interval => new TimeCountdown(config.Interval),
                CountdownType.Distance => new DistanceCountdown(config.Speed, config.Distance),
                _ => throw new Exception("Unrecognized CountdownType")
            };
        }

        public static ICountdown Generate(CountdownType type){
            return type switch{
                CountdownType.Interval => new TimeCountdown(config.Interval),
                CountdownType.Distance => new DistanceCountdown(config.Speed, config.Distance),
                _ => throw new Exception("Unrecognized CountdownType")
            };
        }

        public static void SetConfig(CountdownConfig config) => CountdownFactory.config = config;
        public static ICountdown GetDefaultTimeCountdown() => new TimeCountdown(3);
        public static ICountdown GetDefaultDistanceCountdown() => new DistanceCountdown(1, 10);
    }
}
