using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace System.Web.Mvc
{
    /// <summary>
    /// FlashExtensions creates helper methods on a controller to make it
    /// easier to create "flash" messages.
    /// </summary>
    public static class FlashExtensions
    {
        public static string FlashSuccess(this ControllerBase controller)
        {
            return controller.TempData["success"] as String;
        }

        public static void FlashSuccess(this ControllerBase controller, string message)
        {
            controller.TempData["success"] = message;
        }

        public static string FlashWarning(this ControllerBase controller)
        {
            return controller.TempData["warning"] as String;
        }

        public static void FlashWarning(this ControllerBase controller, string message)
        {
            controller.TempData["warning"] = message;
        }

        public static string FlashError(this ControllerBase controller)
        {
            return controller.TempData["error"] as String;
        }

        public static void FlashError(this ControllerBase controller, string message)
        {
            controller.TempData["error"] = message;
        }
    }
}