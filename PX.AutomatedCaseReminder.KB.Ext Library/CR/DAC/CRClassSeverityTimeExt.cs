using System;
using PX.Data;
using PX.Objects.CR;

namespace PX.AutomatedCaseReminder.KB.Ext.CR
{
    public class CRClassSeverityTimeExt : PXCacheExtension<CRClassSeverityTime>
    {
        #region UsrTimeReactionReminder1
        public abstract class usrTimeReactionReminder1 : IBqlField
        {
        }
        [PXDBTimeSpanLong()]
        [PXDefault(0)]
        [PXUIField(DisplayName = "Reaction Time for Reminder 1")]
        public virtual Int32? UsrTimeReactionReminder1 { get; set; }
        #endregion

        #region UsrTimeReactionReminder2
        public abstract class usrTimeReactionReminder2 : IBqlField
        {
        }
        [PXDBTimeSpanLong()]
        [PXDefault(0)]
        [PXUIField(DisplayName = "Reaction Time for Reminder 2")]
        public virtual Int32? UsrTimeReactionReminder2 { get; set; }
        #endregion

        #region UsrTimeReactionReminder3
        public abstract class usrTimeReactionReminder3 : IBqlField
        {
        }
        [PXDBTimeSpanLong()]
        [PXDefault(0)]
        [PXUIField(DisplayName = "Reaction Time for Reminder 3")]
        public virtual Int32? UsrTimeReactionReminder3 { get; set; }
        #endregion

        #region UsrTimeReactionAutoClose
        public abstract class usrTimeReactionAutoClose : IBqlField
        {
        }
        [PXDBTimeSpanLong()]
        [PXDefault(0)]
        [PXUIField(DisplayName = "Reaction Time for Auto Close")]
        public virtual Int32? UsrTimeReactionAutoClose { get; set; }
        #endregion
    }
}
