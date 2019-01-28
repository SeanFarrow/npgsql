using System;

namespace Npgsql.Replication
{
    /// <summary>
    /// Extensions for the <see cref="NpgsqlConnection"/> type pertaining to PostGres replication.
    /// </summary>
    public static class NpgsqlConnectionExtensions
    {
        /// <summary>
        /// Test
        /// </summary>
        /// <param name="connection"></param>
        public static void CreateReplicationConnection(this NpgsqlConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            throw new NotImplementedException();
        }
    }
}
