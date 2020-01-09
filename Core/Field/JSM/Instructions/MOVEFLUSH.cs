﻿using System;


namespace OpenVIII.Fields
{
    internal sealed class MOVEFLUSH : JsmInstruction
    {
        public MOVEFLUSH()
        {
        }

        public MOVEFLUSH(Int32 parameter, IStack<IJsmExpression> stack)
            : this()
        {
        }

        public override String ToString()
        {
            return $"{nameof(MOVEFLUSH)}()";
        }
    }
}