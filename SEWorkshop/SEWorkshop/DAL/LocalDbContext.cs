﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SEWorkshop.DAL
{
    class LocalDbContext : AppDbContext
    {
        private const string DB_NAME = "AzamazonLocal.mdf";
        private static readonly string dbPath =
            @"C:\Users\tomer\Source\Repos\SEworkshop3\SEWorkshop\SEWorkshop\DAL\AzamazonLocal.mdf";

        public LocalDbContext() : base(string.Format(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={0};Integrated Security=True", dbPath))
        {

        }
    }
}
