#region License
// The PostgreSQL License
//
// Copyright (C) 2018 The Npgsql Development Team
//
// Permission to use, copy, modify, and distribute this software and its
// documentation for any purpose, without fee, and without a written
// agreement is hereby granted, provided that the above copyright notice
// and this paragraph and the following two paragraphs appear in all copies.
//
// IN NO EVENT SHALL THE NPGSQL DEVELOPMENT TEAM BE LIABLE TO ANY PARTY
// FOR DIRECT, INDIRECT, SPECIAL, INCIDENTAL, OR CONSEQUENTIAL DAMAGES,
// INCLUDING LOST PROFITS, ARISING OUT OF THE USE OF THIS SOFTWARE AND ITS
// DOCUMENTATION, EVEN IF THE NPGSQL DEVELOPMENT TEAM HAS BEEN ADVISED OF
// THE POSSIBILITY OF SUCH DAMAGE.
//
// THE NPGSQL DEVELOPMENT TEAM SPECIFICALLY DISCLAIMS ANY WARRANTIES,
// INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY
// AND FITNESS FOR A PARTICULAR PURPOSE. THE SOFTWARE PROVIDED HEREUNDER IS
// ON AN "AS IS" BASIS, AND THE NPGSQL DEVELOPMENT TEAM HAS NO OBLIGATIONS
// TO PROVIDE MAINTENANCE, SUPPORT, UPDATES, ENHANCEMENTS, OR MODIFICATIONS.
#endregion

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
