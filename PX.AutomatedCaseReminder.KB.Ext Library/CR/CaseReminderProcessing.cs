using System;
using System.Linq;
using PX.Data;
using System.Collections.Generic;
using System.Collections;
using PX.Objects.SM;
using PX.Objects.CR;
using PX.Objects.EP;
using PX.SM;

namespace PX.AutomatedCaseReminder.KB.Ext.CR
{
    public class CaseReminderProcessing : PXGraph<CaseReminderProcessing>
    {
        public PXCancel<AutoRemindCaseInfo> Cancel;
        public PXProcessing<AutoRemindCaseInfo> Records;

        protected virtual IEnumerable records()
        {
            List<AutoRemindCaseInfo> results = new List<AutoRemindCaseInfo>();

            foreach (PXResult<AutoRemindCaseInfo> result in PXSelect<AutoRemindCaseInfo>.Select(this))
            {
                AutoRemindCaseInfo rec = result.GetItem<AutoRemindCaseInfo>();
                if (((!rec.UsrReminderCount.HasValue || rec.UsrReminderCount == 0) && (rec.LastActivityAge > rec.UsrTimeReactionReminder1)) ||
                    ((rec.UsrReminderCount == 1) && (rec.LastActivityAge > rec.UsrTimeReactionReminder2)) ||
                    ((rec.UsrReminderCount == 2) && (rec.LastActivityAge > rec.UsrTimeReactionReminder3)) ||
                    ((rec.UsrReminderCount == 3) && (rec.LastActivityAge > rec.UsrTimeReactionAutoClose)))
                {
                    results.Add(result);
                }
            }

            return results;
        }

        public CaseReminderProcessing()
        {
            this.Records.SetProcessDelegate(ProcessReminders);
        }

        public static void ProcessReminders(List<AutoRemindCaseInfo> cases)
        {
            bool erroroccurred = false;

            CRCaseMaint graphCase = PXGraph.CreateInstance<CRCaseMaint>();
            
            //Get the Email Templates
            Notification rowNotification1 = PXSelectJoin<Notification,
                InnerJoin<CRSetup, On<Notification.notificationID, Equal<CRSetupExt.usrRem1NotificationMapID>>>>.Select(graphCase);
            if (rowNotification1 == null)
            {
                throw new PXException("Notification Template for Reminder 1 is not specified.");
            }
            Notification rowNotification2 = PXSelectJoin<Notification,
                InnerJoin<CRSetup, On<Notification.notificationID, Equal<CRSetupExt.usrRem2NotificationMapID>>>>.Select(graphCase);
            if (rowNotification2 == null)
            {
                throw new PXException("Notification Template for Reminder 2 is not specified.");
            }
            Notification rowNotification3 = PXSelectJoin<Notification,
                InnerJoin<CRSetup, On<Notification.notificationID, Equal<CRSetupExt.usrRem3NotificationMapID>>>>.Select(graphCase);
            if (rowNotification3 == null)
            {
                throw new PXException("Notification Template for Reminder 3 is not specified.");
            }
            Notification rowNotificationAutoClose = PXSelectJoin<Notification,
                InnerJoin<CRSetup, On<Notification.notificationID, Equal<CRSetupExt.usrAutoCloseNotificationMapID>>>>.Select(graphCase);
            if (rowNotificationAutoClose == null)
            {
                throw new PXException("Notification Template for Auto Close is not specified.");
            }

            List<AutoRemindCaseInfo> casesToProcess = new List<AutoRemindCaseInfo>(cases);

            foreach (var rec in casesToProcess)
            {
                try
                {
                    if ((!rec.UsrReminderCount.HasValue || rec.UsrReminderCount == 0) && (rec.LastActivityAge > rec.UsrTimeReactionReminder1))
                    {
                        //Send First reminder
                        AddEmailActivity(rec, rowNotification1);
                        UpdateReminderCount(graphCase, rec.CaseID, 1);
                        PXProcessing<AutoRemindCaseInfo>.SetInfo(cases.IndexOf(rec),
                                                        String.Format("First Reminder has been sent for Case # {0}", rec.CaseCD));
                    }
                    else if ((rec.UsrReminderCount == 1) && (rec.LastActivityAge > rec.UsrTimeReactionReminder2))
                    {
                        //Send Second reminder
                        AddEmailActivity(rec, rowNotification2);
                        UpdateReminderCount(graphCase, rec.CaseID, 2);
                        PXProcessing<AutoRemindCaseInfo>.SetInfo(cases.IndexOf(rec),
                                                        String.Format("Second Reminder has been sent for Case # {0}", rec.CaseCD));
                    }
                    else if ((rec.UsrReminderCount == 2) && (rec.LastActivityAge > rec.UsrTimeReactionReminder3))
                    {
                        //Send Third reminder
                        AddEmailActivity(rec, rowNotification3);
                        UpdateReminderCount(graphCase, rec.CaseID, 3);
                        PXProcessing<AutoRemindCaseInfo>.SetInfo(cases.IndexOf(rec),
                                                        String.Format("Third Reminder has been sent for Case # {0}", rec.CaseCD));
                    }
                    else if ((rec.UsrReminderCount == 3) && (rec.LastActivityAge > rec.UsrTimeReactionAutoClose))
                    {
                        //Send Case closure notice
                        AddEmailActivity(rec, rowNotificationAutoClose);
                        UpdateReminderCount(graphCase, rec.CaseID, 4, true);
                        PXProcessing<AutoRemindCaseInfo>.SetInfo(cases.IndexOf(rec),
                                                        String.Format("Case Closure Notice has been sent for Case # {0}", rec.CaseCD));
                    }
                }
                catch (Exception e)
                {
                    erroroccurred = true;
                    PXProcessing<AutoRemindCaseInfo>.SetError(cases.IndexOf(rec), e);
                }
            }

            if (erroroccurred)
                throw new PXException("At least one Case hasn't been processed.");
        }

