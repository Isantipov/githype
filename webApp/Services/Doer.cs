using System;
using System.IO;
using System.Threading.Tasks;
using Octokit;

namespace webApp.Services
{
    public class Doer
    {
        public async Task<Repository> CreateAndRewind(string login, string pwd)
        {
            var client = new GitHubClient(new ProductHeaderValue("my-cool-app"));
//            var userName = "hypo-test";
//            var pwd = "Thi7tael3ojoh";
            var credentials = new Credentials(login, pwd);
            var basicAuth = credentials; // NOTE: not real credentials
            client.Credentials = basicAuth;
            
            

            Repository r = await client.Repository.Create(new NewRepository($"fake-test-{new Random().Next()}"));
            var driver = new GitDriver(client.User.Current().Result, pwd);
            driver.CloneAndUnwind(r.SvnUrl);
            

            return r;
        }

//        public static string UserName { get; } = "hypo-test";
//        public static string Password { get; } = "Thi7tael3ojoh";
    }
    
    
}