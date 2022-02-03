using Octokit;
using System.Collections;
using System.Diagnostics;

var inputs = new Inputs(args);

Github.Client.Credentials = new Credentials(inputs.Token);

var repoid = Github.Repository.Id;

Console.WriteLine(Environment.CommandLine);

Console.WriteLine("::group::Variables:");
foreach (DictionaryEntry v in Environment.GetEnvironmentVariables())
    Console.WriteLine($"  {v.Key}={v.Value}");
Console.WriteLine("::endgroup::");


class Inputs : ActionInputs
{
    public string owner { get; set; } = "AsgerIversen";
    public string reponame { get; set; } = "test";
    public string token { get; set; } = "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";
    public string body { get; set; } = "This change is part of version `{version}` or later.";

    public Inputs(string[] args) : base(args) { }
}