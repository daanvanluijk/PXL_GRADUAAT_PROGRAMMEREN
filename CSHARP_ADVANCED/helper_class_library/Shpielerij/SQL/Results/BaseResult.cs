using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shpielerij.SQL.Results
{
    public class BaseResult
    {
        public bool Succeeded = false;
        public List<ErrorObject> Errors = new List<ErrorObject>();

        public void AddError(Exception e)
        {
            StackTrace trace = new StackTrace();
            int line = int.Parse(e.StackTrace.Split(new string[] { "line " }, StringSplitOptions.None)[1]);
            ErrorObject errorObject = new ErrorObject()
            {
                Exception = e,
                Function = trace.GetFrame(1).GetMethod().Name,
                File = (e.StackTrace.Split(new string[] {"in "}, StringSplitOptions.None)[1]).Replace($":line {line}", ""),
                Line = line,
        };
            Errors.Add(errorObject);
        }
    }

    public struct ErrorObject
    {
        public Exception Exception;
        public string Function;
        public string File;
        public int Line;
    }
}
