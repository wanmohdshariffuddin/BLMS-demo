using BLMS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLMS.CustomAttributes
{
    public class AlertNotification
    {
        #region ALERT FOR LICENSE
        public static string ShowAlertLicense(Enums.Alert obj, string message, string message2, string message3, string message4, string message5)
        {
            string alertDiv = null;

            switch (obj)
            {
                case Enums.Alert.WarningFive:
                    alertDiv = "<div class='alert alert-dark sunny-morning-gradient alert-dismissable' id='alert'><button type='button' class='close' data-dismiss='alert'>&times;" +
                        "</button><i class='fa fa-exclamation-circle'></i>" +
                        "<strong> Warning </strong>" +
                        "<li>" + message + "</li>" +
                        "<li>" + message2 + "</li>" +
                        "<li>" + message3 + " </li>" +
                        "<li>" + message4 + " </li>" +
                        "<li>" + message5 + " </li></div>";
                    break;
            }

            return alertDiv;
        }
        #endregion

        public static string ShowAlertAll(Enums.Alert obj, string message, string message2, string message3, string message4)
        {
            string alertDiv = null;

            switch (obj)
            {
                case Enums.Alert.WarningTwo:
                    alertDiv = "<div class='alert alert-dark sunny-morning-gradient alert-dismissable' id='alert'><button type='button' class='close' data-dismiss='alert'>&times;" +
                        "</button><i class='fa fa-exclamation-circle'></i>" +
                        "<strong> Warning </strong>" +
                        "<li>" + message + "</li>" +
                        "<li>" + message2 + "</li>" + message3 + message4 + "</div>";
                    break;
                case Enums.Alert.WarningThree:
                    alertDiv = "<div class='alert alert-dark sunny-morning-gradient alert-dismissable' id='alert'><button type='button' class='close' data-dismiss='alert'>&times;" +
                        "</button><i class='fa fa-exclamation-circle'></i>" +
                        "<strong> Warning </strong>" +
                        "<li>" + message + "</li>" +
                        "<li>" + message2 + "</li>" +
                        "<li>" + message3 + " </li>" + message4 + "</div>";
                    break;
                case Enums.Alert.WarningFour:
                    alertDiv = "<div class='alert alert-dark sunny-morning-gradient alert-dismissable' id='alert'><button type='button' class='close' data-dismiss='alert'>&times;" +
                        "</button><i class='fa fa-exclamation-circle'></i>" +
                        "<strong> Warning </strong>" +
                        "<li>" + message + "</li>" +
                        "<li>" + message2 + "</li>" +
                        "<li>" + message3 + " </li>" +
                        "<li>" + message4 + " </li></div>";
                    break;
            }

            return alertDiv;
        }

        public static string ShowAlert(Enums.Alert obj, string message)
        {
            string alertDiv = null;

            switch (obj)
            {
                case Enums.Alert.Success:
                    alertDiv = "<div class='alert alert-dark dusty-grass-gradient alert-dismissable' id='alert'><button type='button' class='close' data-dismiss='alert'>&times;</button><i class='fa fa-check-circle'></i><strong> Success </strong><li>" + message + "</li></div>";
                    break;
                case Enums.Alert.Danger:
                    alertDiv = "<div class='alert alert-dark young-passion-gradient alert-dismissable' id='alert'><button type='button' class='close' data-dismiss='alert'>&times;</button><i class='fa fa-ban'></i><strong> Error </strong><li>" + message + "</li></div>";
                    break;
                case Enums.Alert.Info:
                    alertDiv = "<div class='alert alert-dark winter-neva-gradient alert-dismissable' id='alert'><button type='button' class='close' data-dismiss='alert'>&times;</button><strong> Info!</ strong > " + message + "</a>.</div>";
                    break;
                case Enums.Alert.Warning:
                    alertDiv = "<div class='alert alert-dark sunny-morning-gradient alert-dismissable' id='alert'><button type='button' class='close' data-dismiss='alert'>&times;</button><i class='fa fa-exclamation-circle'></i><strong> Warning </strong><li>" + message + "</li></div>";
                    break;
                case Enums.Alert.Delete:
                    alertDiv = "<div class='alert alert-dark young-passion-gradient alert-dismissable' id='alert'><button type='button' class='close' data-dismiss='alert'>&times;</button><i class='fa fa-trash-alt'></i><strong> Delete </strong><li>" + message + "</li></div>";
                    break;
            }

            return alertDiv;
        }
    }
}
