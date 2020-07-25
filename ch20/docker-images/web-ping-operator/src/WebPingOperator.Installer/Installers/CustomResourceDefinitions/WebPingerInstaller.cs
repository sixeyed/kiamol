using k8s;
using k8s.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebPingOperator.Model.CustomResources;

namespace WebPingOperator.Installer.Installers
{
    public class WebPingerInstaller : IInstaller
    {
        private readonly Kubernetes _kubernetes;

        public WebPingerInstaller(Kubernetes kubernetes)
        {
            _kubernetes = kubernetes;
        }

        public async Task InstallAsync()
        {
            await EnsureWebPingerCrdAsync();
        }

        private async Task EnsureWebPingerCrdAsync()
        {
            var crds = await _kubernetes.ListCustomResourceDefinitionAsync(
                                            fieldSelector: $"metadata.name={WebPinger.Definition.Plural}.{WebPinger.Definition.Group}");

            if (!crds.Items.Any())
            {
                var crd = new V1CustomResourceDefinition
                {
                    Metadata = new V1ObjectMeta
                    {
                        Name = $"{WebPinger.Definition.Plural}.{WebPinger.Definition.Group}",
                        Labels = new Dictionary<string, string>()
                        {
                            { "kiamol", "ch20" },
                            { "operator", "web-ping" }
                        }
                    },
                    Spec = new V1CustomResourceDefinitionSpec
                    {
                        Group = WebPinger.Definition.Group,
                        Scope = "Namespaced",
                        Names = new V1CustomResourceDefinitionNames
                        {
                            Plural = WebPinger.Definition.Plural,
                            Singular = WebPinger.Definition.Singular,
                            Kind = WebPinger.Definition.Kind,
                            ShortNames = new List<string>
                            {
                                WebPinger.Definition.ShortName
                            }
                        },
                        Versions = new List<V1CustomResourceDefinitionVersion>
                        {
                            new V1CustomResourceDefinitionVersion
                            {
                                Name = "v1",
                                Served = true,
                                Storage = true,
                                Schema = new V1CustomResourceValidation
                                {
                                    OpenAPIV3Schema = new V1JSONSchemaProps
                                    {
                                        Type = "object",
                                        Properties = new Dictionary<string, V1JSONSchemaProps>
                                        {
                                            {
                                                "spec",
                                                new V1JSONSchemaProps
                                                {
                                                    Type = "object",
                                                    Properties = new Dictionary<string, V1JSONSchemaProps>
                                                    {
                                                        {
                                                            "target",
                                                            new V1JSONSchemaProps
                                                            {
                                                                Type = "string"
                                                            }
                                                        },
                                                        {
                                                            "interval",
                                                            new V1JSONSchemaProps
                                                            {
                                                                Type = "string"
                                                            }
                                                        },
                                                        {
                                                            "method",
                                                            new V1JSONSchemaProps
                                                            {
                                                                Type = "string"
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }                                        
                                    }
                                }
                            }
                        }
                    }
                };

                await _kubernetes.CreateCustomResourceDefinitionAsync(crd);
                Console.WriteLine($"** Created CRD for Kind: {WebPinger.Definition.Kind}; ApiVersion: {WebPinger.Definition.Group}/{WebPinger.Definition.Version}");
            }
            else
            {
                Console.WriteLine($"** CRD already exists for Kind: {WebPinger.Definition.Kind}; ApiVersion: {WebPinger.Definition.Group}/{WebPinger.Definition.Version}");
            }
        }        
    }
}