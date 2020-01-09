﻿using System;


namespace OpenVIII.Fields
{
    internal sealed class LINEON : JsmInstruction
    {
        public LINEON()
        {
        }

        public LINEON(Int32 parameter, IStack<IJsmExpression> stack)
            : this()
        {
        }

        public override String ToString()
        {
            return $"{nameof(LINEON)}()";
        }
    }
}