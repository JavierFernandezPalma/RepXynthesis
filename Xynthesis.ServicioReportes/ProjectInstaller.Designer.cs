namespace Xynthesis.ServicioReportes
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
            this.Servicio_ReportesXynthesis = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaller1
            // 
            this.serviceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.serviceProcessInstaller1.Password = null;
            this.serviceProcessInstaller1.Username = null;
            this.serviceProcessInstaller1.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceProcessInstaller1_AfterInstall);
            // 
            // Servicio_ReportesXynthesis
            // 
            this.Servicio_ReportesXynthesis.Description = "Servicio para realizar reportes programados por medio del aplicativo Xynthesis.";
            this.Servicio_ReportesXynthesis.DisplayName = "Servicio_ReportesXynthesis";
            this.Servicio_ReportesXynthesis.ServiceName = "ServiReport";
            this.Servicio_ReportesXynthesis.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.Servicio_ReportesXynthesis.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.Servicio_ReportesXynthesis_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller1,
            this.Servicio_ReportesXynthesis});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller1;
        private System.ServiceProcess.ServiceInstaller Servicio_ReportesXynthesis;
    }
}