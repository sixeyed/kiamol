using k8s;
using k8s.Models;
using System;

namespace WebPingOperator.Model.CustomResources
{
    public class WebPingerArchive : KubernetesObject
    {
        public V1ObjectMeta Metadata { get; set; }

        public WebPingSpec Spec { get; set; }

        public class WebPingSpec
        {
            public string Target { get; set; }
        }

        public struct Definition
        {
            public const string Group = "ch20.kiamol.net";
            public const string Version = "v1";
            public const string Plural = "webpingerarchives";
            public const string Singular = "webpingerarchive";
            public const string Kind = "WebPingerArchive";
            public const string ShortName = "wpa";
        }
    }
}
