using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

//System.Func<double> d = () => System.Math.Sqrt(4);
namespace Script
{
    public interface IScriptEngine
    {
        object Execute(string code);
    }

    public interface ICSharpScriptEngine : IScriptEngine
    {
        
    }

    public class CSharpScriptEngine : ICSharpScriptEngine
    {
        private Globals _globals = new Globals { x = 1, y = 2 };
        private ScriptState<object> _scriptState;
        public object Execute(string code)
        {
            _scriptState = _scriptState == null ?
                CSharpScript.RunAsync(code, globals: _globals).Result :
                _scriptState.ContinueWithAsync(code).Result;
            if (!string.IsNullOrEmpty(_scriptState.ReturnValue?.ToString()))
                return _scriptState.ReturnValue;
            return null;
        }
    }

    public class Globals
    {
        public int x;
        public int y;
        public int z { get { return 5; } }
    }
}
