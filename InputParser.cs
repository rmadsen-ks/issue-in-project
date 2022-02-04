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
        public CliOptionName(string name)
        {
            this.Name = name;
        }
    }

    [ActionInputName("token")]
    public virtual string Token { get; set; }

    public ActionInputs()
    {
        var props = this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.FlattenHierarchy);
        foreach(var prop in props)
        {
            var attrs = prop.GetCustomAttributesData();
            var inputName = attrs.FirstOrDefault(a => a.AttributeType == typeof(ActionInputName))?.ConstructorArguments[0].Value as String;
            else if(inputName is not null && Environment.GetEnvironmentVariable("INPUT_" + inputName.ToUpper()) is string val2)
            {
                prop.SetValue(this, val2);
            }
        }
    }
}