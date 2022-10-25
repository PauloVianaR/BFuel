using System;
using System.Collections.Generic;
using System.Text;

namespace BFuel.Domain.Models
{
    public class UserTypes
    {
        public enum Types
        {
            Admin,
            Email_Pass,
            Google,
            Facebook
        }
    }
}
