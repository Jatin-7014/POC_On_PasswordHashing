using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using Poc_on_PasswordHashing.Models;

namespace Poc_on_PasswordHashing.Mappings
{
    public class LoginMap:ClassMap<User>
    {
        public LoginMap()
        {
            Table("Login");
            Id(u=>u.Id).GeneratedBy.Identity();
            Map(u => u.Username);
            Map(u=>u.Password);
        }
    }
}