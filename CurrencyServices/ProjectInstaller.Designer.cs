namespace CurrencyServices
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller2 = new System.ServiceProcess.ServiceInstaller();
            this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaller1
            // 
            this.serviceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.NetworkService;
            this.serviceProcessInstaller1.Password = null;
            this.serviceProcessInstaller1.Username = null;
            // 
            // serviceInstaller2
            // 
            this.serviceInstaller2.Description = "Screen Grab from altcointrader";
            this.serviceInstaller2.DisplayName = "CurrencyExchange - AltCoinTrader";
            this.serviceInstaller2.ServiceName = "GetAltcointraderData";
            this.serviceInstaller2.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // serviceInstaller1
            // 
            this.serviceInstaller1.Description = "Get Sync Data From Kraken";
            this.serviceInstaller1.DisplayName = "CurrencyExchange - Kraken";
            this.serviceInstaller1.ServiceName = "GetKrakenData";
            this.serviceInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller1,
            this.serviceInstaller2,
            this.serviceInstaller1});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller1;
        private System.ServiceProcess.ServiceInstaller serviceInstaller2;
        private System.ServiceProcess.ServiceInstaller serviceInstaller1;
    }
}