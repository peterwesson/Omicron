using System;

namespace Omicron.Lanuage.Operators
{
    [Flags]
    public enum OperatorType
    {
        PreParamaeterOperator = 1,
        PostParameterOperator = 2,
        TwoParameterOperator = 4
    }
}