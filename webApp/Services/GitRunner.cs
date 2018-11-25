using System;
using System.IO;
using System.Threading.Tasks;
using Octokit;

namespace webApp.Services
{
    public class GitRunner
    {
        public async Task<Repository> CreateAndRewind()
        {
            var client = new GitHubClient(new ProductHeaderValue("my-cool-app"));
            var basicAuth = new Credentials("hypo-test", "Thi7tael3ojoh"); // NOTE: not real credentials
            client.Credentials = basicAuth;
            

            Repository r = await client.Repository.Create(new NewRepository($"fake-test-{new Random().Next()}"));
            var dir = Path.Combine(Path.GetTempPath() + Guid.NewGuid().ToString("D"));
            
            LibGit2Sharp.Repository.Clone("https://github.com/libgit2/libgit2sharp.git", dir);

            return r;
        }
    }
}