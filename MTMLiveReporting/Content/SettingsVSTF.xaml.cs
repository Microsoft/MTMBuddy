﻿//------------------------------------------------------------------------------------------------------- 
// Copyright (C) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information. 
//------------------------------------------------------------------------------------------------------- 
using System;
using System.Configuration;

using System.Windows;

using MTMIntegration;

using MessageBox = System.Windows.MessageBox;

using UserControl = System.Windows.Controls.UserControl;

namespace MTMLiveReporting
{
    /// <summary>
    ///     Interaction logic for SettingsVSTF.xaml
    /// </summary>
   
    public partial class SettingsVstf 
    {
        public SettingsVstf()
        {
            InitializeComponent();
            TxtTfsUrl.Text = ConfigurationManager.AppSettings["TFSUrl"];
            TxtTeamProject.Text = ConfigurationManager.AppSettings["TeamProject"];
        
            TxtTestPlanId.Text = ConfigurationManager.AppSettings["TestPlanID"];
     
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
           
            
            

            ConfigurationManager.AppSettings["TFSUrl"] = TxtTfsUrl.Text;
            ConfigurationManager.AppSettings["TeamProject"] = TxtTeamProject.Text;
        
            ConfigurationManager.AppSettings["TestPlanId"] = TxtTestPlanId.Text;
       
            try
            {
                MtmInteraction.InitializeVstfConnection(new Uri(ConfigurationManager.AppSettings["TFSUrl"]),
                    ConfigurationManager.AppSettings["TeamProject"],
                    int.Parse(ConfigurationManager.AppSettings["TestPlanID"]));
                DataGetter.SaveConfig("TFSUrl", TxtTfsUrl.Text);
                DataGetter.SaveConfig("TeamProject", TxtTeamProject.Text);
                DataGetter.SaveConfig("TestPlanID", TxtTestPlanId.Text);

                ConfigurationManager.AppSettings["FirstRun"] = false.ToString();
                DataGetter.SaveConfig("FirstRun", false.ToString());



                DataGetter.Diagnostic.AppendLine("Config changed:");
                DataGetter.Diagnostic.AppendLine("TFS URL: " + TxtTfsUrl.Text);
                DataGetter.Diagnostic.AppendLine("Team Project: " + TxtTeamProject.Text);
               

                DataGetter.Diagnostic.AppendLine("---------------------------------------------------");
                MessageBox.Show("Voila! All changes have been saved.");
            }


            catch (Exception exp)
            {
                MessageBox.Show(
                    "Unable to connect to VSTF.Please check your connectivity and configuration" + Environment.NewLine +
                    "Exception Details: " + exp.Message, "OOPS!");
            }
        }

       
    }
}