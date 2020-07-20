using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KubeExplorer.Model
{
    public class PodModel
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Node { get; set; }
        public string Phase { get; set; }
        public string Message { get; set; }
    }
}
