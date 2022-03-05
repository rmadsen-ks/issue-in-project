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
    if (e.action == "closed" && !String.IsNullOrEmpty(inputs.ClosedProject))
    {
        var project = Github.GetProject(inputs.ClosedProject);
        var issueNumber = e.issue.id;
        var columns = await Github.Client.Repository.Project.Column.GetAll(project.Id);
        if (columns.FirstOrDefault(c => c.Name.Contains(inputs.ClosedColumn)) is ProjectColumn c)
        {
            Console.WriteLine($"Adding closed issue '#{e.issue.number}' to column '{c.Name}' of project '{project.Name}'");
            var r = await Github.Client.Repository.Project.Card.Create(c.Id, new NewProjectCard(issueNumber, ProjectCardContentType.Issue));
        }
        else
        {
            Console.WriteLine($"::error::Unable to find column '{inputs.ClosedColumn}' in project '{inputs.ClosedProject}'.");
            Console.WriteLine($"::error::Column names are: {String.Join(", ", columns.Select(cl => cl.Name))}.");
        }
    }
}

class Inputs : ActionInputs
{
    [ActionInputName("issue-closed-to-project")]
    public string ClosedProject { get; set; }

    [ActionInputName("issue-closed-to-column")]
    public string ClosedColumn { get; set; }


}