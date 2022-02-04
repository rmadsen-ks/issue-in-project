using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Octokit;

class Github
{

    public static GithubEnvironment Env = new GithubEnvironment();

    static Github()
    {
        Client = new GitHubClient(new ProductHeaderValue("issue-in-project"));
    }

    public static GitHubClient Client 
    {
        get; private set;
    }

    private static Repository _Repository;
    public static Repository Repository 
    {
        get
        {
            if (_Repository is null)
            {
                var repo = Environment.GetEnvironmentVariable("GITHUB_REPOSITORY");
                _Repository = Client.Repository.Get(repo.Split("/").First(), repo.Split("/").Last()).Result;
            }
            return _Repository;
        }
    }

    public static Project GetProject(string projectName)
    {
        string owner = projectName.Split("/")[0];
        string repo = projectName.Split("/")[1];
        int id = int.Parse(projectName.Split("/")[2]);
        var projects = Github.Client.Repository.Project.GetAllForRepository(owner, repo).Result;
        return projects.FirstOrDefault(p => p.Number == id);
    }
}

class GithubEnvironment
{
    /// <summary>
    /// Always set to true when GitHub Actions is running the workflow. You can use this variable to differentiate when 
    /// tests are being run locally or by GitHub Actions.
    /// </summary>
    public bool Actions => Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true";
    /// <summary>
    /// The owner and repository name. For example, octocat/Hello-World.
    /// </summary>
    public string Repository => Environment.GetEnvironmentVariable("GITHUB_REPOSITORY");
    /// <summary>
    /// The type of ref that triggered the workflow run. Valid values are branch or tag.
    /// </summary>
    public string RefType => Environment.GetEnvironmentVariable("GITHUB_REF_TYPE");
    /// <summary>
    /// The branch or tag ref that triggered the workflow run. For branches this is the format refs/heads/<branch_name>, 
    /// and for tags it is refs/tags/<tag_name>. This variable is only set if a branch or tag is available for the event 
    /// type. For example, refs/heads/feature-branch-1.
    /// </summary>
    public string Ref => Environment.GetEnvironmentVariable("GITHUB_REF");
    public string EventName => Environment.GetEnvironmentVariable("GITHUB_EVENT_NAME");
    /// <summary>
    /// The branch or tag name that triggered the workflow run. For example, feature-branch-1.
    /// </summary>
    public string RefName => Environment.GetEnvironmentVariable("GITHUB_REF_NAME");
    /// <summary>
    /// true if branch protections are configured for the ref that triggered the workflow run.
    /// </summary>
    public bool RefProtected => Environment.GetEnvironmentVariable("GITHUB_REF_PROTECTED") == "true";
    /// <summary>
    /// The commit SHA that triggered the workflow. For example, ffac537e6cbbf934b08745a378932722df287a53.
    /// </summary>
    public string Sha => Environment.GetEnvironmentVariable("GITHUB_SHA");
}

class GithubConsole
{
    public static void WriteOutput(string outputName, string value)
    {
        Console.WriteLine($"::set-output name={outputName}::{value}");
    }
}