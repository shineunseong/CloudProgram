using System;
using System.Windows;
using System.Windows.Threading;

namespace MediCloudDrive.Biz
{
    internal static class MediExtendMethod
    {
        private static Action EmptyDelegate = delegate () { };

        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }
    }
}