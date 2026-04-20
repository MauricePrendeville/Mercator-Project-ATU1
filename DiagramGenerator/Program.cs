using MermaidClassDiagramGenerator;
using System.Reflection;
using VendorProcessManagerV1.Models;

//partial class Program
//{
var outputPath = Path.Combine(
    Directory.GetCurrentDirectory(),
    "classDiagram.mmd");

var assembliesToScan = new List<Assembly>
{
    typeof(ProcessTemplate).Assembly
};


var domainTypes = new List<Type>
{
    typeof(ProcessInstance),
    typeof(ProcessTask),
    typeof(AuditLog),
    typeof(ProcessTemplateTask),
    typeof(ProcessTemplateTransition),
    typeof(ProcessTaskTransition)
};

var generator = new DiagramGenerator(
    outputPath,
    assembliesToScan,
    domainTypes,
    generateWithoutProperties: false
    );

generator.Generate();
Console.WriteLine("Mermaid class diagram generated");



