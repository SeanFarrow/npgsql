using System;
using System.Threading;
using NpgsqlTypes;
using Npgsql.Replication;
using NUnit.Framework;

namespace Npgsql.Tests.Replication
{
    public class NpgsqlConnectionExtensionsTests
    {
        [Test]
        public void AnArgumentNullExceptionIsThrownWhenCreateReplicationConnectionIsCalledWithANullConnection()
        {
            NpgsqlConnection connection = null;
Assert.That(() =>connection.CreateReplicationConnection(), Throws.Exception.TypeOf<ArgumentNullException>());
        }
    }
}
