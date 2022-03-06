using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

abstract class ActionInputs
{
    protected class ActionInputName : Attribute
    {
        string Name { get; }
        public ActionInputName(string name)
        {
            this.Name = name;
        }
    }

    [ActionInputName("token")]
    public virtual string Token { get; set; }

    public ActionInputs()
    {
        var props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.FlattenHierarchy);
        foreach (var prop in props)
        {
            var attrs = prop.GetCustomAttributesData();
            var inputName = attrs.FirstOrDefault(a => a.AttributeType == typeof(ActionInputName))?.ConstructorArguments[0].Value as String;
            if (inputName is not null)
            {
                // if the action is run using "env:" the inputs are in env vars like this:    
                if (Environment.GetEnvironmentVariable(inputName) is string val1)
                    prop.SetValue(this, val1);

                // If the action is run using "with:" the inputs are in env vars like this:
                if (Environment.GetEnvironmentVariable("INPUT_" + inputName.ToUpper()) is string val2)
                    prop.SetValue(this, val2);

            }
        }
    }
}