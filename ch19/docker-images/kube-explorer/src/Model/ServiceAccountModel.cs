namespace KubeExplorer.Model
{
    public class ServiceAccountModel
    {
        public string Name { get; set; }
        public bool AutomountServiceAccountToken { get; set; }
        public int SecretCount { get; set; }
    }
}
