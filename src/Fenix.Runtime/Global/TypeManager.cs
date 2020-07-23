﻿using Fenix.Common;
using Fenix.Common.Attributes;
using Fenix.Common.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Fenix
{
    public class TypeManager
    { 
        protected TypeManager()
        {
        }

        public static TypeManager Instance = new TypeManager();

        protected ConcurrentDictionary<string, Type> mTypeDic = new ConcurrentDictionary<string, Type>();

        protected ConcurrentDictionary<uint, Type> mMessageTypeDic = new ConcurrentDictionary<uint, Type>();
         
        protected ConcurrentDictionary<Type, Type> mRef2ActorTypeDic = new ConcurrentDictionary<Type, Type>();

        protected ConcurrentDictionary<Type, Type> mActor2RefTypeDic = new ConcurrentDictionary<Type, Type>();
         
        public void RegisterType(string name, Type type)
        {
            Log.Info(string.Format("RegisterType: {0} {1}", name, type.FullName));
            this.mTypeDic[name] = type;
        }

        public void ScanAssemblies(Assembly[] asmList)
        {
            //扫描一下
            foreach(var asm in asmList)
            foreach (var t in asm.GetTypes())
            {
                if(RpcUtil.IsHeritedType(t, "Actor"))
                    RegisterType(t.Name, t);
            } 

            foreach(var asm in asmList)
                foreach(var t in asm.GetTypes())
                {
                    var refTypeAttrs = t.GetCustomAttributes(typeof(RefTypeAttribute));
                    if (refTypeAttrs.Count() > 0)
                    {
                        var rta = (RefTypeAttribute)refTypeAttrs.First();
                        var rtaType = Global.TypeManager.Get(rta.TypeName);
                        Global.TypeManager.RegisterRefType(t, rtaType);
                    }

                    var msgTypeAttrs = t.GetCustomAttributes(typeof(MessageTypeAttribute));
                    if (msgTypeAttrs.Count() > 0)
                    {
                        var mta = (MessageTypeAttribute)msgTypeAttrs.First();
                        Global.TypeManager.RegisterMessageType(mta.ProtoCode, t);
                    }
                }
        }  

        public void RegisterRefType(Type refType, Type targetType)
        {
            this.mRef2ActorTypeDic[refType] = targetType;
            this.mActor2RefTypeDic[targetType] = refType;
        }

        public void RegisterMessageType(uint protoCode, Type type)
        {
            mMessageTypeDic[protoCode] = type;
        }

        public void RegisterActorType(Actor actor)
        {
            uint actorId = actor.Id;
            string actorName = actor.UniqueName;
            Type type = actor.GetType(); 
            if(!mTypeDic.ContainsKey(type.Name))
                mTypeDic[type.Name] = type; 
        }

        public Type Get(string typeName)
        {
            if (typeName == null)
                return null;
            mTypeDic.TryGetValue(typeName, out var result);
            return result;
        }

        public Type GetActorType(uint actorId)
        {
            var tname = Global.IdManager.GetActorTypename(actorId);

            return this.Get(tname);
        }

        public Type GetMessageType(uint protocolId)
        {
            return mMessageTypeDic[protocolId];
        }

        public Type GetRefType(Type type)
        { 
            if (this.mActor2RefTypeDic.TryGetValue(type, out Type t))
                return t;
            return typeof(ActorRef);
        }
    }
}
