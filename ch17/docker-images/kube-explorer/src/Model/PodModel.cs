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
