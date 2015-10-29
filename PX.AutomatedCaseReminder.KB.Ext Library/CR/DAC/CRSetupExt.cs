using System;
using PX.Data;
using PX.Objects.CR;
using PX.Objects.SM;
using PX.SM;

namespace PX.AutomatedCaseReminder.KB.Ext.CR
{
    public class CRSetupExt : PXCacheExtension<CRSetup>
    {
        #region UsrRem1NotificationMapID

        public abstract class usrRem1NotificationMapID : IBqlField { }

        [PXDBInt()]
        [PXUIField(DisplayName = "Notification Template for Reminder 1")]
        [PXSelector(typeof(Search<Notification.notificationID, Where<Notification.status,
                        Equal<PX.SM.NotificationStatusAttribute.published>>>),
                        DescriptionField = typeof(Notification.name))]
        public virtual int? UsrRem1NotificationMapID { get; set; }

        #endregion

        #region UsrRem2NotificationMapID

        public abstract class usrRem2NotificationMapID : IBqlField { }

        [PXDBInt()]
        [PXUIField(DisplayName = "Notification Template for Reminder 2")]
        [PXSelector(typeof(Search<Notification.notificationID, Where<Notification.status,
                        Equal<PX.SM.NotificationStatusAttribute.published>>>),
                        DescriptionField = typeof(Notification.name))]
        public virtual int? UsrRem2NotificationMapID { get; set; }

        #endregion

        #region UsrRem3NotificationMapID

        public abstract class usrRem3NotificationMapID : IBqlField { }

        [PXDBInt()]
        [PXUIField(DisplayName = "Notification Template for Reminder 3")]
        [PXSelector(typeof(Search<Notification.notificationID, Where<Notification.status,
                        Equal<PX.SM.NotificationStatusAttribute.published>>>),
                        DescriptionField = typeof(Notification.name))]
        public virtual int? UsrRem3NotificationMapID { get; set; }

        #endregion

        #region UsrAutoCloseNotificationMapID

        public abstract class usrAutoCloseNotificationMapID : IBqlField { }

        [PXDBInt()]
        [PXUIField(DisplayName = "Notification Template for Auto Close")]
        [PXSelector(typeof(Search<Notification.notificationID, Where<Notification.status,
                        Equal<PX.SM.NotificationStatusAttribute.published>>>),
                        DescriptionField = typeof(Notification.name))]
        public virtual int? UsrAutoCloseNotificationMapID { get; set; }

        #endregion
    }
}