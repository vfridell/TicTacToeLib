using System;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Text;
using Microsoft.Extensions.Options;

namespace TicTacToeGame
{
    public class GraphvizDriver : IDisposable
    {
        private string _graphvizPath;

        public GraphvizDriver(IOptions<TicTacToeGameOptions> options)
        {
            _graphvizPath = options.Value.GraphvizPath;
        }

        private Runspace _runspace = null!;
        internal Runspace PSRunspace
        {
            get
            {
                if (null == _runspace)
                {
                    _runspace = GetRunspace();
                    _runspace.Open();
                }
                return _runspace;
            }
        }

        private Runspace GetRunspace()
        {
            return RunspaceFactory.CreateRunspace();
        }

        internal bool TryGetPowershellErrors(PowerShell powershell, out string errors)
        {
            errors = "";
            if (powershell.Streams.Error.Count == 0) return false;
            StringBuilder errSB = new StringBuilder();
            foreach (ErrorRecord err in powershell.Streams.Error)
            {
                errSB.Append(err.ToString()).Append('\n');
            }
            errors = errSB.ToString();
            return true;
        }

        public enum OutputFormat { SVG, PDF, PNG };

        public bool TryGenerateGraph(string dotFilename, OutputFormat outputFormat, out string imageFilename)
        {
            try
            {
                string ext = outputFormat.ToString().ToLower();

                imageFilename = $"{dotFilename.Replace(".dot", "")}.{ext}";
                using (PowerShell powershell = PowerShell.Create())
                {
                    string path = $"{_graphvizPath}dot";
                    powershell.AddCommand("Start-Process");
                    powershell.AddParameter("FilePath", path);
                    powershell.AddParameter("ArgumentList", $@"-T{ext} -o""{imageFilename}"" ""{dotFilename}""");
                    powershell.Runspace = PSRunspace;
                    Collection<PSObject> objs = powershell.Invoke();
                    if (TryGetPowershellErrors(powershell, out string errors))
                    {
                        imageFilename = "";
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                imageFilename = "";
                return false;
            }
        }

        public void Dispose()
        {
            if (null != _runspace)
            {
                _runspace.Dispose();
            }
        }

    }
}
