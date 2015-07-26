﻿using System;
using System.Collections.Generic;
using System.Linq;
using GitVersion;

public class AssemblyInfoBuilder
{
    public string GetAssemblyInfoText(VersionVariables vars, string assemblyName)
    {
        var v = vars.ToList();

        var assemblyInfo = string.Format(
@"using System;
using System.Reflection;

[assembly: AssemblyVersion(""{0}"")]
[assembly: AssemblyFileVersion(""{1}"")]
[assembly: AssemblyInformationalVersion(""{2}"")]

namespace {4}
{{

    [System.Runtime.CompilerServices.CompilerGenerated]
    static class GitVersionInformation
    {{
{3}
    }}

}}
",
        vars.AssemblySemVer,
        vars.MajorMinorPatch + ".0",
        vars.InformationalVersion,
        GenerateStaticVariableMembers(v),
        assemblyName);

        return assemblyInfo;
    }

    static string GenerateStaticVariableMembers(IList<KeyValuePair<string, string>> vars)
    {
        return GenerateMembers(vars, "        public static string {0} = \"{1}\";");
    }


    static string GenerateMembers(IList<KeyValuePair<string, string>> vars, string memberFormat)
    {
        return string.Join(Environment.NewLine, vars.Select(v => string.Format(memberFormat, v.Key, v.Value)));
    }
}