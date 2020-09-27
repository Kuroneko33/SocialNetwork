using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class SearchViewModel
    {
        public List<User> Friends { get; set; } = new List<User>();
        public List<User> RequestsIn { get; set; } = new List<User>();
        public List<User> RequestsOut { get; set; } = new List<User>();
        public List<User> Users { get; set; } = new List<User>();
    }
}