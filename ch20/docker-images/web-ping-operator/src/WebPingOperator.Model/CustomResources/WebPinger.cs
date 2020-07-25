using k8s;
using k8s.Models;
using System;
using System.Collections.Generic;

namespace WebPingOperator.Model.CustomResources
{
    public class WebPinger : KubernetesObject
    {
        public V1ObjectMeta Metadata { get; set; }

        public WebPingSpec Spec { get; set; }

        public class WebPingSpec
        {
            public string Target { get; set; }

            public string Interval { get; set; }

            public string Method { get; set; }

            public int GetIntervalMilliseconds()
            {
                var interval = int.Parse(Interval[0..^1]);
                var measure = Interval.ToLower()[^1];
                if (measure == 'm')
                {
                    interval *= 60;
                }
                //otherwise assume seconds
                return interval * 1000;
            }
            
            public string GetMethod()
            {
                return Method ?? "HEAD";
            }  
        }

        public struct Definition
        {
            public const string Group = "ch20.kiamol.net";
            public const string Version = "v1";
            public const string Plural = "webpingers";
            public const string Singular = "webpinger";
            public const string Kind = "WebPinger";
            public const string ShortName = "wp";
        }
    }
}
