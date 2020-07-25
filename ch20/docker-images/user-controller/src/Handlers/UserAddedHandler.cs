using AutoMapper;
using k8s;
using k8s.Models;
using Microsoft.AspNetCore.JsonPatch;
using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserController.CustomResources;

namespace UserController.Handlers
{
    public class UserAddedHandler
    {
        private readonly Kubernetes _client;

        public UserAddedHandler(Kubernetes client)
        {
            _client = client;
        }

        public void Handle(User user)
        {
            EnsureGroupNamespace(user);
            EnsureServiceAccount(user);
            EnsureServiceAccountToken(user);
        }

        private void EnsureGroupNamespace(User user)
        {
            var groupNamespaceName = user.Spec.GetGroupNamespace();
            var kiamolNamespaces = _client.ListNamespace(fieldSelector: $"metadata.name={groupNamespaceName}");
            if (!kiamolNamespaces.Items.Any())
            {
                var groupNamespace = new V1Namespace
                {
                    Metadata = new V1ObjectMeta
                    {
                        Name = groupNamespaceName,
                        Labels = new Dictionary<string, string>()
                            {
                                { "kiamol", "ch20"},
                            }
                    }
                };
                _client.CreateNamespace(groupNamespace);
                Console.WriteLine($"** Created group namespace: {groupNamespaceName}");
            }
            else
            {
                Console.WriteLine($"** Group namespace exists: {groupNamespaceName}");
            }
        }

        private void EnsureServiceAccount(User user)
        {
            var groupNamespaceName = user.Spec.GetGroupNamespace();
            var serviceAccountName = user.Metadata.Name;
            var serviceAccounts = _client.ListNamespacedServiceAccount(groupNamespaceName,
                                                                       fieldSelector: $"metadata.name={serviceAccountName}");
            if (!serviceAccounts.Items.Any())
            {
                var serviceAccount = new V1ServiceAccount
                {
                    Metadata = new V1ObjectMeta
                    {
                        Name = serviceAccountName,
                        Labels = new Dictionary<string, string>()
                            {
                                { "kiamol", "ch20"},
                            }
                    },
                    AutomountServiceAccountToken = false
                };
                _client.CreateNamespacedServiceAccount(serviceAccount, groupNamespaceName);
                Console.WriteLine($"** Created service account: {serviceAccountName}, in group namespace: {groupNamespaceName}");
            }
            else
            {
                Console.WriteLine($"** Service account exists: {serviceAccountName}, in group namespace: {groupNamespaceName}");
            }
        }

        private void EnsureServiceAccountToken(User user)
        {
            var groupNamespaceName = user.Spec.GetGroupNamespace();
            var tokenName = $"{user.Metadata.Name}-token";
            var tokens = _client.ListNamespacedSecret(groupNamespaceName,
                                                      fieldSelector: $"metadata.name={tokenName}");
            if (!tokens.Items.Any())
            {
                var secret = new V1Secret
                {
                    Metadata = new V1ObjectMeta
                    {
                        Name = tokenName,
                        Labels = new Dictionary<string, string>()
                        {
                            { "kiamol", "ch20"},
                        },
                        Annotations = new Dictionary<string, string>()
                        {
                            { "kubernetes.io/service-account.name", user.Metadata.Name},
                        }
                    },
                    Type = "kubernetes.io/service-account-token"
                };
                _client.CreateNamespacedSecret(secret, groupNamespaceName);
                Console.WriteLine($"** Created token: {tokenName}, in group namespace: {groupNamespaceName}");
            }
            else
            {
                Console.WriteLine($"** Token exists: {tokenName}, in group namespace: {groupNamespaceName}");
            }
        }
    }
}
