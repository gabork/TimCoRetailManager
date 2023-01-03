using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TRMDataManager.Library.SqlDataAccess;

namespace TRMDataManager.Library
{
    internal class SqlDataAccess
    {
        public class SqlDataAccess : IDisposable, ISqlDataAccess
        {
            private IDbConnection connection;
            private IDbTransaction transaction;
            private bool isClosed = false;
            private readonly IConfiguration config;
            private readonly ILogger<SqlDataAccess> logger;

            public SqlDataAccess(
                IConfiguration config
                , ILogger<SqlDataAccess> logger)
            {
                this.config = config;
                this.logger = logger;
            }

            public string GetConnectionString(string name)
            {
                return config.GetConnectionString(name);
            }

            public List<T> LoadData<T, U>(
                string storedProcedure
                , U parameters
                , string connectionStringName)
            {
                var connectionString = GetConnectionString(connectionStringName);

                using IDbConnection connection = new SqlConnection(connectionString);
                var rows = connection.Query<T>(
                    storedProcedure
                    , parameters
                    , commandType: CommandType.StoredProcedure).ToList();

                return rows;
            }

            public void SaveData<T>(
                string storedProcedure
                , T parameters
                , string connectionStringName)
            {
                var connectionString = GetConnectionString(connectionStringName);

                using IDbConnection connection = new SqlConnection(connectionString);
                connection.Execute(
                    storedProcedure
                    , parameters
                    , commandType: CommandType.StoredProcedure);
            }

            public void StartTransaction(string connectionStringName)
            {
                var connectionString = GetConnectionString(connectionStringName);
                connection = new SqlConnection(connectionString);
                connection.Open();
                transaction = connection.BeginTransaction();
                isClosed = false;
            }

            public void CommitTransaction()
            {
                transaction?.Commit();
                connection?.Close();
                isClosed = true;
            }

            public void RollbackTransaction()
            {
                transaction?.Rollback();
                connection?.Close();
                isClosed = true;
            }

            public void Dispose()
            {
                if (isClosed == false)
                {
                    try
                    {
                        CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Commit transaction failed in the dispose method.");
                    }
                }

                transaction = null;
                connection = null;
                GC.SuppressFinalize(this);
            }

            public void SaveDataInTransaction<T>(
                string storedProcedure
                , T parameters)
            {
                connection.Execute(
                    storedProcedure
                    , parameters
                    , commandType: CommandType.StoredProcedure
                    , transaction: transaction);
            }

            public List<T> LoadDataInTransaction<T, U>(
                string storedProcedure
                , U parameters)
            {
                var rows = connection.Query<T>(
                        storedProcedure
                        , parameters
                        , commandType: CommandType.StoredProcedure
                        , transaction: transaction).ToList();

                return rows;
            }
        }
    }
}
