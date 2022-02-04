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

Console.WriteLine("Event json:");
Console.WriteLine(File.ReadAllText(Github.Env.EventPath));

if (Github.Env.EventName == "issues")
{
    var project = Github.GetProject(inputs.ClosedProject);
    Github.Client.Repository.Project.Card.Create(project.Id, new NewProjectCard(1, ProjectCardContentType.Issue));
}

class Inputs : ActionInputs
{
    [ActionInputName("issue-closed-to-project")]
    public string ClosedProject {get;set;}
}