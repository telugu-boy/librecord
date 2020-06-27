using lcapis.Entities;
using lcapis.Helpers;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;

namespace lcapis.Services
{
    public interface IServerService
    {
        LCServer Create(LCServer server, long userid);
        LCServer GetByServerID(long id);
        void Delete(long id);
        void Update(LCServer server, long id);
        void Join(string invite, long userid);
    }
    public class LCServerService : IServerService
    {
        private LCProdDbContext _context;

        public LCServerService(LCProdDbContext context)
        {
            _context = context;
        }
        public LCServer Create(LCServer server, long userid)
        {

            if (string.IsNullOrWhiteSpace(server.ServerName))
            {
                server.ServerName = _context.LCUsers.Find(userid).Username + "'s server";
            }
            server.ServerID = Utils.SnowflakeGeneratorSingleton.Instance.CreateId();
            server.UserID = userid;
            /*
            server.Categories = new List<LCCategory> {new LCCategory
            {
                //ServerID = server.ServerID;
                CategoryID = Utils.SnowflakeGeneratorSingleton.Instance.CreateId(),
                Name = "General",
                Channels = new List<LCChannel> { new LCChannel {
                        ChannelID = Utils.SnowflakeGeneratorSingleton.Instance.CreateId(),
                        Name = "General",
                        Description = "General",
                        IsVoice = false,
                        ServerID = server.ServerID
                    }
                }
            } };
            */
            server.Description = "Default description";
            _context.LCServers.Add(server);
            string invitecode = Utils.InviteGenerator.GenerateInvite(6);
            _context.LCInvites.Add(new LCInvite
            {
                InviteCode = invitecode,
                Expiry = DateTime.Today.AddYears(10),
                Server = server
            });
            Join(invitecode, userid);
            _context.SaveChanges();

            return server;
        }

        public void Delete(long id)
        {
            throw new NotImplementedException();
        }

        public LCServer GetByServerID(long id)
        {
            return _context.LCServers.Find(id);
        }

        public void Update(LCServer server, long id)
        {
            throw new NotImplementedException();
        }

        public void Join(string invite, long userid)
        { 
            LCInvite inv = _context.LCInvites.Find(invite);
            if (inv == null)
                throw new AppException("Invalid invite");
            _context.UserServers.Add(new UserServer
            {
                UserID = userid,
                User = _context.LCUsers.Find(userid),
                ServerID = inv.Server.ServerID,
                Server = inv.Server
            });
            _context.SaveChanges();
        }
    }
}
