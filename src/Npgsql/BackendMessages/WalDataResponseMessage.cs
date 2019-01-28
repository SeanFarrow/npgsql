using System;
using NpgsqlTypes;

namespace Npgsql.BackendMessages
{
    // Note that this message doesn't actually contain the data, but only the length.
    // See https://www.postgresql.org/docs/current/static/protocol-replication.html for details.
    class WalDataResponseMessage : IBackendMessage
    {
        public BackendMessageCode Code => BackendMessageCode.WalData;

        /// <summary>
        /// The starting point of the WAL data in this message.
        /// </summary>
        internal NpgsqlLsn StartLsn { get; private set; }

        /// <summary>
        /// The current end of WAL on the server.
        /// </summary>
        internal NpgsqlLsn EndLsn { get; private set; }

        /// <summary>
        /// The server's system clock at the time of transmission, as microseconds since midnight on 2000-01-01.
        /// </summary>
        internal long SystemClock { get; private set; }

        internal int MessageLength =>
            1    // Code
            + 8  // StartLsn
            + 8  // EndLsn
            + 8; // SystemClock

        internal void Load(NpgsqlReadBuffer buffer)
        {
            var upper = buffer.ReadUInt32();
            var lower = buffer.ReadUInt32();
            StartLsn = new NpgsqlLsn(upper, lower);

            upper = buffer.ReadUInt32();
            lower = buffer.ReadUInt32();
            EndLsn = new NpgsqlLsn(upper, lower);

            SystemClock = buffer.ReadInt64();
        }
    }
}
