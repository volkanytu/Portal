using GK.Library.Business;
using GK.Library.Utility;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace GK.WindowsServices.ProcessDiscoveryForms
{
    public partial class ProcessDiscoveryForms : ServiceBase
    {
        private System.Timers.Timer _timer;
        private const int SERVICE_INTERVAL = 60 * 1000;
        private SqlDataAccess _sda;
        private IOrganizationService _service;

        public ProcessDiscoveryForms()
        {
            InitializeComponent();
        }

        internal void DebugService(string[] args)
        {

        }

        protected override void OnStart(string[] args)
        {
            StartOperation();
        }

        protected override void OnStop()
        {
            StopOperation();
        }

        private void StartOperation()
        {
            _sda = new SqlDataAccess();
            _sda.openConnection(Globals.ConnectionString);

            _service = MSCRM.GetOrgService(true);

            _timer = new System.Timers.Timer();
            _timer.Interval = SERVICE_INTERVAL;
            _timer.AutoReset = true;
            _timer.Enabled = true;

            _timer.Elapsed += _timer_Elapsed;

            _timer.Start();
        }

        void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            FileLogHelper.LogFunction(this.GetType().Name, "Timer Elapsed", @Globals.FileLogPath);

            _timer.Enabled = false;
            _timer.Stop();

            ProcessForms();

            _timer.Enabled = true;
            _timer.Start();
        }

        private void StopOperation()
        {
            _timer.Enabled = false;
            _timer.Stop();
        }

        private void ProcessForms()
        {
            MsCrmResultObject resultRequestList = DiscoveryFormHelper.GetGiftReuqestListByStatus(DiscoveryFormStatus.Waiting, _sda);

            if (resultRequestList.Success)
            {
                try
                {
                    List<DiscoveryForm> lstForms = resultRequestList.GetReturnObject<List<DiscoveryForm>>();

                    FileLogHelper.LogFunction(this.GetType().Name, "DiscoveryFormCount:" + lstForms.Count.ToString(), @Globals.FileLogPath);

                    foreach (DiscoveryForm form in lstForms)
                    {
                        MsCrmResult result = SendToService(form);

                        if (result.Success)
                        {
                            form.Status = new OptionSetValueWrapper() { AttributeValue = (int)DiscoveryFormStatus.ServiceSent };
                        }
                        else
                        {
                            form.Status = new OptionSetValueWrapper() { AttributeValue = (int)DiscoveryFormStatus.ServiceError };

                            FileLogHelper.LogFunction(this.GetType().Name, "SendToService::" + result.Result, @Globals.FileLogPath);
                        }

                        DiscoveryFormHelper.UpdateDiscoveryForm(form, _service);
                    }
                }
                catch (Exception ex)
                {
                    FileLogHelper.LogFunction(this.GetType().Name, ex.Message, @Globals.FileLogPath);
                }
            }
            else
            {
                FileLogHelper.LogFunction(this.GetType().Name, resultRequestList.Result, @Globals.FileLogPath);
            }
        }

        private MsCrmResult SendToService(DiscoveryForm discoveryForm)
        {
            MsCrmResult returnValue = new MsCrmResult();

            try
            {

                lotusService.UcretsizKesifService lotus = new WindowsServices.ProcessDiscoveryForms.lotusService.UcretsizKesifService();

                //lotusService.RESPONSE result = lotus.CREATERECORD("A3108", 1, discoveryForm.FirstName, discoveryForm.LastName, discoveryForm.Email, discoveryForm.PhoneNumber
                //             , discoveryForm.VisitHour.Value, discoveryForm.CityId.Name, discoveryForm.TownId.Name, discoveryForm.HomeType.Value
                //             , "", ((DateTime)discoveryForm.VisitDate).ToString("dd.MM.yyyy HH:mm"), discoveryForm.InformedBy.Value);

                MsCrmResultObject resultUser = PortalUserHelper.GetPortalUserDetail(new Guid(Globals.DefaultPortalId), discoveryForm.UserId.Id, _sda);

                string userName = "";

                if (resultUser.Success)
                {
                    PortalUser portalUser = (PortalUser)resultUser.ReturnObject;

                    userName = portalUser.ContactInfo.Title;
                }
                else
                {
                    userName = discoveryForm.UserId.Name;
                }

                lotusService.RESPONSE result = lotus.CREATERECORD("A3108", Convert.ToDouble(discoveryForm.FormCode), discoveryForm.FirstName, discoveryForm.LastName, discoveryForm.Email, discoveryForm.PhoneNumber
                             , string.Empty, discoveryForm.CityId.Name, discoveryForm.TownId.Name, string.Empty
                             , "", string.Empty, string.Empty, userName);


                if (result.ERRORCODE == 0)
                {
                    returnValue.Success = true;
                    returnValue.Result = "Servise Gönderildi.";
                }
                else
                {
                    returnValue.Result = result.ERRORCODE + "|" + result.ERRORDESCRIPTION;
                }
            }
            catch (Exception ex)
            {
                returnValue.Result = ex.StackTrace;
            }

            return returnValue;
        }
    }
}
