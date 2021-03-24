using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;

namespace Infra.Docker
{
    public class DockerService : IDockerService
    {
        private readonly IDockerClient _client;

        public DockerService() => _client = new DockerClientConfiguration().CreateClient();

        public RunResult RunContainer(RunParameters runParameters)
        {
            if (!runParameters.HasContainerName)
            {
                runParameters.GenerateContainerName(); 
            }

            PullImage(runParameters.FromImage, runParameters.Tag);
            CreateContainer(runParameters.FromImage, runParameters.Tag, runParameters.ContainerName, runParameters.HostPort, runParameters.ExposedPort, volume: runParameters.Volume);
            bool result = StartContainer(runParameters.ContainerName);

            return new RunResult() 
            {
                ContainerName = runParameters.ContainerName,
                ImageName = runParameters.FromImage,
                ImageTag = runParameters.Tag,
                Success = result
            };
        }

        public CreateContainerResponse CreateContainer(string image, string tag, string containerName, string hostPort, string exposedPort, string volume = null) 
        {
             var task = _client.Containers.CreateContainerAsync(
                new CreateContainerParameters()
                {
                    Image = $"{image}:{tag}",
                    Name = containerName,
                    Volumes = new Dictionary<string, EmptyStruct>() 
                    { 
                        { 
                            volume, default 
                        } 
                    },
                    ExposedPorts = new Dictionary<string, EmptyStruct>() 
                    {
                        { exposedPort, default } 
                    },
                    HostConfig = new HostConfig()
                    { 
                        PortBindings = new Dictionary<string, IList<PortBinding>>()
                        { 
                             { exposedPort, new List<PortBinding> { new PortBinding { HostPort = hostPort } } } 
                        }
                    }
                });

            return task.Result; 
        }

        public bool StartContainer(string containerName)
        {
            var task = _client.Containers.StartContainerAsync(containerName, new ContainerStartParameters { });
            return task.Result;
        }

        public void PullImage(string fromImage, string tag) 
        {
            var progress = new Progress<JSONMessage>();

            _client.Images.CreateImageAsync(new ImagesCreateParameters()
            {
                FromImage = fromImage,
                Tag = tag
            }, null, progress).Wait();
        }

        public bool StopContainer(string containerName) 
        {
            var task = _client.Containers.StopContainerAsync(containerName, new ContainerStopParameters());
            return task.Result;
        }

        public void RemoveContainer(string containerName) 
        {
            _client.Containers.RemoveContainerAsync(containerName, new ContainerRemoveParameters()).Wait();
        }

        public void RemoveImage(string name, string tag) 
        {
            _client.Images.DeleteImageAsync($"{name}:{tag}", new ImageDeleteParameters()).Wait();
        }
    }
}