        public override bool IsDirty
        {
            get
            {
                return false;
            }
        }

        private static void AddEmailActivity(AutoRemindCaseInfo CurrentCase, Notification rowNotiication)
        {
            bool sent = false;
            string sError = "Failed to send E-mail.";
            try
            {
                var sender = TemplateNotificationGenerator.Create(CurrentCase, rowNotiication.NotificationID.Value);
                sender.MailAccountId = (rowNotiication.NFrom.HasValue) ? rowNotiication.NFrom.Value :
                                                                         PX.Data.EP.MailAccountManager.DefaultMailAccountID;

                sender.RefNoteID = CurrentCase.NoteID;
                sender.To = CurrentCase.EMail;
                sender.Owner = CurrentCase.OwnerID;
                sent |= sender.Send().Any();
            }
            catch (Exception Err)
            {
                sent = false;
                sError = Err.Message;
            }
            if (!sent)
                throw new PXException(sError);
        }

        private static void UpdateReminderCount(CRCaseMaint graphCase, int? CaseID, int ReminderCount, bool bShouldClose = false)
        {
            try
            {
                CRCase rowCRCase = PXSelect<CRCase, Where<CRCase.caseID, Equal<Required<CRCase.caseID>>>>.Select(graphCase, CaseID);
                if (rowCRCase != null)
                {
                    graphCase.Case.Current = rowCRCase;

                    if (bShouldClose)
                    {
                        rowCRCase.Status = CRCaseStatusesAttribute._CLOSED;
                        rowCRCase.MajorStatus = CRCaseMajorStatusesAttribute._CLOSED;
                        
                        //Set resolution/reason to "Abandoned"
                        rowCRCase.Resolution = "CA";
                    }

                    CRCaseExt caseExt = PXCache<CRCase>.GetExtension<CRCaseExt>(graphCase.Case.Current);
                    caseExt.UsrReminderCount = ReminderCount;

                    graphCase.Case.Update(graphCase.Case.Current);
                    graphCase.Save.Press();
                }
            }
            catch (Exception Err)
            {
                throw Err;
            }
        }
    }
}