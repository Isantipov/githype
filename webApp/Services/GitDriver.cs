using System;
using System.IO;
using System.Linq;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using Octokit;
using Commit = LibGit2Sharp.Commit;
using Repository = LibGit2Sharp.Repository;
using Signature = LibGit2Sharp.Signature;

namespace webApp.Services
{
    public class GitDriver
    {
        private readonly User _user;
        private readonly string _pwd;

        public GitDriver(User user, string pwd)
        {
            _user = user;
            _pwd = pwd;
        }

        public void CloneAndUnwind(string newRepoUrl)
        {
            var dir = Path.Combine(Path.GetTempPath() + Guid.NewGuid().ToString("D"));
            
            var repoPath = LibGit2Sharp.Repository.Clone("https://github.com/Isantipov/fb_groups_intersector", dir);
            var repo = new Repository(repoPath);
            var originalRefs = repo.Refs.ToList().OrderBy(r => r.CanonicalName);
            var commits = repo.Commits.QueryBy(new CommitFilter { IncludeReachableFrom = repo.Refs }).ToArray();

            // Noop header rewriter
            repo.Refs.RewriteHistory(new RewriteHistoryOptions
            {
//                OnError = OnError,
//                OnSucceeding = OnSucceeding,
                CommitHeaderRewriter = ThisUser,
            }, commits);

            var newRemote = repo.Network.Remotes.Add("new", newRepoUrl);
            
            PushOptions options = new PushOptions();
            options.CredentialsProvider = new CredentialsHandler(
                (url, usernameFromUrl, types) =>
                    new UsernamePasswordCredentials()
                    {
                        Username = _user.Login,
                        Password = _pwd
                    });

            repo.Network.Push(newRemote, "refs/heads/master", options);
        }
        
        public  CommitRewriteInfo ThisUser(Commit commit)
        {
            var sg = new Signature(string.IsNullOrEmpty(_user.Name) ? _user.Login : _user.Name, _user.Email,
                commit.Author.When);
            return new CommitRewriteInfo
            {
                Author = sg,
                Committer = sg,
                Message = commit.Message
            };
        }
    }
}