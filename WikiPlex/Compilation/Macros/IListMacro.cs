﻿using System;

namespace WikiPlex.Compilation.Macros
{
    public interface IListMacro : IMacro
    {
        string ListStartScopeName { get; }
        string ListEndScopeName { get; }
        int DetermineLevel(string content);
    }
}