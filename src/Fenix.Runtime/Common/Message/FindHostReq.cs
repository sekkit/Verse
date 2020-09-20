﻿//AUTOGEN, do not modify it!

using Fenix.Common.Utils;
using Fenix.Common;
using Fenix.Common.Attributes;
using Fenix.Common.Rpc;
using MessagePack; 
using System.ComponentModel;
using System; 

namespace Fenix.Common.Message
{
    [MessageType(OpCode.FIND_HOST_REQ)]
    [MessagePackObject]
    public class FindHostReq : IMessageWithCallback
    {
        [Key(0)]
        public global::System.UInt64 hostId { get; set; }

        [Key(1)]

        public Callback callback
        {
            get => _callback as Callback;
            set => _callback = value;
        } 

        [MessagePackObject]
        public class Callback : IMessage
        {
            [Key(0)]
            public global::System.Boolean arg0 { get; set; }

            [Key(1)]
            public global::Fenix.HostInfo arg1 { get; set; }

            public override byte[] Pack()
            {
                return MessagePackSerializer.Serialize<Callback>(this);
            }

            public new static Callback Deserialize(byte[] data)
            {
                return MessagePackSerializer.Deserialize<Callback>(data);
            }

            public override void UnPack(byte[] data)
            {
                var obj = Deserialize(data);
                Copier<Callback>.CopyTo(obj, this);
            }
        }

        public override byte[] Pack()
        {
            return MessagePackSerializer.Serialize<FindHostReq>(this);
        }

        public new static FindHostReq Deserialize(byte[] data)
        {
            return MessagePackSerializer.Deserialize<FindHostReq>(data);
        }

        public override void UnPack(byte[] data)
        {
            var obj = Deserialize(data);
            Copier<FindHostReq>.CopyTo(obj, this);
        }
    }
}

