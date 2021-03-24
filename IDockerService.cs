using Docker.DotNet.Models;

namespace Infra.Docker
{
    /// <summary>
    /// Docker Service contract
    /// </summary>
    public interface IDockerService
    {
        /// <summary>
        /// Command run, pull image, create and run container.
        /// </summary>
        /// <param name="runParameters">Command run paramenters</param>
        /// <returns>Result infos</returns>
        RunResult RunContainer(RunParameters runParameters);

        /// <summary>
        /// Create the container
        /// </summary>
        /// <param name="image">Image name</param>
        /// <param name="tag">Image version</param>
        /// <param name="containerName">Container name</param>
        /// <param name="hostPort">Host port</param>
        /// <param name="exposedPort">Exposed port</param>
        /// <param name="volume">Volume</param>
        /// <returns>Command result</returns>
        CreateContainerResponse CreateContainer(string image, string tag, string containerName, string hostPort, string exposedPort, string volume = null);

        /// <summary>
        /// Start the container
        /// </summary>
        /// <param name="containerName">Container name</param>
        /// <returns>Success</returns>
        bool StartContainer(string containerName);

        /// <summary>
        /// Pull the image
        /// </summary>
        /// <param name="fromImage">Image name</param>
        /// <param name="tag">Image version</param>
        void PullImage(string fromImage, string tag);

        /// <summary>
        /// Stop the container
        /// </summary>
        /// <param name="containerName">Container name</param>
        /// <returns>Success</returns>
        bool StopContainer(string containerName);

        /// <summary>
        /// Remove the container
        /// </summary>
        /// <param name="containerName">Container name</param>
        void RemoveContainer(string containerName);

        /// <summary>
        /// Remove the image
        /// </summary>
        /// <param name="name">Image name</param>
        /// <param name="tag">Image version</param>
        void RemoveImage(string name, string tag);
    }
}
