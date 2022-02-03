using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

// class Inputs : Dictionary<string, string>
// {
//     private readonly string[] RequiredInputKeys;
//     public Inputs(params string[] requiredInputNames)
//     {
//         RequiredInputKeys = requiredInputNames;
//     }

//     public void Parse(string[] args)
//     {
//         foreach(var key in RequiredInputKeys)
//         {
//             var val = Environment.GetEnvironmentVariable(key);
//             if(val != null)
//                 this.Add(key, val);
//         }
//         foreach (var arg in args)
//         {
//             if (arg.Contains('='))
//             {
//                 var option = arg.Split('=').First();
//                 var value = arg.Substring(2);
//                 if (string.IsNullOrWhiteSpace(value))
//                     continue;
//                 this[option] = value;
//             }
//         }
//     }
// }

class ActionInputs
{
    private class EnvVarName : Attribute
    {
        string Name { get; }
        public EnvVarName(string name)
        {
            this.Name = name;
        }
    }

    private class CliOptionName : Attribute
    {
        string Name { get; }
        public CliOptionName(string name)
        {
            this.Name = name;
        }
    }

    [EnvVarName("token")]
    [CliOptionName("token")]
    public virtual string Token { get; set; }

    [EnvVarName("GITHUB_REPOSITORY_OWNER")]
    public virtual string Owner { get; set; }

    [EnvVarName("GITHUB_REPOSITORY")]
    public virtual string Repo { get; set; }

    public ActionInputs(string[] args)
    {
        Dictionary<string, string> options = new Dictionary<string, string>();
        foreach (var arg in args)
        {
            if (arg.Contains('='))
            {
                var option = arg.Split('=').First();
                var value = arg.Substring(2);
                if (string.IsNullOrWhiteSpace(value))
                    continue;
                options[option] = value;
            }
        }

        var props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.FlattenHierarchy);
        foreach(var prop in props)
        {
            var attrs = prop.GetCustomAttributesData();
            var optionName = attrs.FirstOrDefault(a => a.AttributeType == typeof(CliOptionName))?.ConstructorArguments[0].Value as String;
            var envVarName = attrs.FirstOrDefault(a => a.AttributeType == typeof(EnvVarName))?.ConstructorArguments[0].Value as String;
            if (optionName is not null && options.TryGetValue(optionName, out var val))
            {
                prop.SetValue(this, val);
            }
            else if(envVarName is not null && Environment.GetEnvironmentVariable(envVarName) is string val2)
            {
                prop.SetValue(this, val2);
            }
        }
    }
}