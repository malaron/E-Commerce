using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedContracts.Enum
{
    public enum LoginResult
    {
        Success,
        RequiresTwoFactor,
        IsLockedOut,
        InvalidAttempt
    }
}
