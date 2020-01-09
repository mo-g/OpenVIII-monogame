﻿using System;


namespace OpenVIII.Fields
{
    internal sealed class REFRESHPARTY : JsmInstruction
    {
        public REFRESHPARTY()
        {
        }

        public REFRESHPARTY(Int32 parameter, IStack<IJsmExpression> stack)
            : this()
        {
        }

        public override String ToString()
        {
            return $"{nameof(REFRESHPARTY)}()";
        }
    }
}