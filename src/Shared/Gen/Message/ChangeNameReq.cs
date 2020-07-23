﻿//AUTOGEN, do not modify it!

using Fenix.Common;
using Fenix.Common.Attributes;
using Fenix.Common.Rpc;
using MessagePack; 
using Shared;
using Shared.Protocol;
using Shared.DataModel;
using System; 

namespace Shared.Message
{
    [MessageType(ProtocolCode.CHANGE_NAME_REQ)]
    [MessagePackObject]
    public class ChangeNameReq : IMessageWithCallback
    {
        [Key(0)]
        public String name;


        [Key(199)]
        public Callback callback
        {
            get => _callback as Callback;
            set => _callback = value;
        } 

        [MessagePackObject]
        public class Callback
        {
            [Key(0)]
            public ErrCode code;

        }

    }
}

