﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.DataAccess.DTO
{
    public class NewRequestDaTa
    {
        [Key] public int newsID { set; get; }
        public string? NewsName { set; get; }
        public string? NewsMain { set; get; }
    }

    public class NewInsertUpdateRequestDaTa : NewRequestDaTa
    {

    }


    public class NewDeleteRequestDaTa : NewRequestDaTa
    {

    }
}
