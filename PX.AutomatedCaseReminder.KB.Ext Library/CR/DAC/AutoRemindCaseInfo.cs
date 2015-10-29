using System;
using PX.Data;
using PX.Objects.CR;
using PX.Objects.CT;
using PX.Data.EP;

namespace PX.AutomatedCaseReminder.KB.Ext.CR
{
    [Serializable]
    [PXPrimaryGraph(typeof(CaseReminderProcessing))]
    [PXProjection(typeof(Select2<CRCase,
            LeftJoin<CRClassSeverityTime, On<CRClassSeverityTime.caseClassID, Equal<CRCase.caseClassID>, And<CRClassSeverityTime.severity, Equal<CRCase.severity>>>,
            LeftJoin<BAccount, On<BAccount.bAccountID, Equal<CRCase.customerID>>,
            LeftJoin<BAccountParent, On<BAccountParent.bAccountID, Equal<BAccount.parentBAccountID>>,
            LeftJoin<Contact, On<Contact.contactID, Equal<CRCase.contactID>>,
            LeftJoin<Contract, On<Contract.contractID, Equal<CRCase.contractID>>,
            LeftJoin<Location, On<Location.locationID, Equal<CRCase.locationID>>>>>>>>,
        Where<True, Equal<True>,
            And<CRCase.majorStatus, Equal<CRCaseMajorStatusesAttribute.pendingCustomer>>>>), Persistent = false)]
    public class AutoRemindCaseInfo : CRCase
    {
        #region Email
        public abstract class eMail : PX.Data.IBqlField
        {
        }
        [PXDBEmail(BqlField = typeof(Contact.eMail))]
        [PXUIField(DisplayName = "Partner Contact Email")]
        public virtual String EMail { get; set; }
        #endregion
        #region FullName
        public abstract class fullName : PX.Data.IBqlField
        {
        }
        [PXDBString(255, IsUnicode = true, BqlField = typeof(Contact.fullName))]
        [PXUIField(DisplayName = "Partner Name")]
        public virtual String FullName { get; set; }
        #endregion
        #region DisplayName
        public abstract class displayName : PX.Data.IBqlField
        {
        }
        [PXDBString(IsUnicode = true, BqlField = typeof(Contact.displayName))]
        [PXUIField(DisplayName = "Partner Contact Name")]
        public virtual String DisplayName { get; set; }
        #endregion
        #region UsrReminderCount
        public abstract class usrReminderCount : PX.Data.IBqlField
        {
        }
        [PXDBInt(BqlField = typeof(CRCaseExt.usrReminderCount))]
        [PXDefault(0)]
        [PXUIField(DisplayName = "Reminder Count")]
        public virtual Int32? UsrReminderCount { get; set; }
        #endregion
        #region UsrTimeReactionReminder1
        public abstract class usrTimeReactionReminder1 : PX.Data.IBqlField
        {
        }
        [PXDBTimeSpanLong(BqlField = typeof(CRClassSeverityTimeExt.usrTimeReactionReminder1))]
        [PXDefault(0)]
        [PXUIField(DisplayName = "Reaction Time for Reminder 1")]
        public virtual Int32? UsrTimeReactionReminder1 { get; set; }
        #endregion
        #region UsrTimeReactionReminder2
        public abstract class usrTimeReactionReminder2 : PX.Data.IBqlField
        {
        }
        [PXDBTimeSpanLong(BqlField = typeof(CRClassSeverityTimeExt.usrTimeReactionReminder2))]
        [PXDefault(0)]
        [PXUIField(DisplayName = "Reaction Time for Reminder 2")]
        public virtual Int32? UsrTimeReactionReminder2 { get; set; }
        #endregion
        #region UsrTimeReactionReminder3
        public abstract class usrTimeReactionReminder3 : PX.Data.IBqlField
        {
        }
        [PXDBTimeSpanLong(BqlField = typeof(CRClassSeverityTimeExt.usrTimeReactionReminder3))]
        [PXDefault(0)]
        [PXUIField(DisplayName = "Reaction Time for Reminder 3")]
        public virtual Int32? UsrTimeReactionReminder3 { get; set; }
        #endregion
        #region UsrTimeReactionAutoClose
        public abstract class usrTimeReactionAutoClose : PX.Data.IBqlField
        {
        }
        [PXDBTimeSpanLong(BqlField = typeof(CRClassSeverityTimeExt.usrTimeReactionAutoClose))]
        [PXDefault(0)]
        [PXUIField(DisplayName = "Reaction Time for Auto Close")]
        public virtual Int32? UsrTimeReactionAutoClose { get; set; }
        #endregion

        #region TimeReactionReminder1INDays
        public abstract class timeReactionReminder1INDays : PX.Data.IBqlField{ }
        [PXInt]
        [PXUIField(DisplayName = "Reaction Time for Reminder 1 in Days")]
        [PXDBCalced(typeof(Div<IsNull<CRClassSeverityTimeExt.usrTimeReactionReminder1, int_0>, Mult<int_24, int_60>>), typeof(int))]
        public virtual int? TimeReactionReminder1INDays { get; set; }
        #endregion
        #region TimeReactionReminder2INDays
        public abstract class timeReactionReminder2INDays : PX.Data.IBqlField
        { }
        [PXInt]
        [PXUIField(DisplayName = "Reaction Time for Reminder 2 in Days")]
        [PXDBCalced(typeof(Div<IsNull<CRClassSeverityTimeExt.usrTimeReactionReminder2, int_0>, Mult<int_24, int_60>>), typeof(int))]
        public virtual int? TimeReactionReminder2INDays { get; set; }
        #endregion
        #region TimeReactionReminder3INDays
        public abstract class timeReactionReminder3INDays : PX.Data.IBqlField
        { }
        [PXInt]
        [PXUIField(DisplayName = "Reaction Time for Reminder 3 in Days")]
        [PXDBCalced(typeof(Div<IsNull<CRClassSeverityTimeExt.usrTimeReactionReminder3, int_0>, Mult<int_24, int_60>>), typeof(int))]
        public virtual int? TimeReactionReminder3INDays { get; set; }
        #endregion
        #region TimeReactionAutoCloseINDays
        public abstract class timeReactionAutoCloseINDays : PX.Data.IBqlField
        { }
        [PXInt]
        [PXUIField(DisplayName = "Reaction Time for auto close in Days")]
        [PXDBCalced(typeof(Div<IsNull<CRClassSeverityTimeExt.usrTimeReactionAutoClose, int_0>, Mult<int_24, int_60>>), typeof(int))]
        public virtual int? TimeReactionAutoCloseINDays { get; set; }
        #endregion
    }

    public class int_0 : Constant<int>
    {
        public int_0()
            : base(0)
        {
        }
    }

    public class int_24 : Constant<int>
    {
        public int_24()
            : base(24)
        {
        }
    }

    public class int_60 : Constant<int>
    {
        public int_60()
            : base(60)
        {
        }
    }
}