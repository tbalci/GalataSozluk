using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Configuration;
using System.Xml;
namespace Tuanna.Lib
{
    public class AppSetting
    {
        /// <summary>
        /// UpdateAppSettings: It will update the app.Config file AppConfig key values
        /// </summary>
        /// <param name="KeyName">AppConfigs KeyName</param>
        /// <param name="KeyValue">AppConfigs KeyValue</param>
        /// <remarks></remarks>
        public static void UpdateAppSettings(string KeyName, string KeyValue)
        {
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            foreach (XmlElement xElement in XmlDoc.DocumentElement)
            {
                if (xElement.Name == "connectionStrings")
                {
                    foreach (XmlNode xNode in xElement.ChildNodes)
                    {
                        if (xNode.Attributes[0].Value == KeyName)
                        {
                            xNode.Attributes[1].Value = KeyValue;
                        }
                    }
                }
            }
            XmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
        }
    }
}