using System;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Npgsql.EntityFrameworkCore;

namespace postgres1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initialize connection...");
            var context = GetContext();

            Console.WriteLine("Check connection...");

            using(var c = new NpgsqlConnection(context.Database.GetDbConnection().ConnectionString))
            {
                c.Open();
                c.Close();
            }

            Console.WriteLine("Ok");

            Console.WriteLine("Input command");
            var line = string.Empty;

            while(true)
            {
                line = Console.ReadLine();

                if(line == "exit")
                {
                    break;
                }
                else if(line == "show")
                {
                    foreach(var v in context.Values)
                    {
                        Console.WriteLine("{0}: {1}", v.Id, v.Data);
                    }
                }
                else if(line.StartsWith("add"))
                {
                    var _v = line.Substring(4);
                    context.Values.Add(new Value
                    {  
                        Id = Guid.NewGuid(),
                        Data = _v
                    });

                    context.SaveChanges();

                    Console.WriteLine("added");
                }
                else if(line.StartsWith("delete"))
                {
                    Guid _id;
                    if(!Guid.TryParse(line.Substring(7), out _id))
                    {
                        Console.WriteLine("Invalid Id: {0}", line.Substring(7));
                        continue;
                    }

                    var _entity = context.Values.Find(_id);

                    if(_entity == null)
                    {
                        Console.WriteLine("Not found");
                        continue;
                    }

                    context.Values.Remove(_entity);
                    context.SaveChanges();

                    Console.WriteLine("deleted");
                }
                else if(line.StartsWith("update"))
                {
                    string _idStr = line.Substring(7).Split(' ')[0];
                    string _data = line.Substring(7 + _idStr.Length + 1);

                    Guid _id;
                    if(!Guid.TryParse(_idStr, out _id))
                    {
                        Console.WriteLine("Invalid Id: {0}", _idStr);
                        continue;
                    }

                    var _entity = context.Values.Find(_id);
                    if(_entity == null)
                    {
                        Console.WriteLine("Not found");
                        continue;
                    }

                    _entity.Data = _data;
                    context.SaveChanges();

                    Console.WriteLine("updated");
                }
                else
                {
                    Console.WriteLine("Unknown command");
                    continue;
                }

            }
        } // Main

        static GeneralContext GetContext()
        {
            var f = Path.Combine(
                Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                "connection");
            
            if(!File.Exists(f))
            {
                throw new Exception("File \"connection\" not found. You should to create \"connection\" file with valid PostgreSQL connection string.");
            }

            var connStr = File.ReadAllText(f);
            var ob = new DbContextOptionsBuilder();
            ob.UseNpgsql(connStr);
            
            return new GeneralContext(ob.Options);
        }
    }
}
