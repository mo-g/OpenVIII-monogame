﻿using System;


namespace OpenVIII.Fields
{
    internal sealed class MAPJUMPON : JsmInstruction
    {
        public MAPJUMPON()
        {
        }

        public MAPJUMPON(Int32 parameter, IStack<IJsmExpression> stack)
            : this()
        {
        }

        public override String ToString()
        {
            return $"{nameof(MAPJUMPON)}()";
        }
    }
}