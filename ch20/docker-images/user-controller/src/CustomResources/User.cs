using k8s;
using k8s.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace UserController.CustomResources
{
    public class User : KubernetesObject
    {
        public V1ObjectMeta Metadata { get; set; }

        public UserSpec Spec { get; set; }

        public class UserSpec
        {
            public string Email { get; set; }

            public string Group { get; set; }

            public string Token { get; set; }

            public string GetGroupNamespace()
            {
                return $"kiamol-ch20-authn-{Group}";
            }
        }  
        
        public struct Definition
        {
            public const string Group = "ch20.kiamol.net";
            public const string Version= "v1";
            public const string Plural = "users";
        }
    }
}
