﻿using System;


namespace OpenVIII.Fields
{
    internal sealed class MENUSHOP : JsmInstruction
    {
        private IJsmExpression _arg0;

        public MENUSHOP(IJsmExpression arg0)
        {
            _arg0 = arg0;
        }

        public MENUSHOP(Int32 parameter, IStack<IJsmExpression> stack)
            : this(
                arg0: stack.Pop())
        {
        }

        public override String ToString()
        {
            return $"{nameof(MENUSHOP)}({nameof(_arg0)}: {_arg0})";
        }
    }
}