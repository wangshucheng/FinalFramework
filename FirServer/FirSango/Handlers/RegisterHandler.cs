﻿using log4net;
using FirServer.Define;
using LiteNetLib;
using LiteNetLib.Utils;
using System.Collections.Generic;
using FirServer.Handler;
using FirServer;
using GameLibs.FirSango.Defines;
using GameLibs.FirSango.Model;
using System;

namespace GameLibs.FirSango.Handlers
{
    public class RegisterHandler : BaseHandler
    {
        private static readonly ILog logger = LogManager.GetLogger(AppServer.repository.Name, typeof(RegisterHandler));

        public override void OnMessage(NetPeer peer, byte[] bytes)
        {
            var username = string.Empty;
            var password = string.Empty;

            long uid = 0L;
            var dw = new NetDataWriter();
            dw.Put(GameProtocal.Register);

            var user = new UserInfo()
            {
                username = username,
                password = password,
                money = 10000L,
                lasttime = DateTime.Now.ToShortDateString()
            };
            var userModel = modelMgr.GetModel(ModelNames.User) as UserModel;
            if (userModel != null)
            {
                uid = userModel.AddUser(user);
            }
            var result = uid == 0 ? ResultCode.Failed : ResultCode.Success;
            dw.Put((ushort)result);
            if (uid > 0L) 
            {
                dw.Put(uid);
            }
            peer.Send(dw, DeliveryMethod.ReliableOrdered);
            logger.Info("OnMessage: " + uid);
        }
    }
}
