namespace Hidistro.ControlPanel.Members
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Store;
    using Hidistro.SqlDal.Members;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.InteropServices;

    public static class MemberHelper
    {
        public static IList<string> BatchCreateMembers(IList<string> distributornames, int referruserId,string wid, string createtype = "1")
        {
            string referralPath = string.Empty;
            IList<string> list = new List<string>();
            DistributorGrade threeDistributor = DistributorGrade.ThreeDistributor;
            if (referruserId > 0)
            {
                referralPath = new DistributorsDao().GetDistributorInfo(referruserId).ReferralPath;
                if (string.IsNullOrEmpty(referralPath))
                {
                    referralPath = referruserId.ToString();
                    threeDistributor = DistributorGrade.TowDistributor;
                }
                else if (referralPath.Contains("|"))
                {
                    referralPath = referralPath.Split(new char[] { '|' })[1] + "|" + referruserId.ToString();
                }
                else
                {
                    referralPath = referralPath + "|" + referruserId.ToString();
                }
            }
            foreach (string str2 in distributornames)
            {
                MemberInfo member = new MemberInfo();
                string generateId = Globals.GetGenerateId();
                member.GradeId = new MemberGradeDao().GetDefaultMemberGrade(wid);
                member.UserName = str2;
                member.CreateDate = DateTime.Now;
                member.UserBindName = str2;
                member.SessionId = generateId;
                member.SessionEndTime = DateTime.Now.AddDays(10);
                member.Password = HiCryptographer.Md5Encrypt("888888");
                if ((new MemberDao().GetusernameMember(str2,wid) == null) && new MemberDao().CreateMember(member))
                {
                    DistributorsInfo distributor = new DistributorsInfo {
                        UserId = new MemberDao().GetusernameMember(str2,wid).UserId,
                        RequestAccount = "",
                        StoreName = str2,
                        StoreDescription = "",
                        BackImage = "",
                        Logo = "",
                        DistributorGradeId = threeDistributor
                    };
                    distributor.UserId.ToString();
                    distributor.ReferralPath = referralPath;
                    distributor.ParentUserId = new int?(Convert.ToInt32(referruserId));
                    DistributorGradeInfo isDefaultDistributorGradeInfo = new DistributorsDao().GetIsDefaultDistributorGradeInfo();
                    distributor.DistriGradeId = isDefaultDistributorGradeInfo.GradeId;
                    if (new DistributorsDao().CreateDistributor(distributor) && (createtype == "1"))
                    {
                        list.Add(str2);
                    }
                }
                else if (createtype == "2")
                {
                    list.Add(str2);
                }
            }
            return list;
        }

        public static bool CreateMemberGrade(MemberGradeInfo memberGrade)
        {
            if (memberGrade == null)
            {
                return false;
            }
            Globals.EntityCoding(memberGrade, true);
            if (!IsCanSetThisGrade(memberGrade))
            {
                throw new Exception("交易次数的上下级别，与交易额的上下级别不是同一个！");
            }
            bool flag = new MemberGradeDao().CreateMemberGrade(memberGrade);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.AddMemberGrade, string.Format(CultureInfo.InvariantCulture, "添加了名为 “{0}” 的会员等级", new object[] { memberGrade.Name }));
            }
            return flag;
        }

        public static bool Delete(int userId)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteMember);
            bool flag = new MemberDao().Delete(userId);
            if (flag)
            {
                HiCache.Remove(string.Format("DataCache-Member-{0}", userId));
                EventLogs.WriteOperationLog(Privilege.DeleteMember, string.Format(CultureInfo.InvariantCulture, "删除了编号为 “{0}” 的会员", new object[] { userId }));
            }
            return flag;
        }

        public static bool Delete2(int userId)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteMember);
            bool flag = new MemberDao().Delete2(userId);
            if (flag)
            {
                HiCache.Remove(string.Format("DataCache-Member-{0}", userId));
                EventLogs.WriteOperationLog(Privilege.DeleteMember, string.Format(CultureInfo.InvariantCulture, "逻辑删除了编号为 “{0}” 的会员", new object[] { userId }));
            }
            return flag;
        }

        public static bool DeleteMemberGrade(int gradeId)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteMemberGrade);
            bool flag = new MemberGradeDao().DeleteMemberGrade(gradeId);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.DeleteMemberGrade, string.Format(CultureInfo.InvariantCulture, "删除了编号为 “{0}” 的会员等级", new object[] { gradeId }));
            }
            return flag;
        }

        public static bool Deletes(string userId)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteMember);
            return new MemberDao().Deletes(userId);
        }

        public static string GetAllDistributorsName(string keysearch)
        {
            string str = "";
            foreach (DataRow row in new DistributorsDao().GetAllDistributorsName(keysearch).Rows)
            {
                string str2 = str;
                str = str2 + "{\"title\":\"" + Globals.HtmlEncode(row[0].ToString()) + "\",\"result\":\"" + row[1].ToString() + "\"},";
            }
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Substring(0, str.Length - 1);
            }
            return str;
        }

        public static DbQueryResult GetIntegralDetail(IntegralDetailQuery query)
        {
            return new IntegralDetailDao().GetIntegralDetail(query);
        }

        public static MemberInfo GetMember(int userId)
        {
            return new MemberDao().GetMember(userId);
        }

        public static Dictionary<int, MemberClientSet> GetMemberClientSet()
        {
            return new MemberDao().GetMemberClientSet();
        }

        public static MemberGradeInfo GetMemberGrade(int gradeId)
        {
            return new MemberGradeDao().GetMemberGrade(gradeId);
        }

        public static IList<MemberGradeInfo> GetMemberGrades(string wid)
        {
            return new MemberGradeDao().GetMemberGrades(wid);
        }

        public static DbQueryResult GetMembers(MemberQuery query)
        {
            return new MemberDao().GetMembers(query);
        }

        public static IList<MemberInfo> GetMembersByRank(int? gradeId)
        {
            return new MemberDao().GetMembersByRank(gradeId);
        }

        public static DataTable GetMembersNopage(MemberQuery query, IList<string> fields)
        {
            return new MemberDao().GetMembersNopage(query, fields);
        }

        public static IList<MemberInfo> GetMemdersByCardNumbers(string cards)
        {
            return new MemberDao().GetMemdersByCardNumbers(cards);
        }

        public static IList<MemberInfo> GetMemdersByOpenIds(string openids)
        {
            return new MemberDao().GetMemdersByOpenIds(openids);
        }

        public static bool HasSameMemberGrade(MemberGradeInfo memberGrade)
        {
            return new MemberGradeDao().HasSameMemberGrade(memberGrade);
        }

        public static bool HasSamePointMemberGrade(MemberGradeInfo memberGrade)
        {
            return new MemberGradeDao().HasSamePointMemberGrade(memberGrade);
        }

        public static bool InsertClientSet(Dictionary<int, MemberClientSet> clientset)
        {
            return new MemberDao().InsertClientSet(clientset);
        }

        private static bool IsCanSetThisGrade(MemberGradeInfo memberGrade)
        {
            List<MemberGradeInfo> list = GetMemberGrades(memberGrade.wid).ToList<MemberGradeInfo>();
            int num = 0;
            int num2 = 0;
            foreach (MemberGradeInfo info in list)
            {
                if (info.GradeId != memberGrade.GradeId)
                {
                    if ((info.TranVol.HasValue && memberGrade.TranVol.HasValue) && (info.TranVol.Value > memberGrade.TranVol.Value))
                    {
                        num++;
                    }
                    if ((info.TranTimes.HasValue && memberGrade.TranTimes.HasValue) && (info.TranTimes.Value > memberGrade.TranTimes.Value))
                    {
                        num2++;
                    }
                }
            }
            return (num == num2);
        }

        public static bool IsExist(string name,string wid)
        {
            return new MemberGradeDao().IsExist(name,wid);
        }

        public static int IsExiteDistributorNames(string distributorname)
        {
            return new DistributorsDao().IsExiteDistributorsByStoreName(distributorname);
        }

        public static int SelectUserCountGrades(int gid,string wid)
        {
            return new MemberGradeDao().SelectUserCountGrades(gid,wid);
        }

        public static int SelectUserGroupSet()
        {
            return new MemberGradeDao().SelectUserGroupSet();
        }

        public static void SetDefalutMemberGrade(int gradeId)
        {
            new MemberGradeDao().SetDefalutMemberGrade(gradeId);
        }

        public static int SetRegion(string userID, int regionId)
        {
            return new MemberDao().SetRegion(userID, regionId);
        }

        public static int SetRegions(string userIDs, int regionId)
        {
            return new MemberDao().SetRegions(userIDs, regionId);
        }

        public static int SetUserGroup(int day)
        {
            return new MemberGradeDao().SetUserGroup(day);
        }

        public static int SetUsersGradeId(string userId, int gradeId)
        {
            return new MemberDao().SetUsersGradeId(userId, gradeId);
        }

        public static bool Update(MemberInfo member)
        {
            bool flag = new MemberDao().Update(member);
            if (flag)
            {
                HiCache.Remove(string.Format("DataCache-Member-{0}", member.UserId));
                EventLogs.WriteOperationLog(Privilege.EditMember, string.Format(CultureInfo.InvariantCulture, "修改了编号为 “{0}” 的会员", new object[] { member.UserId }));
            }
            return flag;
        }

        public static bool UpdateMemberGrade(MemberGradeInfo memberGrade)
        {
            if (memberGrade == null)
            {
                return false;
            }
            Globals.EntityCoding(memberGrade, true);
            if (!IsCanSetThisGrade(memberGrade))
            {
                throw new Exception("交易次数的上下级别，与交易额的上下级别不是同一个！");
            }
            bool flag = new MemberGradeDao().UpdateMemberGrade(memberGrade);
            if (flag)
            {
                EventLogs.WriteOperationLog(Privilege.EditMemberGrade, string.Format(CultureInfo.InvariantCulture, "修改了编号为 “{0}” 的会员等级", new object[] { memberGrade.GradeId }));
            }
            return flag;
        }
    }
}

