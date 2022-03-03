﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Domain.Security
{
    public class JwtSettings
    {
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string Secret { get; set;}
        public string TokenValidityInMinutes { get; set; }
        public string RefreshTokenValidityInDays { get; set; }
    }
}
