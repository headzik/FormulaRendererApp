using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FormulaRendererApp.Services
{
    public interface IFileSystem
    {
        string GetDownloadPath();
        string GetDownloadName();
    }
}
