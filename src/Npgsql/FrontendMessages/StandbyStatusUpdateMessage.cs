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

namespace Npgsql.FrontendMessages
{
    // See https://www.postgresql.org/docs/current/static/protocol-replication.html for details.
    class StandbyStatusUpdateMessage : SimpleFrontendMessage
    {
        internal override int Length =>
            1    // Message type
            + 8  // LastWrittenLsn
            + 8  // LastFlushedLsn
            + 8  // LastAppliedLsn
            + 8  // SystemClock
            + 1; // ReplyImmediately

        /// <summary>
        /// The location of the last WAL byte + 1 received and written to disk in the standby.
        /// </summary>
        public NpgsqlLsn LastWrittenLsn { get; set; }

        /// <summary>
        /// The location of the last WAL byte + 1 flushed to disk in the standby.
        /// </summary>
        public NpgsqlLsn LastFlushedLsn { get; set; }

        /// <summary>
        /// The location of the last WAL byte + 1 applied in the standby.
        /// </summary>
        public NpgsqlLsn LastAppliedLsn { get; set; }

        /// <summary>
        /// The client's system clock at the time of transmission, as microseconds since midnight on 2000-01-01.
        /// </summary>
        public long SystemClock { get; set; }

        /// <summary>
        /// If true, the client requests the server to reply to this message immediately. This can be used to ping the server, to test if the connection is still healthy.
        /// </summary>
        public bool ReplyImmediately { get; set; }

        internal override void WriteFully(NpgsqlWriteBuffer buf)
        {
            buf.WriteByte((byte)'r');

            buf.WriteUInt32(LastWrittenLsn.Upper);
            buf.WriteUInt32(LastWrittenLsn.Lower);

            buf.WriteUInt32(LastFlushedLsn.Upper);
            buf.WriteUInt32(LastFlushedLsn.Lower);

            buf.WriteUInt32(LastAppliedLsn.Upper);
            buf.WriteUInt32(LastAppliedLsn.Lower);

            buf.WriteInt64(SystemClock);

            buf.WriteByte((byte)(ReplyImmediately ? 1 : 0));
        }
    }
}
