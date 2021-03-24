namespace Infra.Docker
{
    /// <summary>
    /// Command run result 
    /// </summary>
    public class RunResult
    {
        public string ImageName { get; set; }
        public string ImageTag { get; set; }
        public string ContainerName { get; set; }
        public bool Success { get; set; }
    }
}
