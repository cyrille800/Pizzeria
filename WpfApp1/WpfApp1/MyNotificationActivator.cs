// The GUID CLSID must be unique to your app. Create a new GUID if copying this code.

using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Runtime.InteropServices;
using static Microsoft.Toolkit.Uwp.Notifications.NotificationActivator;

[ClassInterface(ClassInterfaceType.None)]
[ComSourceInterfaces(typeof(INotificationActivationCallback))]
[Guid("21EC2020-3AEA-1069-A2DD-08002b30309d"), ComVisible(true)]
public class MyNotificationActivator : NotificationActivator
{
    public override void OnActivated(string invokedArgs, NotificationUserInput userInput, string appUserModelId)
    {
        // TODO: Handle activation
    }
}