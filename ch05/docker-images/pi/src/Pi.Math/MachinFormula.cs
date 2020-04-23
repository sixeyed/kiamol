using Prometheus;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pi.Math
{
    public static class MachinFormula
    {
        private static readonly string[] _Labels = { "dpBucket" };
        private static readonly Dictionary<string, Tuple<int, int>> _DpBuckets = new Dictionary<string, Tuple<int, int>>()
            {
                {"1K", new Tuple<int, int>(0, 1000) },
                {"10K", new Tuple<int, int>(1000, 10000) },
                {"50K", new Tuple<int, int>(10000, 50000) },
                {"100K", new Tuple<int, int>(50000, 100000) },
                {"500K", new Tuple<int, int>(100000, 500000) },
                {"1M", new Tuple<int, int>(500000, 1000000) }
            };

        private static readonly Counter _ComputeCounter;
        private static readonly Counter _ComputeErrorCounter;
        private static readonly Gauge _ActiveComputeGauge;
        private static readonly Histogram _ComputeDurationHistogram;

        static MachinFormula()
        {
            _ComputeCounter = Metrics.CreateCounter("pi_compute_requestted_total", "Number of computations requested", _Labels);
            _ComputeErrorCounter = Metrics.CreateCounter("pi_compute_errored_total", "Number of computations which errored", _Labels);
            _ActiveComputeGauge = Metrics.CreateGauge("pi_compute_active", "Number of active computations", _Labels);
            _ComputeDurationHistogram = Metrics.CreateHistogram("pi_compute_duration_seconds", "Run duration for computations", new HistogramConfiguration
            {
                LabelNames = _Labels,
                Buckets = Histogram.ExponentialBuckets(0.5, 4, 10) //from 0.5s to ~36h
            });
        }

        /// <summary>
        /// Calculates Pi to the specified number of decimal places
        /// </summary>
        /// <see cref="https://latkin.org/blog/2012/03/20/how-to-calculate-1-million-digits-of-pi/"/>
        /// <param name="decimalPlaces">Decimal places to calculate</param>
        /// <param name="recordMetrics">Whether to record calculation metrics</param>
        /// <returns>Pi</returns>
        public static HighPrecision Calculate(int decimalPlaces, bool recordMetrics = false)
        {
            if (recordMetrics)
            {
                return CalculateAndRecordMetrics(decimalPlaces);
            }
            else
            {
                return CalculateInternal(decimalPlaces);
            }
        }

        private static HighPrecision CalculateAndRecordMetrics(int decimalPlaces)
        {
            var labels = new string[]
            {
                _DpBuckets.First(x=> decimalPlaces > x.Value.Item1 &&
                                     decimalPlaces < x.Value.Item2).Key
            };

            _ComputeCounter.WithLabels(labels).Inc();
            _ActiveComputeGauge.WithLabels(labels).Inc();

            try
            {
                using (_ComputeDurationHistogram.WithLabels(labels).NewTimer())
                {
                    return CalculateInternal(decimalPlaces);
                }
            }
            catch
            {
                _ComputeErrorCounter.WithLabels(labels).Inc();
                return new HighPrecision(0, 0);
            }
            finally
            {
                _ActiveComputeGauge.WithLabels(labels).Dec();
            }
        }

        private static HighPrecision CalculateInternal(int decimalPlaces)
        {
            HighPrecision.Precision = decimalPlaces;
            HighPrecision first = 4 * Atan.Calculate(5);
            HighPrecision second = Atan.Calculate(239);
            var pi = 4 * (first - second);
            return pi;
        }
    }
}
