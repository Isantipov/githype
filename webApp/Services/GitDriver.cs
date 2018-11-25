using System;
using System.IO;
using System.Linq;
using LibGit2Sharp;

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