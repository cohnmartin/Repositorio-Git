using System;
using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.JScript;

///
/// Implements JScript/JavaScript Eval like
/// functionality in C# to allow dynamic evaluation
/// of expressions.
///
public static class ExpressionEvaluator
{
    #region Private Members

    private static Type _evaluatorType;
    private static object _evaluatorInstance;
    private static readonly string _jscriptEvalClass =
    @"import System;
    class JScriptEvaluator
    {
    public static function Eval(expression : String) : String
    {
        return eval(expression);
    }
    }";

    #endregion

    #region Private Methods

    ///
    /// Creates a dynamic in-memory assembly using JScript.NET
    /// to evaluate expressions.
    ///
    private static void Initialize()
    {

        CodeDomProvider compiler = new JScriptCodeProvider();

        CompilerParameters parameters = new CompilerParameters();
        parameters.GenerateInMemory = true;
        parameters.ReferencedAssemblies.Add("system.dll");

        CompilerResults results = compiler.CompileAssemblyFromSource(parameters, _jscriptEvalClass);

        Assembly assembly = results.CompiledAssembly;
        _evaluatorType = assembly.GetType("JScriptEvaluator");
        _evaluatorInstance = Activator.CreateInstance(_evaluatorType);
    }

    #endregion

    #region Public Methods

    ///
    /// Evaluates specified expression using reflection
    /// on the assembly generated in Initialize() method.
    ///
    /// The expression.
    /// A string representing evaluated expression.
    public static string Eval(string expression)
    {
        if (_evaluatorInstance == null)
            Initialize();

        object result = _evaluatorType.InvokeMember(
        "Eval",
       BindingFlags.InvokeMethod,
        null,
       _evaluatorInstance,
        new object[] { expression }
       );

        return (result == null) ? null : result.ToString();

    }

    #endregion
}