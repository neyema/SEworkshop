﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SEWorkshop.DAL
{
    public class DatabaseProxy
    {
        private static readonly object padlock = new object();
        private static AppDbContext? instance = null;

        public static AppDbContext Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            //try
                            //{
                            //    instance = new RemoteDbContext();
                            //}
                            //catch
                            //{
                                instance = new LocalDbContext();
                            //}
                        }
                    }
                }
                return instance;
            }
        }

        public static void MoveToTestDb()
        {
            if (!(instance is TestDbContext))
            {
                instance = new TestDbContext();
            }
        }
    }
}
