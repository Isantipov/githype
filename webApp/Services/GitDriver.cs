using System;
using System.IO;
using System.Linq;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;

namespace webApp.Services
{
    public class GitDriver
    {
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
                        Username = Doer.UserName,
                        Password = Doer.Password
                    });

            repo.Network.Push(newRemote, "refs/heads/master", options);
        }
        
        public static CommitRewriteInfo ThisUser(Commit commit)
        {
            return new CommitRewriteInfo
            {
                Author = new Signature("hypo-test", "ivan.antsipau2@gmail.com", commit.Author.When),
                Committer = new Signature("hypo-test", "ivan.antsipau2@gmail.com", commit.Author.When),
                Message = commit.Message
            };
        }
    }
}