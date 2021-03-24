using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Infra.Docker
{
    /// <summary>
    /// Command run Parameters 
    /// </summary>
    public class RunParameters
    {
        public RunParameters(string fromImage, string tag)
        {
            FromImage = fromImage;
            Tag = tag;
        }

        public string FromImage { get; }
        public string Tag { get; }
        public string ContainerName { get; set; }
        public string HostPort { get; set; }
        public string ExposedPort { get; set; }
        public string  Volume { get; set; }

        public bool HasContainerName => !string.IsNullOrEmpty(ContainerName);

        public void GenerateContainerName() 
        {
            string containerName = $"{FromImage}-{Guid.NewGuid()}";
            var invalidCharacters = Regex.Replace(containerName, @"[a-zA-Z0-9_.-]", string.Empty);
            invalidCharacters.ToList().ForEach(x => ContainerName = containerName.Replace(x, '-'));
        }
    }
}
