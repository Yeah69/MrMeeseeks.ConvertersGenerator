using System.Globalization;
using System.Text;
using Microsoft.CodeAnalysis;

namespace MrMeeseeks.ConvertersGenerator;

internal record OptionsInfo(string RootNamespace);

[Generator]
public sealed class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var compilationProvider = context.CompilationProvider;
        
        context.RegisterSourceOutput(compilationProvider, (spc, _) =>
        {
            spc.AddSource("Generated.cs", "// Hello World!");
        });
        
        var analyzerConfigOptionsProvider = context.AnalyzerConfigOptionsProvider;
        context.RegisterImplementationSourceOutput(analyzerConfigOptionsProvider, (spc, configOptions) =>
        {
            var code = new StringBuilder();
            code.AppendLine("/*");
            
            foreach (var globalOptionsKey in configOptions.GlobalOptions.Keys)
            {
                code.AppendFormat(CultureInfo.InvariantCulture, "Key:   {0}", globalOptionsKey);
                code.AppendLine();
                if (configOptions.GlobalOptions.TryGetValue(globalOptionsKey, out var value))
                {
                    code.AppendFormat(CultureInfo.InvariantCulture, "Value: {0}", value);
                    code.AppendLine();
                }
                code.AppendLine();
            }
            
            code.AppendLine("*/");
            
            spc.AddSource("GlobalConfigOptions.g.cs", code.ToString());
        });
        
        context.RegisterPostInitializationOutput(c =>
        {
            c.AddSource(
                "Attribute.cs", 
                """
                using System;
                
                namespace MrMeeseeks.ConvertersGenerator
                {
                    [AttributeUsage(AttributeTargets.Class)]
                    public class WpfConvertersAttribute : Attribute
                    {
                        
                    }
                }
                """);
        });
    }
}
