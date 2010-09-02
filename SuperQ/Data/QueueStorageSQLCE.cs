using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperQ.Data;
using System.Data.SqlServerCe;
using System.IO;
using System.Data;

namespace SuperQ.Data
{
    public class QueueStorageSQLCE : IQueueStorage
    {
        private static readonly object _lock = new object();
        private string _name;
        private string _connection;

        public void CreateIfRequired(string name)
        {
            _name = name;
            _connection = string.Format(@"data source=|DataDirectory|\{0}.sdf", name);
            string sdfPath = string.Empty;

            //get path
            using (var testConn = new SqlCeConnection(_connection))
            {
                sdfPath = testConn.Database;
            }

            //thread safety first
            if (!File.Exists(sdfPath))
            {
                lock (_lock)
                {
                    if (!File.Exists(sdfPath))
                    {
                        //create le db
                        using (var engine = new SqlCeEngine(_connection))
                        {
                            engine.CreateDatabase();
                        }

                        //script tables
                        string createScript = @"CREATE TABLE [Queue] (
                                                  [QueueID] uniqueidentifier NOT NULL default NEWID() PRIMARY KEY
                                                , [Message] nvarchar(4000) NOT NULL
                                                , [Added] datetime NOT NULL
                                                , [Retrieved] datetime NULL
                                                );";

                        using (var conn = new SqlCeConnection(_connection))
                        {
                            conn.Open();
                            using (var cmd = new SqlCeCommand())
                            {
                                cmd.Connection = conn;
                                cmd.CommandText = createScript;
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }

        }

        public void PushMessage<T>(QueueMessage<T> message)
        {
            string sql = @"INSERT INTO [Queue] (Message, Added) VALUES(@Message, @Added)";
            string deserialized = Helpers.Serialization.ToXml(message.Payload);

            using (SqlCeConnection conn = new SqlCeConnection(_connection))
            {
                conn.Open();

                using (SqlCeCommand cmd = new SqlCeCommand(sql))
                {
                    cmd.Connection = conn;
                    cmd.Parameters.Add("@Message", SqlDbType.NVarChar, 4000).Value = deserialized;
                    cmd.Parameters.Add("@Added", SqlDbType.DateTime, 255).Value = DateTime.Now;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlCeException e)
                    {
                        //do something? yeah probs do something
                    }
                }
            }
        }

        public QueueMessage<T> GetMessage<T>()
        {
            QueueMessage<T> message = null;
            string sql = @"SELECT TOP(1) QueueID, Added, Retrieved, Message 
                           FROM [Queue] WHERE Retrieved IS NULL ORDER BY Added";
            string sqlUpdate = @"UPDATE [Queue] SET Retrieved = GETDATE() WHERE QueueID = @id";

            using (SqlCeConnection conn = new SqlCeConnection(_connection))
            {
                using (SqlCeCommand cmd = new SqlCeCommand(sql, conn))
                {
                    try
                    {
                        conn.Open();
                        using (var reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            if (reader.Read())
                            {
                                message = new QueueMessage<T>()
                                {
                                    QueueID = reader.GetGuid(0),
                                    Added = reader.GetDateTime(1),
                                    Retrieved = reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2),
                                    Payload = Helpers.Serialization.FromXml<T>(reader.GetString(3))
                                };
                            }
                            else
                            {
                                //do something?
                            }
                        }
                    }
                    catch
                    {                        
                        //do something?
                    }
                }

                if (message != null)
                {
                    using (SqlCeCommand cmd = new SqlCeCommand(sql, conn))
                    {
                        cmd.Connection = conn;
                        cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = message.QueueID;

                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (SqlCeException e)
                        {
                            //do something? yeah probs do something
                        }
                    }
                }
            }

            return message;
        }

        public IEnumerable<QueueMessage<T>> GetAllMessages<T>()
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            string sdfPath = string.Empty;

            //get path
            using (var testConn = new SqlCeConnection(_connection))
            {
                sdfPath = testConn.Database;
            }

            //todo close db connections?
            if (File.Exists(sdfPath))
            {
                File.Delete(sdfPath);
            }
        }
    }
}
