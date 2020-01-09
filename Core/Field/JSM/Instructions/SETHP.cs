﻿using System;


namespace OpenVIII.Fields
{
    internal sealed class SETHP : JsmInstruction
    {
        private IJsmExpression _arg0;
        private IJsmExpression _arg1;

        public SETHP(IJsmExpression arg0, IJsmExpression arg1)
        {
            _arg0 = arg0;
            _arg1 = arg1;
        }

        public SETHP(Int32 parameter, IStack<IJsmExpression> stack)
            : this(
                arg1: stack.Pop(),
                arg0: stack.Pop())
        {
        }

        public override String ToString()
        {
            return $"{nameof(SETHP)}({nameof(_arg0)}: {_arg0}, {nameof(_arg1)}: {_arg1})";
        }
    }
}