﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Entities.LogModels
{
    public class LogDetails
    {
        public Object? Controller { get; set; }
        public Object? ModelName { get; set; }
        public Object? Id { get; set; }
        public Object? CreateAt { get; set; }
        public Object? Action { get; set; }

        public LogDetails()
        {
            CreateAt = DateTime.UtcNow;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
