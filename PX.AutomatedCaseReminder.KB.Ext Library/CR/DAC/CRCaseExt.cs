using System;
using PX.Data;
using PX.Objects.CR;

namespace PX.AutomatedCaseReminder.KB.Ext.CR
{
    public class CRCaseExt : PXCacheExtension<CRCase>
    {
        #region UsrReminderCount
        public abstract class usrReminderCount : IBqlField
        {
        }
        [PXDBInt()]
        [PXUIField(DisplayName = "Reminder Count")]
        public virtual Int32? UsrReminderCount { get; set; }
        #endregion
    }
}