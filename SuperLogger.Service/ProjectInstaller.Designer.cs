namespace SuperLogger.Service
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
            this.SuperLoggerServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.SuperLoggerServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // SuperLoggerServiceProcessInstaller
            // 
            this.SuperLoggerServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.SuperLoggerServiceProcessInstaller.Password = null;
            this.SuperLoggerServiceProcessInstaller.Username = null;
            // 
            // SuperLoggerServiceInstaller
            // 
            this.SuperLoggerServiceInstaller.Description = "SuperLogger service";
            this.SuperLoggerServiceInstaller.DisplayName = "SuperLogger";
            this.SuperLoggerServiceInstaller.ServiceName = "SuperLogger";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.SuperLoggerServiceProcessInstaller,
            this.SuperLoggerServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller SuperLoggerServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller SuperLoggerServiceInstaller;
    }
}