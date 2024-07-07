﻿
// Services/Database.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Models;

namespace Todo.Services
{
    public class Database
    {
        public IEnumerable<TodoItem> GetItems() => new[]
        {
            new TodoItem {Description = "Check-lista"},
        };
    }
}
