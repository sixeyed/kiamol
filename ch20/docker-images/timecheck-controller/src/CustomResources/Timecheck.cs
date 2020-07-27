using k8s;
using k8s.Models;

namespace TimecheckController.CustomResources
{
    public class Timecheck : KubernetesObject
    {
        public V1ObjectMeta Metadata { get; set; }

        public TimecheckSpec Spec { get; set; }

        public class TimecheckSpec
        {
            public string Environment { get; set; }

            public int Interval { get; set; }
        }  
        
        public struct Definition
        {
            public const string Group = "ch20.kiamol.net";
            public const string Version= "v1";
            public const string Plural = "timechecks";
        }
    }
}
