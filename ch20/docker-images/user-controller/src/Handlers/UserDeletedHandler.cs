using k8s;
using k8s.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserController.CustomResources;

namespace UserController.Handlers
{
    class UserDeletedHandler
    {
        // delete sa
        // delete group ns if 0 sas

        private readonly Kubernetes _client;

        public UserDeletedHandler(Kubernetes client)
        {
            _client = client;
        }

        public void Handle(User user)
        {
            DeleteServiceAccount(user);
            DeleteNamespaceIfEmpty(user);
        }

        private void DeleteServiceAccount(User user)
        {
            var groupNamespaceName = user.Spec.GetGroupNamespace();
            var serviceAccountName = user.Metadata.Name;
            var serviceAccounts = _client.ListNamespacedServiceAccount(groupNamespaceName,
                                                                       fieldSelector: $"metadata.name={serviceAccountName}");
            if (serviceAccounts.Items.Any())
            {
                _client.DeleteNamespacedServiceAccount(serviceAccountName, groupNamespaceName);
                Console.WriteLine($"** Deleted service account: {serviceAccountName}, in group namespace: {groupNamespaceName}");
            }
        }

        private void DeleteNamespaceIfEmpty(User user)
        {
            var groupNamespaceName = user.Spec.GetGroupNamespace();
            var serviceAccountName = user.Metadata.Name;
            var serviceAccounts = _client.ListNamespacedServiceAccount(groupNamespaceName);

            if (serviceAccounts.Items.Count == 1 && serviceAccounts.Items[0].Metadata.Name == "default") 
            {
                _client.DeleteNamespace(groupNamespaceName);
                Console.WriteLine($"** No accounts left, deleted namespace: {groupNamespaceName}");
            }
        }
    }
}
