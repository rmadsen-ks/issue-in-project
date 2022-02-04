using GithubUtils;
using Octokit;
using System.Collections;
using System.Diagnostics;

var inputs = new Inputs();

Github.Client.Credentials = new Credentials(inputs.Token);

var repoid = Github.Repository.Id;

Console.WriteLine(Environment.CommandLine);

Console.WriteLine("::group::Variables:");
foreach (DictionaryEntry v in Environment.GetEnvironmentVariables())
    Console.WriteLine($"  {v.Key}={v.Value}");
Console.WriteLine("::endgroup::");

Console.WriteLine("::group::Event json:");
Console.WriteLine(File.ReadAllText(Github.Env.EventPath));
Console.WriteLine("::endgroup::");

if (Github.Env.EventName == "issues")
{
    var e = Github.GetEvent<GithubUtils.Event.IssueEvent>();
    if (e.action == "closed")
    {
        var project = Github.GetProject(inputs.ClosedProject);
        Console.WriteLine($"Adding closed issue '#{e.issue.number}' to project '{project.Name}'");
        Github.Client.Repository.Project.Card.Create(project.Id, new NewProjectCard(e.issue.number, ProjectCardContentType.Issue));
    }
}

class Inputs : ActionInputs
{
    [ActionInputName("issue-closed-to-project")]
    public string ClosedProject {get;set;}
}