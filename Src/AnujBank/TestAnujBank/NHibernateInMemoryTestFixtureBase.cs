using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace TestAnujBank
{
    public class NHibernateInMemoryTestFixtureBase
    {
        protected static ISessionFactory sessionFactory;
        protected static Configuration configuration;

        public static void InitalizeSessionFactory(params Assembly[] assemblies)
        {
            if (sessionFactory != null)
                return;

            var properties = new Dictionary<string, string>();
            properties.Add("connection.driver_class", "NHibernate.Driver.SQLite20Driver");
            properties.Add("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
            properties.Add("dialect", "NHibernate.Dialect.SQLiteDialect");
            properties.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
            properties.Add("connection.connection_string", "Data Source=:memory:;Version=3;New=True;");
            properties.Add("connection.release_mode", "auto");
            properties.Add("show_sql", "true");

            configuration = new Configuration();
            configuration.Properties = properties;

            foreach (Assembly assembly in assemblies)
            {
                configuration = configuration.AddAssembly(assembly);
            }
            configuration.BuildMapping();
            sessionFactory = configuration.BuildSessionFactory();
        }
        //name="proxyfactory.factory_class">NHibernate.ByteCode.Castle.ProxyFactoryFactory,NHibernate.ByteCode
        public ISession CreateSession()
        {
            ISession openSession = sessionFactory.OpenSession();
            IDbConnection connection = openSession.Connection;
            using (var writer = new StringWriter())
            {
                new SchemaExport(configuration).Execute(false, true, false, connection, writer);
                Console.WriteLine(writer);

            }
            return openSession;
        }

    }
}
