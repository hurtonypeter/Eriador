
using Microsoft.AspNet.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Eriador.Models.Data.Entity
{
    public class User : IdentityUser<int>
    {
        public string FullName { get; set; }
    }
}