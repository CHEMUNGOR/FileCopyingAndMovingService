namespace FileCopyingService
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
            this.DemoService = new System.ServiceProcess.ServiceProcessInstaller();
            this.siFileCopyingService = new System.ServiceProcess.ServiceInstaller();
            // 
            // DemoService
            // 
            this.DemoService.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.DemoService.Password = null;
            this.DemoService.Username = null;
            // 
            // siFileCopyingService
            // 
            this.siFileCopyingService.Description = "File Copying Service";
            this.siFileCopyingService.DisplayName = "File Copying Service";
            this.siFileCopyingService.ServiceName = "File Copying Service";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.DemoService,
            this.siFileCopyingService});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller DemoService;
        private System.ServiceProcess.ServiceInstaller siFileCopyingService;
    }
}