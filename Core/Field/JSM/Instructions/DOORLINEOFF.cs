﻿using System;


namespace OpenVIII.Fields
{
    internal sealed class DOORLINEOFF : JsmInstruction
    {
        public DOORLINEOFF()
        {
        }

        public DOORLINEOFF(Int32 parameter, IStack<IJsmExpression> stack)
            : this()
        {
        }

        public override String ToString()
        {
            return $"{nameof(DOORLINEOFF)}()";
        }
    }
}