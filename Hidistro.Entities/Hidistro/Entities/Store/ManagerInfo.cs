namespace Hidistro.Entities.Store
{
    using System;
    using System.Runtime.CompilerServices;

    public class ManagerInfo
    {
        public ManagerInfo()
        {
            this.CreateDate = DateTime.Now;
        }

        public virtual DateTime CreateDate { get; set; }

        public virtual string Email { get; set; }

        public virtual string Password { get; set; }

        public virtual int RoleId { get; set; }

        public virtual int UserId { get;  set; }

        public virtual string UserName { get; set; }
        public virtual string businessNum { get; set; }
        public virtual string realname { get; set; }
        public virtual string telephone { get; set; }
        public virtual string islock { get; set; }
        public virtual string salt { get; set; }


    }
}

